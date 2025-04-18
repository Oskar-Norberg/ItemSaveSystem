using System;
using System.Collections.Generic;
using ringo.ServiceLocator;
using UnityEditor;
using UnityEngine;

namespace _Project.SaveSystem
{
    [Serializable]
    public class Saveable : MonoBehaviour
    {
        // TODO: I think if this is created at runtime it will not get a GUID. So as a fallback a GUID should be created in the awake function too.
        public string GUID => guid.GuidString;
        public SaveableType SaveableType => saveableType;
        
        [SerializeField, HideInInspector] private SerializableGuid guid;
        [SerializeField] private SaveableType saveableType;
        
        // String is type name.
        private Dictionary<string, SaveData> _saveData = new();
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!guid || IsDuplicate())
            {
                guid = new SerializableGuid(Guid.NewGuid());
                EditorUtility.SetDirty(this);
            }
        }

        private bool IsDuplicate()
        {
            var saveableObjects = FindObjectsByType(typeof(Saveable), FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var saveableObject in saveableObjects)
            {
                var saveable = saveableObject as Saveable;
                
                if (saveable == null)
                    Debug.LogError(saveableObject + " is not a Saveable");
                
                if (saveable == this)
                    continue;
                
                if (saveable.guid == guid)
                {
                    Debug.Log("Duplicate GUID found, fixing issue.");
                    return true;
                }
            }

            return false;
        }
        #endif

        private void Awake()
        {
            ServiceLocator.Instance.GetService<SaveManager>().BindSaveable(this);
        }

        public Dictionary<string, SaveData> GetSaveData()
        {
            return _saveData;
        }
        
        public void BindSaveData(SaveData saveData)
        {
            if (saveData == null) 
                return;
            
            _saveData[saveData.GetType().Name] = saveData;
        }
    }
}