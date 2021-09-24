using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace FileOperations
{
    public static class SavindLoading
    {
        private static JsonSerializerSettings GetJSONSetstings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        private static XmlSerializer Serializer(object obj)
        {
            return new XmlSerializer(obj.GetType());
        }
        private static XmlSerializer Serializer<T>()
        {
            return new XmlSerializer(typeof(T));
        }
        public static void SaveJSON(object obj, string path)
        {
            string jstring = JsonConvert.SerializeObject(obj, Formatting.Indented, GetJSONSetstings());
            File.WriteAllText(path, jstring);
        }

        public static void SaveXML(object obj, string path)
        {
            XmlSerializer serX = new XmlSerializer(obj.GetType());
            TextWriter w = new StreamWriter(path);
            serX.Serialize(w, obj);
            w.Close();
        }

        public static T LoadJSON<T>(string path)
        {
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(jsonString, GetJSONSetstings());
        }

        public static T LoadXML<T>(string path)
        {
            TextReader r = new StreamReader(path);
            T result = (T)Serializer<T>().Deserialize(r);
            r.Close();
            return result;
        }
    }
}
