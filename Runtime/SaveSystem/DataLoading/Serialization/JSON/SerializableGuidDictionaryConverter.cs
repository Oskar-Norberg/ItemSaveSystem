using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ringo.SaveSystem.GUID;

namespace ringo.SaveSystem.DataLoading.Serialization.JSON
{
    public class SerializableGuidDictionaryConverter : JsonConverter
    {
        private const string GuidPropertyName = "GUID";
        private const string ValuePropertyName = "Value";
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectType = value.GetType();
            var valueType = objectType.GetGenericArguments()[1];
            var dict = (System.Collections.IDictionary) value;

            writer.WriteStartArray();
            
            foreach (System.Collections.DictionaryEntry entry in dict)
            {
                writer.WriteStartObject();
                
                writer.WritePropertyName(GuidPropertyName);
                serializer.Serialize(writer, entry.Key);
                
                writer.WritePropertyName(ValuePropertyName);
                serializer.Serialize(writer, entry.Value, valueType);
                
                writer.WriteEndObject();
            }
            
            writer.WriteEndArray();
        }

        // Slightly large function, possibly could be split up for readability.
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueType = objectType.GetGenericArguments()[1];
            var result = (System.Collections.IDictionary) Activator.CreateInstance(objectType);

            if (reader.TokenType is JsonToken.None or not JsonToken.StartArray)
                throw new JsonSerializationException("Expected StartArray token.");

            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                if (reader.TokenType != JsonToken.StartObject)
                    throw new JsonSerializationException("Expected StartObject token.");

                SerializableGuid key = null;
                object value = null;

                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType != JsonToken.PropertyName)
                        throw new JsonSerializationException("Expected PropertyName token.");
                    

                    switch ((string) reader.Value)
                    {
                        case GuidPropertyName:
                            reader.Read();
                            key = serializer.Deserialize<SerializableGuid>(reader);
                            break;
                        case ValuePropertyName:
                            reader.Read();
                            value = serializer.Deserialize(reader, valueType);
                            break;
                        default:
                            throw new JsonSerializationException($"Unexpected property '{reader.Value}'.");
                    }
                }

                result.Add(key, value);
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            // Is generic type of Dictionary<SerializableGuid, T>
            return objectType.IsGenericType
                   && objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>)
                   && objectType.GetGenericArguments()[0] == typeof(SerializableGuid);
        }
    }
}