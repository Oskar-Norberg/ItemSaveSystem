using System.Collections.Generic;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Interfaces;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveManager
    {
        private SaveFileService _saveFileService;
        
        public SaveManager(SaveFileService saveFileService)
        {
            _saveFileService = saveFileService;
        }

        public HeadSaveData LoadGame(string fileName)
        {
            HeadSaveData loadedData = _saveFileService.LoadFromFile(fileName);
            
            if (loadedData == null)
            {
                Debug.LogWarning($"Save file {fileName} does not exist.");
            }
            
            return loadedData;
        }

        public void SaveGame(string fileName, HeadSaveData data, bool overrideSave = false)
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
        
        public bool SaveFileExists(string fileName)
        {
            return _saveFileService.SaveFileExists(fileName);
        }
    }
}
