using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public class JSONSerializer : ISerializer
    {
        private const string FileExtension = ".json";
        
        public bool Serialize(object objectToSerialize, string path)
        {
            // TOOD: Factory method to create the correct JSONContainer.
            JsonSerializer jsonSerializer = new JsonSerializer();
            StreamWriter streamWriter = new StreamWriter(GetPathString(path));
            JsonWriter writer = new JsonTextWriter(streamWriter);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
            writer.Formatting = Formatting.Indented;
            
            jsonSerializer.Serialize(writer, objectToSerialize);
            
            writer.Close();
            streamWriter.Close();

            return true;
        }

        public T Deserialize<T>(string path)
        {
            if (!File.Exists(GetPathString(path)))
            {
                // TODO: Change this to a TryDeserialize method that returns a bool. (This should return false)
                return default;
            }
            
            // TODO: Move this to a separate Serializer/Deserializer class.
            JsonSerializer jsonSerializer = new JsonSerializer();
            StreamReader streamReader = new StreamReader(GetPathString(path));
            JsonReader reader = new JsonTextReader(streamReader);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
                
            T deserializedObject = jsonSerializer.Deserialize<T>(reader);
            
            reader.Close();
            streamReader.Close();
            
            return deserializedObject;
        }
        
        private string GetPathString(string path)
        {
            return path + FileExtension;
        }
    }
}