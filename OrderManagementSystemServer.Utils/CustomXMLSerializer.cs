using System.Xml.Serialization;

namespace OrderManagementSystemServer.Components.Utils
{
    public class CustomXMLSerializer
    {
        public static void SerializeToXml<T>(string filepath, T data)
        {

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (StreamWriter streamWriter = new StreamWriter(filepath))
                {
                    xmlSerializer.Serialize(streamWriter, data);
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Error trying to serialize data to XML.");
                return;
            }
        }

        public static T DeserializeFromXml<T>(string filepath)
        {

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    return (T)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error trying to deserialize data from XML.");
                return default;
            }
        }

    }
}

