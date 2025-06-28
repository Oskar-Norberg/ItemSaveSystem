using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ringo.SaveSystem.DataLoading.Common.Merging;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;
using ringo.SaveSystem.Subsystem;
using UnityEngine;

namespace ringo.SaveModules.Subsystems.Bindable
{
    /// <summary>
    /// Subsystem class for Saveable components. Communicates with the SaveLoader to save and load data for all registered Saveables.
    /// Will not automatically bind to Saveables; user must call BindSaveable to this manager.
    /// See also SingletonSaveableSubsystem for a singleton version of this.
    /// </summary>
    // TODO: This needs to be renamed to SaveableSubsystem.
    public class SaveableSubsystem : MonoSaveSubsystem<SaveableDataContainer>, ISaveableSubsystem
    {
        private List<Saveable> _saveables = new();
        private ISaveLoader _saveLoader;

        public override SerializableGuid GUID => _guid;
        // TODO: Make this a serialize field so it can be set in the inspector.
        private SerializableGuid _guid = new("SaveableSubsystem");

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

        protected override SaveableDataContainer GetSaveDataTyped()
        {
            Dictionary<SerializableGuid, Dictionary<string, object>> saveableData = new();
            
            foreach (var saveable in _saveables)
            {
                saveableData[saveable.GUID] = saveable.GetSaveData();
            }
            
            return new SaveableDataContainer(saveableData);
        }

        protected override Task LoadTyped(SaveableDataContainer saveData)
        {
            foreach (var saveable in _saveables)
            {
                if (saveData._saveableData.TryGetValue(saveable.GUID, out var data))
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
    public struct SaveableDataContainer : IMergeable
    {
        public Dictionary<SerializableGuid, Dictionary<string, object>> _saveableData;
        
        public SaveableDataContainer(Dictionary<SerializableGuid, Dictionary<string, object>> saveableData)
        {
            _saveableData = saveableData;
        }

        public void Merge(object data)
        {
            if (data is not SaveableDataContainer other)
            {
                throw new ArgumentException("Cannot merge with non-SaveableDataContainer type.");
            }

            // Merge previous saveable data ignoring duplicates.
            foreach (var kvp in other._saveableData)
            {
                if (_saveableData.ContainsKey(kvp.Key))
                    continue;
                
                _saveableData[kvp.Key] = kvp.Value;
            }
        }
    }
}