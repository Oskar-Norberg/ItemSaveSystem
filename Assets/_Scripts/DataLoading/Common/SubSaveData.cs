using System.Collections.Generic;

namespace _Project.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class SubSaveData
    {
        public Dictionary<string, SaveData> Data;

        public SubSaveData(Dictionary<string, SaveData> data)
        {
            Data = data;
        }
    }
}