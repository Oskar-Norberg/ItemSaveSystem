using System.Collections.Generic;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public class JSONSerializer : ISerializer
    {
        public bool Serialize(List<Saveable> saveables, string path)
        {
            throw new System.NotImplementedException();
        }

        public ILoadedData Deserialize(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}