using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace OrderManagementSystemServer.Repository
{
    [Serializable]
    public class OrderDetail : INotifyPropertyChanged
    {

        private Product m_Product;

        [XmlElement]
        public Product Product
        {
            get { return m_Product; }
            set
            {
                m_Product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        [XmlElement]
        private int m_Quantity;

        public int Quantity
        {
            get
            {
                return m_Quantity;
            }
            set
            {
                m_Quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not OrderDetail other) return false;

            return Product.Id == other.Product.Id && Quantity == other.Quantity;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
