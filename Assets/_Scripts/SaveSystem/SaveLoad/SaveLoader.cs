using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            // Only allow one instance of SaveLoader.
            if (GlobalServiceLocator.Instance.TryGetService<ISaveLoader>(out _))
            {
                Destroy(this);
                return;
            }
            
            GlobalServiceLocator.Instance.Register<ISaveLoader>(this);
        }

        private void Start()
        {
            _saveManager = GlobalServiceLocator.Instance.GetService<SaveManager>();
        }

        // TODO: implement save-override/merging logic.
        public void Save(string fileName, bool overrideSave = false)
        {
            HeadSaveData data = new HeadSaveData();
            
            foreach (var subsystem in _saveSubsystems)
            {
                data.AddSubContainer(subsystem.GUID, subsystem.GetSaveData());
            }

            _saveManager.SaveGame(fileName, data);
        }
        
        public async Task Load(string fileName)
        {
            HeadSaveData data = _saveManager.LoadGame<HeadSaveData>(fileName);

            // Load all stages in order.
            foreach (LoadStage stage in Enum.GetValues(typeof(LoadStage)))
            {
                // Get the subsystems each stage.
                // This ensures if a previous stage altered the coming stage it will be reflected.
                foreach (var subsystem in GetSubsystemsByStage(stage))
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
        
        private IEnumerable<ISaveSubsystem> GetSubsystemsByStage(LoadStage stage)
        {
            List<ISaveSubsystem> subsystems = new List<ISaveSubsystem>(_saveSubsystems.Count);
            foreach (var subsystem in _saveSubsystems)
            {
                if (subsystem.SystemLoadStage == stage)
                {
                    subsystems.Add(subsystem);
                }
            }

            return subsystems;
            
            // foreach (var subsystem in _saveSubsystems)
            // {
            //     if (subsystem.SystemLoadStage == stage)
            //     {
            //         yield return subsystem;
            //     }
            // }
        }
    }
}
