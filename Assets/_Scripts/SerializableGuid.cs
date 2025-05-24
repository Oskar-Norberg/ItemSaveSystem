using System;

namespace _Project.SaveSystem
{
    [Serializable]
    public class SerializableGuid
    {
        // TODO: Why is this a string? Why not a Guid?
        // TODO: Readonly?
        public string GuidString;
        
        public SerializableGuid()
        {
            GuidString = Guid.NewGuid().ToString();
        }
        
        public SerializableGuid(string guidString)
        {
            // TODO: Validate the string format.
            if (string.IsNullOrEmpty(guidString))
            {
                throw new ArgumentException("Guid string cannot be null or empty.", nameof(guidString));
            }
            
            GuidString = guidString;
        }

        public static implicit operator bool(SerializableGuid serializableGuid)
        {
            if (serializableGuid == null)
            {
                return false;
            }
            
            return !string.IsNullOrEmpty(serializableGuid.GuidString);
        }

        // TODO: Expensive string comparison.
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