using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;
using ringo.SaveSystem.Subsystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace ringo.SaveModules.Subsystems.Bindable
{
    // TODO: This needs to be renamed to SaveableSubsystem.
    public class SaveableManager : MonoSaveSubsystem, ISaveableManager
    {
        private List<Saveable> _saveables = new();
        private ISaveLoader _saveLoader;

        public override SerializableGuid GUID => _guid;
        // TODO: Make this a serialize field so it can be set in the inspector.
        private SerializableGuid _guid = new("SaveableManager");

        private void Awake()
        {
            GlobalServiceLocator.Instance.Register<ISaveableManager>(this);
        }
        
        private void Start()
        {
            _saveLoader = GlobalServiceLocator.Instance.GetService<ISaveLoader>();
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
            Dictionary<SerializableGuid, Dictionary<string, object>> saveableData = new();
            
            foreach (var saveable in _saveables)
            {
                saveableData[saveable.GUID] = saveable.GetSaveData();
            }
            
            return new SaveableDataContainer(saveableData);
        }

        public override async Task Load(object saveData)
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
    [SaveSystem.Attributes.SaveData("SaveableData")]
    public struct SaveableDataContainer
    {
        public Dictionary<SerializableGuid, Dictionary<string, object>> _saveableData;
        
        public SaveableDataContainer(Dictionary<SerializableGuid, Dictionary<string, object>> saveableData)
        {
            _saveableData = saveableData;
        }
    }
}