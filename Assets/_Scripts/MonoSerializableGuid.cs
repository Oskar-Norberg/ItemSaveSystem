using UnityEditor;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class MonoSerializableGuid : MonoBehaviour
    {
        public string GUIDString => guid.GuidString;
        public SerializableGuid GUID => guid;

        [SerializeField, HideInInspector] private SerializableGuid guid;
        
        private void Awake()
        {
            if (!guid)
                Reset();
        }

        public void Reset()
        {
            guid = new SerializableGuid();
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!guid || IsDuplicate())
            {
                Debug.Log("Fixing duplicate GUID.");
                guid = new SerializableGuid();
                EditorUtility.SetDirty(this);
            }
        }

        private bool IsDuplicate()
        {
            var monoGuidObjects = FindObjectsByType(typeof(MonoSerializableGuid), FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var monoGuidObject in monoGuidObjects)
            {
                var monoGuid = monoGuidObject as MonoSerializableGuid;
                
                if (!monoGuid)
                    continue;
                
                if (monoGuid == this)
                    continue;

                if (!Equals(monoGuid.guid, guid)) 
                    continue;
                
                Debug.Log("Duplicate GUID found.");
                return true;
            }

            return false;
        }
#endif
    }
}