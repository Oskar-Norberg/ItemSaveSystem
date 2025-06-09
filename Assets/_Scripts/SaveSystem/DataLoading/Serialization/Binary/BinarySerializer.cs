using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ringo.SaveSystem.Exceptions;

namespace ringo.SaveSystem.DataLoading.Serialization.Binary
{
    public class BinarySerializer : ISerializer
    {
        public string Serialize<T>(T objectToSerialize)
        {
            using var stream = new MemoryStream();
            
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectToSerialize);
            return Convert.ToBase64String(stream.ToArray());
        }

        public T Deserialize<T>(string serializedObject)
        {
            try
            {
                byte[] data = Convert.FromBase64String(serializedObject);
                using var stream = new MemoryStream(data);
            
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(stream);
            }
            catch (FormatException)
            {
                throw new InvalidSaveException("Invalid data format.");
            }
        }
    }
}