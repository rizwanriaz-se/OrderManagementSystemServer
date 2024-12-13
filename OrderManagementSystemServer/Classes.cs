using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemServer
{
    public class Classes
    {
        public class Request
        {
            public Enums.MessageType MessageType { get; set; }
            public Enums.MessageAction MessageAction { get; set; }
            public object Data { get; set; }
        }

        public class Response
        {
            public Enums.MessageType MessageType { get; set; }
            public Enums.MessageAction MessageAction { get; set; }
            public object Data { get; set; }
        }

    }
}
