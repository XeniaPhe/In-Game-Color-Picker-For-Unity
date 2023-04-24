using System.IO;
using UnityEngine;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Xenia.ColorPicker.Serialization
{
    internal static class SerializationManager
    {
        private static readonly string SerializationDir = Application.persistentDataPath + "/ColorPicker";

        internal static void Serialize<T>(T data) where T : ISerializable
        {
            if (!Directory.Exists(SerializationDir))
                Directory.CreateDirectory(SerializationDir);

            StringBuilder pathBuilder = new StringBuilder(SerializationDir);
            pathBuilder.Append('/');
            pathBuilder.Append(data.NonVolatileReference);

            string subDir = pathBuilder.ToString();

            if(!Directory.Exists(subDir))
                Directory.CreateDirectory(subDir);

            byte[] hash = Encoding.ASCII.GetBytes(typeof(T).ToString());

            pathBuilder.Append('/');

            foreach (var item in hash)
                pathBuilder.Append(item.ToString("x2"));

            pathBuilder.Append(".bin");
            string file = pathBuilder.ToString();

            using FileStream fs = File.Open(file, FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, data);
        }

        internal static bool Deserialize<T>(ref T data) where T : ISerializable
        {
            if (!Directory.Exists(SerializationDir))
            {
                Directory.CreateDirectory(SerializationDir);
                return false;
            }
            
            StringBuilder pathBuilder= new StringBuilder(SerializationDir);
            pathBuilder.Append('/');
            pathBuilder.Append(data.NonVolatileReference);

            string subDir = pathBuilder.ToString();

            if (!Directory.Exists(subDir))
            {
                Directory.CreateDirectory(subDir);
                return false;
            }

            byte[] hash = Encoding.ASCII.GetBytes(typeof(T).ToString());

            pathBuilder.Append('/');

            foreach (var item in hash)
                pathBuilder.Append(item.ToString("x2"));

            pathBuilder.Append(".bin");

            string file = pathBuilder.ToString();

            if (!File.Exists(file))
                return false;

            using FileStream fs = File.Open(file, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            data = (T)formatter.Deserialize(fs);

            return true;
        }
    }
}
