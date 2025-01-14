using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace OrderManagementSystemServer.Repository
{

    [Serializable]
    public class User : INotifyPropertyChanged
    {

        public enum ApprovalStates
        {
            Pending,
            Approved,
            Rejected
        }

        private int m_nId;
        private string m_stName;
        private string m_stEmail;
        private string m_stPhone;
        private string m_stPassword;
        private bool m_bIsAdmin;
        private bool m_bIsArchived;
        private ApprovalStates m_enApprovalStatus = ApprovalStates.Pending;

        [XmlElement]
        public int Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        [XmlElement]
        public string Name
        {
            get { return m_stName; }
            set
            {
                m_stName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        [XmlElement]
        public string Email
        {
            get { return m_stEmail; }
            set
            {
                m_stEmail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        [XmlElement]
        public string Phone
        {
            get { return m_stPhone; }
            set
            {
                m_stPhone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
            }
        }
        [XmlElement]
        public string Password
        {
            get { return m_stPassword; }
            set
            {
                m_stPassword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }
        [XmlElement]
        public bool IsAdmin
        {
            get { return m_bIsAdmin; }
            set
            {
                m_bIsAdmin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdmin)));
            }
        }

        [XmlElement]
        public bool IsArchived
        {
            get { return m_bIsArchived; }
            set
            {
                m_bIsArchived = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsArchived)));
            }
        }

        [XmlElement]
        public ApprovalStates ApprovalStatus
        {
            get { return m_enApprovalStatus; }
            set
            {
                m_enApprovalStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApprovalStatus)));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}