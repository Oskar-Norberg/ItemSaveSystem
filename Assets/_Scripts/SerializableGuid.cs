using System;

namespace _Project.SaveSystem
{
    [Serializable]
    public struct SerializableGuid
    {
        public string GuidString;
        
        public SerializableGuid(Guid guid)
        {
            GuidString = guid.ToString();
        }
        
        public SerializableGuid(string guidString)
        {
            GuidString = guidString;
        }

        public static implicit operator bool(SerializableGuid serializableGuid)
        {
            return !string.IsNullOrEmpty(serializableGuid.GuidString);
        }
    }
}