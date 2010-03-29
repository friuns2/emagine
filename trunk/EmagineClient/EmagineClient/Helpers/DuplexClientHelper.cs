using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Globalization;
using System.ComponentModel;

namespace EmagineClient
{

    public enum CallBackEvents
    {
        NotifyReceived,
        RegisterCompleted,
        DrawCompleted
    }

    /// <summary>
    /// A helper class to initialize the proxy and receive call backs
    /// </summary>
    public class DuplexClientHelper<TDataType>
    {
        #region Variables
        object syncRoot = new object();
        DuplexDrawServiceClient client = null;
        #endregion


        #region Events
        public event Action<string,Exception> GotError;
        public event Action<CallBackEvents, TDataType> GotNotification;
        #endregion


        #region Methods
        public void Initialize(string endPointAddress)
        {
            this.client = new DuplexDrawServiceClient(
            new PollingDuplexHttpBinding(),
            new EndpointAddress(endPointAddress));

            this.client.NotifyReceived += new EventHandler<NotifyReceivedEventArgs>(client_NotifyReceived);
            this.client.RegisterCompleted += new EventHandler<AsyncCompletedEventArgs>(client_RegisterCompleted);
        }


        /// <summary>
        /// Current client instance
        /// </summary>
        public DuplexDrawServiceClient Client
        {
            get
            {
                return client;
            }
        }



        /// <summary>
        /// Callback to get the notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_NotifyReceived(object sender, NotifyReceivedEventArgs e)
        {
            if (!this.IsError(e))
            {
                try
                {
                    if (GotNotification != null)
                    {
                        var data = e.request.GetBody<TDataType>();
                        GotNotification(CallBackEvents.NotifyReceived, data);
                    }
                }
                catch (Exception ex)
                {
                    if (GotError != null)
                        GotError(ex.Message,ex);

                }
            }
        }

        /// <summary>
        /// Callback to get the register completed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_RegisterCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!this.IsError(e))
            {
                if (GotNotification != null)
                    GotNotification(CallBackEvents.RegisterCompleted, default(TDataType));
            }
        }

        /// <summary>
        /// Check if we have an error
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        bool IsError(AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lock (this.syncRoot)
                {
                    this.client.CloseAsync();
                    this.client = null;
                }
                if (GotError!=null)
                    GotError(e.Error.Message,e.Error);
            }


            return e.Error != null;
        }

        #endregion

    }
}
