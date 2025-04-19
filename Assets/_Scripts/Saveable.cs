using System;
using System.Collections.Generic;
using System.Linq;
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
        
        // String is name of bond.
        private Dictionary<string, IBindable> _bonds = new();
        
        private void Awake()
        {
            if (!guid)
                guid = new SerializableGuid();
            
            // TODO: Unbind on destroy.
            ServiceLocator.Instance.GetService<SaveManager>().BindSaveable(this);
        }

        public void Bind(string bindName, IBindable bindable)
        {
            _bonds[bindName] = bindable;
        }
        
        public void Load(Dictionary<string, SaveData> loadedData)
        {
            foreach (var kvp in loadedData)
            {
                if (_bonds.TryGetValue(kvp.Key, out var bindable))
                {
                    bindable.LoadSaveData(kvp.Value);
                }
                else
                {
                    Debug.LogWarning($"No bond found for {kvp.Key} in {gameObject.name}");
                }
            }
        }
        
        public Dictionary<string, SaveData> GetSaveData()
        {
            // TODO: Consider making this a list of SaveData instead of a dictionary.
            // TODO: This will generate a bit of garbage.
            return _bonds.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetSaveData());
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