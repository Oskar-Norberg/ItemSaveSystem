using System.Collections.Generic;

namespace _Project.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class SubSaveData
    {
        public SerializableGuid GUID;
        public Dictionary<string, SaveData> Data;

        public SubSaveData(SerializableGuid guid, Dictionary<string, SaveData> data)
        {
            GUID = guid;
            Data = data;
        }
    }
}