using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;

namespace EmagineServer
{
    [MessageContract]
    public class DrawData
    {
        public const string DrawAction = "http://amazedsaint.net/SilverPaint/draw";

        [MessageBodyMember]
        public string Content { get; set; }

        [MessageBodyMember]
        public string From { get; set; }
    }
}