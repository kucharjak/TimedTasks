using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TimedTasks.Utils
{
    public class XML
    {
        public static string Serialize<T>(T objectToSerialize)
        {
            using (var wr = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(wr, objectToSerialize);
                return wr.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            using (TextReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }
    }
}
