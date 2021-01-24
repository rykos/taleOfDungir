using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace taleOfDungir.Helpers
{
    public static class SystemHelper
    {
        public static int RandomSign()
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) * 2 - 1;
        }

        public static byte[] Serialize(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        public static T Deserialize<T>(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return default;
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(buffer);
            return (T)formatter.Deserialize(memoryStream);
        }
    }
}