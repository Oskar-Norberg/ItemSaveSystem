using System;
using System.Collections.Generic;
using _Project.SaveSystem.Interfaces;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    [Serializable]
    public class Saveable : MonoBehaviour
    {
        public string GUID => guid.GuidString;
        public SaveableType SaveableType => saveableType;
        
        [SerializeField, HideInInspector] private SerializableGuid guid;
        [SerializeField] private SaveableType saveableType;
        
        // String is type name.
        private Dictionary<string, SaveData> _saveData = new();
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!guid)
                guid = new SerializableGuid(Guid.NewGuid());
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