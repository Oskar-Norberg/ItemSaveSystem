using System;
using System.Collections.Generic;
using _Project.SaveSystem.SaveLoader;
using _Project.SaveSystem.Subsystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveableManager : MonoSaveSubsystem, ISaveableManager
    {
        private List<Saveable> _saveables = new();
        private ISaveLoader _saveLoader;

        private void Awake()
        {
            ServiceLocator.Instance.Register<ISaveableManager>(this);
        }
        
        private void Start()
        {
            _saveLoader = ServiceLocator.Instance.GetService<ISaveLoader>();
            _saveLoader.RegisterSaveSubsystem(this);
        }

        public void BindSaveable(Saveable saveable)
        {
            _saveables.Add(saveable);
        }

        public void UnbindSaveable(Saveable saveable)
        {
            if (!_saveables.Remove(saveable))
            {
                Debug.LogWarning($"Saveable {saveable.GUIDString} not found in saveables list.");
            }
        }
        
        public override object GetSaveData()
        {
            Dictionary<SerializableGuid, Dictionary<string, SaveData>> saveableData = new();
            
            foreach (var saveable in _saveables)
            {
                saveableData[saveable.GUID] = saveable.GetSaveData();
            }
            
            return new SaveableDataContainer(saveableData);
        }

        public override void Load(object saveData)
        {
            if (saveData is not SaveableDataContainer saveableDataContainer)
            {
                Debug.LogError("Invalid save data type. Expected SaveableDataContainer.");
                return;
            }
            
            foreach (var saveable in _saveables)
            {
                if (saveableDataContainer._saveableData.TryGetValue(saveable.GUID, out var data))
                {
                    saveable.Load(data);
                }
                else
                {
                    Debug.LogWarning($"No data found for saveable {saveable.GUIDString} in loaded data.");
                }
            }
        }
    }

    [Serializable]
    [Attributes.SaveData]
    public struct SaveableDataContainer
    {
        public Dictionary<SerializableGuid, Dictionary<string, SaveData>> _saveableData;
        
        public SaveableDataContainer(Dictionary<SerializableGuid, Dictionary<string, SaveData>> saveableData)
        {
            _saveableData = saveableData;
        }
    }
}