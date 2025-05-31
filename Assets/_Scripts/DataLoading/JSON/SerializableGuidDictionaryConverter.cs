using System.Linq;
using _Project.SaveSystem.DataLoading.Common;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class SerializableGuidDictionaryConverter<T> : JsonConverter<Dictionary<SerializableGuid, T>> 
    {
        public override void WriteJson(JsonWriter writer, Dictionary<SerializableGuid, T> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            
            foreach (var kvp in value)
            {
                writer.WriteStartObject();
                
                writer.WritePropertyName("Key");
                serializer.Serialize(writer, kvp.Key);
                
                writer.WritePropertyName("Value");
                serializer.Serialize(writer, kvp.Value);
                
                writer.WriteEndObject();
            }
            
            writer.WriteEndArray();
        }

        public override Dictionary<SerializableGuid, T> ReadJson(JsonReader reader, Type objectType, Dictionary<SerializableGuid, T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            // TODO: Handle edge case where list is empty or null
            var list = serializer.Deserialize<List<KeyValuePair<SerializableGuid, T>>>(reader);
            return list.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}