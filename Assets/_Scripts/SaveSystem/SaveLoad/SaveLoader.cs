using System.Collections.Generic;
using ringo.SaveSystem.DataLoading.Common;
using ringo.SaveSystem.Subsystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace ringo.SaveSystem.Managers
{
    public class SaveLoader : MonoBehaviour, ISaveLoader
    {
        private readonly HashSet<ISaveSubsystem> _saveSubsystems = new();
        
        private SaveManager _saveManager;

        private void Awake()
        {
            GlobalServiceLocator.Instance.Register<ISaveLoader>(this);
        }

        private void Start()
        {
            _saveManager = GlobalServiceLocator.Instance.GetService<SaveManager>();
        }

        // TODO: implement override logic.
        public void Save(string fileName, bool overrideSave = false)
        {
            IEnumerable<ISaveSubsystem> sortedSubsystems = SortSubsystemsByPriority();
            
            HeadSaveData data = new HeadSaveData();
            
            foreach (var subsystem in sortedSubsystems)
            {
                data.AddSubContainer(subsystem.GUID, subsystem.GetSaveData());
            }

            _saveManager.SaveGame(fileName, data);
        }
        
        public void Load(string fileName)
        {
            IEnumerable<ISaveSubsystem> sortedSubsystems = SortSubsystemsByPriority();
            
            HeadSaveData data = _saveManager.LoadGame<HeadSaveData>(fileName);

            foreach (var subsystem in sortedSubsystems)
            {
                var subsystemData = data.TryGetSubsystemData(subsystem.GUID, out var subsystemSaveData);
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

        private IEnumerable<ISaveSubsystem> SortSubsystemsByPriority()
        {
            List<ISaveSubsystem> sortedSubsystems = new List<ISaveSubsystem>(_saveSubsystems);
            sortedSubsystems.Sort((a, b) => a.ExecutionPriority.CompareTo(b.ExecutionPriority));
            return sortedSubsystems;
        }
    }
}
