﻿using System;
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
            HeartBeat,
            Error
        }
        public enum MessageAction
        {
            Add,
            Update,
            Delete,
            HeartBeat,
            Error

        }


    }
}
