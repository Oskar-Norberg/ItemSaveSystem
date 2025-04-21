using System.IO;
using Newtonsoft.Json;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public class JSONSerializer : ISerializer
    {
        public string Serialize(object objectToSerialize)
        {
            // TOOD: Factory method to create the correct JSONContainer.
            JsonSerializer jsonSerializer = new JsonSerializer();
            StringWriter stringWriter = new StringWriter();
            JsonWriter writer = new JsonTextWriter(stringWriter);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
            writer.Formatting = Formatting.Indented;
            
            jsonSerializer.Serialize(writer, objectToSerialize);
            
            var json = stringWriter.ToString();
            
            writer.Close();
            stringWriter.Close();
            
            return json;
        }
        
        public T Deserialize<T>(string serializedObject)
        {
            // TODO: Same as other todo. Please create a factory method to save settings.
            JsonSerializer jsonSerializer = new JsonSerializer();
            StringReader stringReader = new StringReader(serializedObject);
            JsonReader reader = new JsonTextReader(stringReader);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
                
            T deserializedObject = jsonSerializer.Deserialize<T>(reader);
            
            reader.Close();
            stringReader.Close();
            
            return deserializedObject;
        }
    }
}