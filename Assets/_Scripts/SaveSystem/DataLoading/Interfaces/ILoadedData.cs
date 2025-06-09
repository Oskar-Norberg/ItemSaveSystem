using System;

namespace _Project.SaveSystem.Interfaces.DataLoading
{
    public interface ILoadedData
    {
        public bool TryGetSubsystemData(SerializableGuid guid, out object data);
    }
}