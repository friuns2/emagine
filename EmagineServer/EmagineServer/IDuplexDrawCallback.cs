using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace EmagineServer
{
    [ServiceContract]
    public interface IDuplexDrawCallback
    {
        [OperationContract(IsOneWay = true, AsyncPattern = true, Action = DrawData.DrawAction)]
        IAsyncResult BeginNotify(Message message, AsyncCallback callback, object state);
        void EndNotify(IAsyncResult result);
    }
}