using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace EmagineServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class DuplexDrawService : IDuplexDrawService
    {

        //Object for using with Sync
        object syncRoot = new object();

        //Dictionary for keeping subscribed clients
        Dictionary<string, IDuplexDrawCallback> clients = new Dictionary<string, IDuplexDrawCallback>();
        Dictionary<string, string> userNames = new Dictionary<string, string>();

        //Raised when notification is completed
        static AsyncCallback onNotifyCompleted = new AsyncCallback(OnNotifyCompleted);


        static TypedMessageConverter messageConverter = TypedMessageConverter.Create(
                   typeof(DrawData),
                   DrawData.DrawAction,
                   "http://schemas.datacontract.org/2004/07/EmagineClient");



        /// <summary>
        /// Regsiter a client with our service
        /// </summary>
        /// <param name="name"></param>
        public void Register(string name)
        {
            string sessionId = OperationContext.Current.Channel.SessionId;

            lock (syncRoot)
            {
                clients[sessionId] = Callback;
                userNames[sessionId] = name;
            }

            OperationContext.Current.Channel.Faulted += new EventHandler(this.OnUnsubscribe);
            OperationContext.Current.Channel.Closed += new EventHandler(this.OnUnsubscribe);
            lock (syncRoot)
            {
                NotifyClients("@added:" + name, "Server", "");
            }

        }

        /// <summary>
        /// Draw the line in other user's screen
        /// </summary>
        /// <param name="name"></param>
        public void Draw(string data)
        {
            lock (this.syncRoot)
            {
                string sessionId = OperationContext.Current.Channel.SessionId;
                if (userNames.ContainsKey(sessionId))
                    NotifyClients("@draw:" + data, userNames[sessionId], sessionId);
            }
        }




        /// <summary>
        /// Current Callback
        /// </summary>
        IDuplexDrawCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IDuplexDrawCallback>();
            }
        }



        /// <summary>
        /// Handle any errors during notifying a client
        /// </summary>
        /// <param name="result"></param>
        static void OnNotifyCompleted(IAsyncResult result)
        {
            try
            {
                ((IDuplexDrawCallback)(result.AsyncState)).EndNotify(result);
            }
            catch (CommunicationException)
            {
                // empty
            }
            catch (TimeoutException)
            {
                // empty
            }
        }

        /// <summary>
        /// Remove any dead channels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUnsubscribe(object sender, EventArgs e)
        {
            IContextChannel channel = (IContextChannel)sender;
            if (channel != null && channel.SessionId != null)
            {
                lock (this.syncRoot)
                {
                    if (clients.ContainsKey(channel.SessionId))
                    {
                        clients.Remove(channel.SessionId);
                        userNames.Remove(channel.SessionId);
                    }
                }
            }
        }

        /// <summary>
        /// Send the notification to all clients
        /// </summary>
        /// <param name="bundle"></param>
        public void NotifyClients(string bundle, string from, string sessionId)
        {
            MessageBuffer notificationMessageBuffer = messageConverter.ToMessage(new DrawData { Content = bundle, From = from }).CreateBufferedCopy(65536);
            foreach (var client in clients.Values)
            {
                try
                {
                    client.BeginNotify(notificationMessageBuffer.CreateMessage(), onNotifyCompleted, client);
                }
                catch
                {
                }
            }


        }


    }
}
