using System.IO;
using Newtonsoft.Json;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public class JSONSerializer : ISerializer
    {
        public string Serialize(object objectToSerialize)
        {
            JsonSerializer jsonSerializer = CreateJSONSerializer();
            StringWriter stringWriter = CreateStringWriter();
            JsonWriter writer = CreateJSONWriter(stringWriter);
            
            jsonSerializer.Serialize(writer, objectToSerialize);
            
            var json = stringWriter.ToString();
            
            writer.Close();
            stringWriter.Close();
            
            return json;
        }
        
        public T Deserialize<T>(string serializedObject)
        {
            JsonSerializer jsonSerializer = CreateJSONSerializer();
            StringReader stringReader = CreateStringReader(serializedObject);
            JsonReader reader = CreateJSONReader(stringReader);
            
            try
            {
                T deserializedObject = jsonSerializer.Deserialize<T>(reader);
                
                return deserializedObject;
            }
            catch (JsonReaderException e)
            {
                throw new InvalidDataException("Invalid data format.", e);
            }
            finally
            {
                reader.Close();
                stringReader.Close();
            }
        }
        
        private Newtonsoft.Json.JsonSerializer CreateJSONSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };
            
            var jsonSerializer = JsonSerializer.Create(settings);
            
            return jsonSerializer;
        }
        
        private JsonWriter CreateJSONWriter(StringWriter stringWriter)
        {
            var jsonTextWriter = new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented
            };
            return jsonTextWriter;
        }
        
        private JsonReader CreateJSONReader(StringReader stringReader)
        {
            var jsonTextReader = new JsonTextReader(stringReader);
            return jsonTextReader;
        }

        private StringWriter CreateStringWriter()
        {
            return new StringWriter();
        }

        private StringReader CreateStringReader(string serializedString)
        {
            return new StringReader(serializedString);
        }
    }
}