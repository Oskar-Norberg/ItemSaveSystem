using System;
using System.Collections.Generic;
using ringo.SaveSystem.GUID;

namespace ringo.SaveSystem.DataLoading.Common
{
    [Serializable]
    public class HeadSaveData : ILoadedData
    {
        // TODO: Any way I can have this private and still serialize it?
        public Dictionary<SerializableGuid, object> _saveDatas = new();
        
        public HeadSaveData(Dictionary<SerializableGuid, object> saveDatas)
        {
            _saveDatas = saveDatas;
        }

        public HeadSaveData() : this(new())
        {
        }

        public void AddSubContainer(SerializableGuid type, object data)
        {
            _saveDatas.Add(type, data);
        }
        
        public void AddData(SerializableGuid guid, object data)
        {
            AddSubContainer(guid, data);
        }

        public bool TryGetSubsystemData(SerializableGuid guid, out object data)
        {
            return _saveDatas.TryGetValue(guid, out data);
        }
    }
}