using System.Collections.Generic;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.SaveSubsystem;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveManager
    {
        private HashSet<ISaveSubsystem> _saveSubsystems = new();
        private SaveFileService _saveFileService;
        
        public SaveManager(SaveFileService saveFileService)
        {
            _saveFileService = saveFileService;
        }

        public void LoadGame(string fileName)
        {
            HeadSaveData loadedData = _saveFileService.LoadFromFile(fileName);
            
            if (loadedData == null)
            {
                Debug.LogWarning($"Save file {fileName} does not exist.");
            }
            
            return loadedData;
        }

        public void SaveGame(string fileName, bool overrideSave = false)
        {
            // Merge with existing save data if not overriding.
            // TODO: Nesting hell, make this into a separate funciton.
            if (!overrideSave)
            {
                // TODO: Error handling.
                if (SaveFileExists(fileName))
                {
                    HeadSaveData loadedData = LoadGame(fileName);
                    
                    if (loadedData != null)
                    {
                        HeadSaveData.Merge(data, loadedData);
                    }
                }
            }
            
            _saveFileService.SaveToFile(data, fileName);
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
        
        public bool SaveFileExists(string fileName)
        {
            return _saveFileService.SaveFileExists(fileName);
        }
    }
}
