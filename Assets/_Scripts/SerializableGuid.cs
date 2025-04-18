using System;

namespace _Project.SaveSystem
{
    [Serializable]
    public class SerializableGuid
    {
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
    }
}