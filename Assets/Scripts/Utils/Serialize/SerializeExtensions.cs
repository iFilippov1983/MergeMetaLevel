using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Utils.Serialize
{
    public static class SerializeExtensions
    {
        public static string ToJson<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
        
        public static T DeepCopy<T>(this T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
 
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T[] Copy<T>(this T[] array)
        {
            T[] res = new T[array.Length]; 
            Array.Copy(array, res, array.Length);
            return res;
        }
    }
}