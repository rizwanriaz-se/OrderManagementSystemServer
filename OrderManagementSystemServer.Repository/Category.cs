using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;


namespace OrderManagementSystemServer.Repository
{
    [Serializable]
    //[XmlRoot("Categories")]
    public class Category : INotifyPropertyChanged
    {
        private int? m_nId;
        private string? m_stName;
        private string? m_stDescription;
        //private byte[]? m_Picture;

        [XmlElement("Id")]
        public int? Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        [XmlElement("Name")]
        public string? Name
        {
            get { return m_stName; }
            set
            {
                m_stName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        [XmlElement("Description")]
        public string? Description
        {
            get { return m_stDescription; }
            set
            {
                m_stDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
