using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemServer
{
    public class Enums
    {
        public enum MessageType
        {
            Category,
            Order,
            Product,
            User,
            Error
        }
        public enum MessageAction
        {
            Add,
            Update,
            Delete,
            Load,
            Error

        }


    }
}
