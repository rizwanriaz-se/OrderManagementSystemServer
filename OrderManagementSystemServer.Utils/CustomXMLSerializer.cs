using System.Xml.Serialization;

namespace OrderManagementSystemServer.Components.Utils
{
    public class CustomXMLSerializer
    {
        public static void SerializeToXml<T>(string filepath, T data)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StreamWriter streamWriter  = new StreamWriter(filepath)) {
                xmlSerializer.Serialize(streamWriter, data);
            }
        }

        public static T DeserializeFromXml<T>(string filepath)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StreamReader streamReader = new StreamReader(filepath))
            {
                return (T)xmlSerializer.Deserialize(streamReader);
            }
        }

    }
}

