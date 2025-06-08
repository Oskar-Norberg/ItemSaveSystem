using System;
using Newtonsoft.Json;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public class SerializableGuidConverter : JsonConverter<SerializableGuid>
    {
        public override void WriteJson(JsonWriter writer, SerializableGuid value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GuidString);
        }

        public override SerializableGuid ReadJson(JsonReader reader, Type objectType, SerializableGuid existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var guidString = reader.Value as string;
            return new SerializableGuid(guidString);
        }
    }
}