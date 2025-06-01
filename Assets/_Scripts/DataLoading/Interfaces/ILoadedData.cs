using System;

namespace _Project.SaveSystem.Interfaces.DataLoading
{
    public interface ILoadedData
    {
        public bool TryGetSubsystemData(Type t, out object data);
        public bool TryGetSubsystemData<T>(out T data);
    }
}