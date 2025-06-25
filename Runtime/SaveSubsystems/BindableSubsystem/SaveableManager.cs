using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;
using ringo.SaveSystem.Subsystem;
using UnityEngine;

namespace ringo.SaveModules.Subsystems.Bindable
{
    /// <summary>
    /// Manager class for Saveable components. Communicates with the SaveLoader to save and load data for all registered Saveables.
    /// Will not automatically bind to Saveables; user must call BindSaveable to this manager.
    /// See also SingletonSaveableManager for a singleton version of this.
    /// </summary>
    // TODO: This needs to be renamed to SaveableSubsystem.
    public class SaveableManager : MonoSaveSubsystem, ISaveableManager
    {
        private List<Saveable> _saveables = new();
        private ISaveLoader _saveLoader;

        public override SerializableGuid GUID => _guid;
        // TODO: Make this a serialize field so it can be set in the inspector.
        private SerializableGuid _guid = new("SaveableManager");

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

        public override Task Load(object saveData)
        {
            if (saveData is not SaveableDataContainer saveableDataContainer)
            {
                Debug.LogError("Invalid save data type. Expected SaveableDataContainer.");
                return Task.CompletedTask;
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

            return Task.CompletedTask;
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