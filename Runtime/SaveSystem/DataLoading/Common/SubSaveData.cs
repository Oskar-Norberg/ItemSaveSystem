using System.Collections.Generic;

namespace ringo.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class SubSaveData
    {
        public Dictionary<string, object> Data;

        public SubSaveData(Dictionary<string, object> data)
        {
            Data = data;
        }
    }
}