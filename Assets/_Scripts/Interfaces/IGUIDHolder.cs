namespace _Project.SaveSystem.Interfaces
{
    public interface IGUIDHolder
    {
        public string GUIDString { get; }
        public SerializableGuid GUID { get; }
    }
}