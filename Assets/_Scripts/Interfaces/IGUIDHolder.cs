namespace _Project.SaveSystem.Interfaces
{
    // TODO: Rename to IGUIDProvider.
    public interface IGUIDHolder
    {
        public string GUIDString { get; }
        public SerializableGuid GUID { get; }
    }
}