using System;
using System.Collections.Generic;
using _Project.SaveSystem.Events;
using _Project.SaveSystem.Interfaces;
using ringo.EventSystem;
using ringo.ServiceLocator;
using UnityEditor;
using UnityEngine;

namespace _Project.SaveSystem
{
    [Serializable]
    public class Saveable : MonoBehaviour
    {
        // TODO: Consider moving this to another class and adding a RequireComponent attribute.
        public string GUID => guid.GuidString;
        public SaveableType SaveableType => saveableType;
        
        [SerializeField, HideInInspector] private SerializableGuid guid;
        [SerializeField] private SaveableType saveableType;
        
        private ArgumentEventHandler<LoadGameResponse> _loadGameResponseHandler;
        
        // String is type name.
        private Dictionary<string, SaveData> _saveData = new();
        
        private void Awake()
        {
            if (!guid)
                guid = new SerializableGuid();
            
            // TODO: Unbind on destroy.
            ServiceLocator.Instance.GetService<SaveManager>().BindSaveable(this);
            _loadGameResponseHandler = new ArgumentEventHandler<LoadGameResponse>(LoadGame);
        }
        
        private void OnEnable()
        {
            _loadGameResponseHandler.Activate();
        }

        private void OnDisable()
        {
            _loadGameResponseHandler.Deactivate();
        }
        
        private void LoadGame(LoadGameResponse loadGameResponse)
        {
            
        }
        
        public Dictionary<string, SaveData> GetSaveData()
        {
            return _saveData;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!guid || IsDuplicate())
            {
                guid = new SerializableGuid();
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
    }
}