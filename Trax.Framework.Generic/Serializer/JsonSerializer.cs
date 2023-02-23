
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Trax.Framework.Generic.Serializer
{
    public class JsonSerializer
    {
        public static T UnserializeFile<T>(string FilePath)
        {
            if (!System.IO.File.Exists(FilePath))
                throw new Exception("File not found");

            T genericObject;
            using (StreamReader file = File.OpenText(FilePath))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //};
                genericObject = (T)serializer.Deserialize(file, typeof(T));
            }
            return genericObject;
        }

        public static T Deserialize<T>(string SerializedString)
        {
            T o = JsonConvert.DeserializeObject<T>(SerializedString);
            return o;
        }

        public static void SerializeToFile<T>(T ObjectToSerialize, string FilePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                throw new Exception("FilePath directory not found");

            var serializer = new DataContractJsonSerializer(typeof(T));
            var _MemoryStream = new MemoryStream();
            serializer.WriteObject(_MemoryStream, ObjectToSerialize);
            var JsonTradeItem = Encoding.UTF8.GetString(_MemoryStream.ToArray());
            if (File.Exists(FilePath))
                File.Delete(FilePath);

            File.WriteAllText(FilePath, JsonTradeItem, Encoding.UTF8);
        }

        public static string Serialize<T>(T ObjectToSerialize)
        {
            //var serializer = new DataContractJsonSerializer(typeof(T));
            //var _MemoryStream = new MemoryStream();
            //serializer.WriteObject(_MemoryStream, ObjectToSerialize);
            //var JsonTradeItem = Encoding.UTF8.GetString(_MemoryStream.ToArray());
            //return JsonTradeItem;
            return JsonConvert.SerializeObject(ObjectToSerialize, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeXML<T>(T Object)
        {
            string _Payload = string.Empty;
            var _XmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter _StringWriter = new Utf8StringWriter())
            {
                _XmlSerializer.Serialize(_StringWriter, Object);
                _Payload = _StringWriter.ToString();
            }
            return _Payload;
        }

        public static T DesserializeXML<T>(string ObjectString)
        {
            T _Object = default(T);
            var _XmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream _MemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ObjectString)))
            {
                _Object = (T)_XmlSerializer.Deserialize(_MemoryStream);
            }
            return _Object;
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
