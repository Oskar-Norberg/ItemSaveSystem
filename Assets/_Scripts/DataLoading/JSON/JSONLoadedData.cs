using System.Collections.Generic;

namespace _Project.SaveSystem.Interfaces.DataLoading.JSON
{
    public struct JSONLoadedData : ILoadedData
    {
        // String is the GUID of the saveable.
        private Dictionary<SerializableGuid, SubJSONContainer> _saveDatas;

        public JSONLoadedData(HeadJSONContainer headJsonContainer)
        {
            _saveDatas = new();
            
            // Populate _saveDatas with all subcontainers.
            foreach (var subContainer in headJsonContainer.SubContainers)
            {
                _saveDatas[subContainer.GUID] = subContainer;
            }
        }

        public bool TryGetDataByGUID(SerializableGuid guid, out Dictionary<string, SaveData> data)
        {
            if (_saveDatas.TryGetValue(guid, out var subContainer))
            {
                data = subContainer.Data;
                return true;
            }
        
            data = null;
            return false;
        }
    }
}