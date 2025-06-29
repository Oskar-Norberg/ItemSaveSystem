using System;
using Newtonsoft.Json;
using ringo.SaveSystem.DataLoading.Serialization.Common;
using UnityEngine;

namespace ringo.SaveSystem.DataLoading.Serialization.JSON.UnityConverters
{
    public class TransformDataConverter : JsonConverter<TransformData>
    {
        private const string PositionPropertyName = "Position";
        private const string RotationPropertyName = "Rotation";
        private const string ScalePropertyName = "Scale";
        
        public override void WriteJson(JsonWriter writer, TransformData value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName(PositionPropertyName);
            serializer.Serialize(writer, value.Position);
            
            writer.WritePropertyName(RotationPropertyName);
            serializer.Serialize(writer, value.Rotation);
            
            // TODO: Double check this is the correct local/global scale.
            writer.WritePropertyName(ScalePropertyName);
            serializer.Serialize(writer, value.Scale);
            
            writer.WriteEndObject();
        }

        public override TransformData ReadJson(JsonReader reader, Type objectType, TransformData existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException("Expected StartObject token.");
            }

            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Vector3 scale = Vector3.one;

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

                switch (propertyName)
                {
                    case PositionPropertyName:
                        position = serializer.Deserialize<Vector3>(reader);
                        break;
                    case RotationPropertyName:
                        rotation = serializer.Deserialize<Quaternion>(reader);
                        break;
                    case ScalePropertyName:
                        scale = serializer.Deserialize<Vector3>(reader);
                        break;
                    default:
                        throw new JsonSerializationException($"Unexpected property {propertyName} in Vector3.");
                }
            }
            
            return new TransformData
            {
                Position = position,
                Rotation = rotation,
                Scale = scale
            };
        }
    }
}