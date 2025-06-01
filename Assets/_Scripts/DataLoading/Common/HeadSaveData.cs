using System;
using System.Collections.Generic;
using _Project.SaveSystem.Interfaces.DataLoading;

namespace _Project.SaveSystem.DataLoading.Common
{
    [Serializable]
    public class HeadSaveData : ILoadedData
    {
        // TODO: Any way I can have this private and still serialize it?
        public Dictionary<Type, object> _saveDatas = new();
        
        public HeadSaveData(Dictionary<Type, object> saveDatas)
        {
            _saveDatas = saveDatas;
        }

        public HeadSaveData() : this(new Dictionary<Type, object>())
        {
        }

        public void AddSubContainer(Type type, object data)
        {
            _saveDatas.Add(type, data);
        }
        
        public void AddData<T>(object data)
        {
            AddSubContainer(typeof(T), data);
        }

        public bool TryGetSubsystemData(Type t, out object data)
        {
            return _saveDatas.TryGetValue(t, out data);
        }

        public bool TryGetSubsystemData<T>(out T data)
        {
            if (_saveDatas.TryGetValue(typeof(T), out var value) && value is T typedValue)
            {
                data = typedValue;
                return true;
            }

            data = default;
            return false;
        }
    }
}