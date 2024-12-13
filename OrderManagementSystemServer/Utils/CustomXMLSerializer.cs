using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OrderManagementSystemServer.Utils
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

