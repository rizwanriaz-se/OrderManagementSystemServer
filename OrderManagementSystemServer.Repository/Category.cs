//using DevExpress.XtraExport.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        private byte[]? m_Picture;

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

        [XmlElement("Picture")]
        public byte[]? Picture
        {
            get { return m_Picture; }
            set
            {
                m_Picture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Picture)));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not Category other) return false;

            return Id == other.Id && Name == other.Name && Description == other.Description;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
