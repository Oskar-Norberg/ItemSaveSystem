using System.Collections.Generic;

namespace _Project.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class SubSaveData
    {
        public SerializableGuid GUID;
        public SaveableType SaveableType;
        public Dictionary<string, SaveData> Data;

        public SubSaveData(SerializableGuid guid, SaveableType saveableType, Dictionary<string, SaveData> data)
        {
            GUID = guid;
            Data = data;
            SaveableType = saveableType;
        }
    }
}