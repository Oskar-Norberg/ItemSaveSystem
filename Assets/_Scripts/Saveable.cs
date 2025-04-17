using System;
using System.Collections.Generic;
using _Project.SaveSystem.Interfaces;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class Saveable : MonoBehaviour
    {
        [SerializeField, HideInInspector] private SerializableGuid guid;
        
        private List<ISaveData> _saveData = new();
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!guid)
                guid = new SerializableGuid(Guid.NewGuid());
        }
        #endif

        private void Start()
        {
            ServiceLocator.Instance.GetService<SaveManager>().BindSaveable(this);
        }

        public List<ISaveData> GetSaveData()
        {
            return _saveData;
        }
        
        public void BindSaveData(ISaveData saveData)
        {
            if (saveData == null) 
                return;
            
            _saveData.Add(saveData);
        }
    }
}