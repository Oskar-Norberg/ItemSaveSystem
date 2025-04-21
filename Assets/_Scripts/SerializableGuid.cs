using System;

namespace _Project.SaveSystem
{
    [Serializable]
    public class SerializableGuid
    {
        // TODO: Why is this a string? Why not a Guid?
        public string GuidString;
        
        public SerializableGuid()
        {
            GuidString = Guid.NewGuid().ToString();
        }

        public static implicit operator bool(SerializableGuid serializableGuid)
        {
            if (serializableGuid == null)
            {
                return false;
            }
            
            return !string.IsNullOrEmpty(serializableGuid.GuidString);
        }

        public override bool Equals(object obj)
        {
            if (obj is SerializableGuid serializableGuid)
            {
                return GuidString == serializableGuid.GuidString;
            }

            return false;
        }
        
        public override int GetHashCode()
        {
            return GuidString.GetHashCode();
        }
    }
}