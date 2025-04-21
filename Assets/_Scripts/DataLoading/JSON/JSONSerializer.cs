using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public class JSONSerializer : ISerializer
    {
        private const string FileExtension = ".json";
        
        public bool Serialize(List<Saveable> saveables, string path)
        {
            HeadJSONContainer headJSONContainer = new HeadJSONContainer();
            
            foreach (var saveable in saveables)
            {
                var saveDatas = saveable.GetSaveData();
            
                SubJSONContainer container = new SubJSONContainer
                {
                    GUID = saveable.GUID,
                    SaveableType = saveable.SaveableType,
                    Data = saveDatas
                };
                
                headJSONContainer.AddSubContainer(container);
            }
            
            JsonSerializer jsonSerializer = new JsonSerializer();
            StreamWriter streamWriter = new StreamWriter(GetPathString(path));
            JsonWriter writer = new JsonTextWriter(streamWriter);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
            writer.Formatting = Formatting.Indented;
            
            jsonSerializer.Serialize(writer, headJSONContainer);
            
            writer.Close();
            streamWriter.Close();

            return true;
        }

        public ILoadedData Deserialize(string path)
        {
            if (!File.Exists(GetPathString(path)))
            {
                // TODO: Change this to a TryDeserialize method that returns a bool. (This should return false)
                return null;
            }
            
            // TODO: Move this to a separate Serializer/Deserializer class.
            JsonSerializer jsonSerializer = new JsonSerializer();
            StreamReader streamReader = new StreamReader(GetPathString(path));
            JsonReader reader = new JsonTextReader(streamReader);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
                
            HeadJSONContainer headJSONContainer = jsonSerializer.Deserialize<HeadJSONContainer>(reader);
            
            reader.Close();
            streamReader.Close();
            
            return new JSONLoadedData(headJSONContainer);
        }
        
        private string GetPathString(string path)
        {
            return path + FileExtension;
        }
    }
}