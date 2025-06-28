using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ringo.SaveSystem.DataLoading.Common;
using ringo.SaveSystem.DataLoading.Common.Merging;
using ringo.SaveSystem.DataLoading.Serialization;
using ringo.SaveSystem.DataLoading.Serialization.Binary;
using ringo.SaveSystem.DataLoading.Serialization.JSON;
using ringo.SaveSystem.Services;
using ringo.SaveSystem.Subsystem;
using UnityEngine;

namespace ringo.SaveSystem.Managers
{
    public class SaveLoader : MonoBehaviour, ISaveLoader
    {
        [SerializeField] private SerializationTypes serializationType = SerializationTypes.JSON;
        
        private readonly HashSet<ISaveSubsystem> _saveSubsystems = new();
        
        private SaveManager _saveManager;

        protected void Awake()
        {
            ISerializer serializer = null;
            
            switch (serializationType)
            {
                case SerializationTypes.Binary:
                    serializer = new BinarySerializer();
                    break;
                // Fallback to JSON if no valid type.
                case SerializationTypes.JSON:
                    serializer = new JSONSerializer();
                    break;
                default:
                    Debug.LogError("Unknown serialization type: " + serializationType);
                    // Tremendously cursed
                    goto case SerializationTypes.JSON;
            }
            
            SaveFileService saveFileService = new SaveFileService(serializer);
            _saveManager = new SaveManager(saveFileService);
        }

        // TODO: implement save-override/merging logic.
        public void Save(string fileName, bool overrideSave = false)
        {
            HeadSaveData newSaveData = GetNewSaveData();

            // Do not override save / merge data. Consider moving to a separate method. Maybe even a MergeOrchestrator class.
            if (!overrideSave)
            {
                HeadSaveData existingData = _saveManager.LoadGame<HeadSaveData>(fileName);
                
                if (existingData != null)
                {
                    foreach (var subData in newSaveData._saveDatas)
                    {
                        var data = existingData.TryGetSubsystemData(subData.Key, out var existingSubData);
                        if (data)
                        {
                            // If data exists, merge it.
                            DataMerger.TryMergeData(subData.Value, existingSubData);
                        }
                    }
                }
            }

            _saveManager.SaveGame(fileName, newSaveData);
        }
        
        public async Task Load(string fileName)
        {
            HeadSaveData data = _saveManager.LoadGame<HeadSaveData>(fileName);

            // Load all stages in order.
            foreach (LoadStage stage in Enum.GetValues(typeof(LoadStage)))
            {
                // Get the subsystems each stage.
                // This ensures if a previous stage altered the coming stage it will be reflected.
                foreach (var subsystem in GetSubsystemsByStage(stage).ToList())
                {
                    var subsystemData = data.TryGetSubsystemData(subsystem.GUID, out var subsystemSaveData);
                
                    if (subsystemData)
                    {
                        await subsystem.Load(subsystemSaveData);
                    }
                    else
                    {
                        Debug.Log($"No data found for subsystem {subsystem.GetType().Name} in loaded data.");
                    }
                }
            }
        }
        
        public void RegisterSaveSubsystem(ISaveSubsystem saveSubsystem)
        {
            _saveSubsystems.Add(saveSubsystem);
        }
        
        public void UnregisterSaveSubsystem(ISaveSubsystem saveSubsystem)
        {
            if (!_saveSubsystems.Remove(saveSubsystem))
            {
                Debug.LogWarning($"Save subsystem {saveSubsystem.GetType().Name} not found in registered subsystems.");
            }
        }

        private HeadSaveData GetNewSaveData()
        {
            var data = new HeadSaveData();
            
            foreach (var subsystem in _saveSubsystems)
            {
                data.AddSubContainer(subsystem.GUID, subsystem.GetSaveData());
            }

            return data;
        }
        
        private IEnumerable<ISaveSubsystem> GetSubsystemsByStage(LoadStage stage)
        {
            foreach (var subsystem in _saveSubsystems)
            {
                if (subsystem.SystemLoadStage == stage)
                {
                    yield return subsystem;
                }
            }
        }
    }
}
