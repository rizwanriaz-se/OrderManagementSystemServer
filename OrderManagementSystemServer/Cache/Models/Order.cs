using OrderManagementSystemServer.Cache.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
//using System.Runtime.Serialization;


namespace OrderManagementSystemServer.Cache.Models
{
    [Serializable]
    public class Order : INotifyPropertyChanged
    {

        public enum OrderStatus
        {
            Pending,
            Shipped,
            Delivered
        }

        private int? m_nId;
        private User? m_User;
        private DateTime? m_OrderDate;
        private OrderStatus? m_enStatus;
        private Dictionary<Product, int> m_Products;
        private DateTime? m_ShippedDate;
        private string? m_stShippingAddress;

        [XmlElement]
        public int? Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        [XmlElement]
        public User? User
        {
            get { return m_User; }
            set
            {
                m_User = value;
                OnPropertyChanged(nameof(User));
            }
        }

        [XmlElement]
        public DateTime? OrderDate
        {
            get { return m_OrderDate; }
            set
            {
                m_OrderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        [XmlElement]
        public OrderStatus? Status
        {
            get { return m_enStatus; }
            set
            {
                m_enStatus = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private ObservableCollection<OrderDetail> m_OrderDetails;

        [XmlArray("OrderDetails")] // Matches the XML tag
        [XmlArrayItem("OrderDetail")] // Matches individual elements
        public ObservableCollection<OrderDetail> OrderDetails
        {
            get { return m_OrderDetails; }
            set
            {
                m_OrderDetails = value;
                OnPropertyChanged(nameof(OrderDetails));
            }
        }

        //public Dictionary<Product, int> Products {
        //    get { return m_Products; }
        //    set
        //    {
        //        m_Products = value;
        //        OnPropertyChanged(nameof(Products));
        //        OnPropertyChanged(nameof(ProductRows)); // Notify that ProductRows has changed
        //    }
        //}

        [XmlElement]
        public DateTime? ShippedDate
        {
            get { return m_ShippedDate; }
            set
            {
                m_ShippedDate = value;
                OnPropertyChanged(nameof(ShippedDate));
            }
        }

        [XmlElement]
        public string? ShippingAddress
        {
            get { return m_stShippingAddress; }
            set
            {
                m_stShippingAddress = value;
                OnPropertyChanged(nameof(ShippingAddress));
            }
        }

        // New property to expose Products as a collection
        //public ObservableCollection<ProductRow> ProductRows
        //{
        //    get
        //    {
        //        return new ObservableCollection<ProductRow>(
        //            m_Products?.Select(p => new ProductRow
        //            {
        //                SelectedProduct = p.Key,
        //                Quantity = p.Value
        //            }) ?? Enumerable.Empty<ProductRow>()
        //        );
        //    }
        //}

        public override bool Equals(object obj)
        {
            if (obj is not Order other) return false;

            return Id == other.Id && User == other.User && OrderDate == other.OrderDate && Status == other.Status && OrderDetails == other.OrderDetails && ShippingAddress == other.ShippingAddress && ShippedDate == other.ShippedDate;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
