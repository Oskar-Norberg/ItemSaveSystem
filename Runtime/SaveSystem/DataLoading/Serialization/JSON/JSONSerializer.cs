using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ringo.SaveSystem.DataLoading.Serialization.JSON.UnityConverters;

namespace ringo.SaveSystem.DataLoading.Serialization.JSON
{
    public class JSONSerializer : ISerializer
    {
        public string Serialize<T>(T objectToSerialize)
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
            catch (ArgumentNullException e)
            {
                throw new InvalidDataException("Serialized data is null.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new InvalidDataException("Failed to deserialize data.", e);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidDataException("Invalid data format.", e);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("An unexpected error occurred during deserialization.", e);
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
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                Converters = GetJsonConverters(),
                // SerializationBinder = new JSONSaveDataBinder()
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
        
        private IList<JsonConverter> GetJsonConverters()
        {
            var converters = new List<JsonConverter>
            {
                // SaveManager Specific types
                new SerializableGuidConverter(),
                new SerializableGuidDictionaryConverter(),
                
                // Unity Specific Types
                new Vector3Converter(),
                new QuaternionConverter(),
            };
            return converters;
        }
    }
}