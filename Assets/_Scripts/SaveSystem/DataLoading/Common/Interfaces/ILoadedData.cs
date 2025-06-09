using ringo.SaveSystem.GUID;

namespace ringo.SaveSystem.DataLoading.Common
{
    public interface ILoadedData
    {
        public bool TryGetSubsystemData(SerializableGuid guid, out object data);
    }
}