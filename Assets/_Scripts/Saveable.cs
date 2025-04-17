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
        
        [SerializeField, HideInInspector] private SerializableGuid guid;
        
        private List<SaveData> _saveData = new();
        
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

        public List<SaveData> GetSaveData()
        {
            return _saveData;
        }
        
        public void BindSaveData(SaveData saveData)
        {
            if (saveData == null) 
                return;
            
            _saveData.Add(saveData);
        }
    }
}