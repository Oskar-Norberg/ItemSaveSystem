using System;
using Newtonsoft.Json;
using UnityEngine;

namespace ringo.SaveSystem.DataLoading.Serialization.JSON.UnityConverters
{
    public class QuaternionConverter : JsonConverter<Quaternion>
    {
        private const string XPropertyName = "X";
        private const string YPropertyName = "Y";
        private const string ZPropertyName = "Z";
        private const string WPropertyName = "W";
        
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName(XPropertyName);
            writer.WriteValue(value.x);

            writer.WritePropertyName(YPropertyName);
            writer.WriteValue(value.y);

            writer.WritePropertyName(ZPropertyName);
            writer.WriteValue(value.z);

            writer.WritePropertyName(WPropertyName);
            writer.WriteValue(value.w);

            writer.WriteEndObject();
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException("Expected StartObject token.");
            }

            float x = 0, y = 0, z = 0, w = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonToken.PropertyName)
                {
                    throw new JsonSerializationException("Expected PropertyName token.");
                }

                string propertyName = reader.Value?.ToString() ??
                                      throw new JsonSerializationException("Property name is null.");

                if (!reader.Read())
                {
                    throw new JsonSerializationException($"Unexpected end when reading value for {propertyName}.");
                }

                if (reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Integer)
                {
                    throw new JsonSerializationException($"Invalid value type for property {propertyName}.");
                }

                float value = Convert.ToSingle(reader.Value);

                switch (propertyName)
                {
                    case XPropertyName:
                        x = value;
                        break;
                    case YPropertyName:
                        y = value;
                        break;
                    case ZPropertyName:
                        z = value;
                        break;
                    case WPropertyName:
                        w = value;
                        break;
                    default:
                        throw new JsonSerializationException($"Unexpected property {propertyName} in Vector3.");
                }
            }

            return new Quaternion(x, y, z, w);
        }
    }
}