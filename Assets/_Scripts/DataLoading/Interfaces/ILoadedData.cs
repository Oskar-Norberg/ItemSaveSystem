using System.Collections.Generic;

namespace _Project.SaveSystem.Interfaces.DataLoading
{
    public interface ILoadedData
    {
        public bool TryGetDataByGUID(SerializableGuid guid, out Dictionary<string, SaveData> data);
    }
}