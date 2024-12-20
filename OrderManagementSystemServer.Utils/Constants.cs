using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemServer
{
    public static class Constants
    {
        //TCP /IP Constants
        public const string IPAddress = "127.0.0.1";
        public const int Port = 4444;
        public const int BufferSize = 25600;
        public const int HeartbeatInterval = 5000;

        //XML Directory Path
        public const string XMLDirectoryPath = "C:\\Users\\rriaz\\source\\repos\\OrderManagementSystemDataStore\\";

        //XML DataStore Paths
        public const string UserDataStoreName = "UserDataStore.xml";
        public const string CategoryDataStoreName = "CategoryDataStore.xml";
        public const string ProductDataStoreName = "ProductDataStore.xml";
        public const string OrderDataStoreName = "OrderDataStore.xml";

    }
}
