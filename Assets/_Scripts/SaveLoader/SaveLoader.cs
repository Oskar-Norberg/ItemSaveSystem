using System.Collections.Generic;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Subsystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem.SaveLoader
{
    public class SaveLoader : MonoBehaviour, ISaveLoader
    {
        private readonly HashSet<ISaveSubsystem> _saveSubsystems = new();
        
        private SaveManager _saveManager;

        private void Awake()
        {
            ServiceLocator.Instance.Register<ISaveLoader>(this);
        }

        private void Start()
        {
            _saveManager = ServiceLocator.Instance.GetService<SaveManager>();
        }

        public void Save(string fileName, bool overrideSave = false)
        {
            // TODO: implement override logic.
            
            HeadSaveData data = new HeadSaveData();
            
            foreach (var subsystem in _saveSubsystems)
            {
                data.AddSubContainer(subsystem.GetType(), subsystem.GetSaveData());
            }

            _saveManager.SaveGame(fileName, data);
        }
        
        public void Load(string fileName)
        {
            HeadSaveData data = _saveManager.LoadGame<HeadSaveData>(fileName);

            foreach (var subsystem in _saveSubsystems)
            {
                var subsystemData = data.TryGetSubsystemData(subsystem.GetType(), out var subsystemSaveData);
                if (subsystemData)
                {
                    subsystem.Load(subsystemSaveData);
                }
                else
                {
                    Debug.Log($"No data found for subsystem {subsystem.GetType().Name} in loaded data.");
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
    }
}
