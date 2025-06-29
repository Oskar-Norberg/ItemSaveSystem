using System;
using System.IO;
using ringo.SaveSystem.Exceptions;
using ringo.SaveSystem.Services;
using UnityEngine;

namespace ringo.SaveSystem.Managers
{
    public class SaveManager
    {
        private SaveFileService _saveFileService;
        
        public SaveManager(SaveFileService saveFileService)
        {
            _saveFileService = saveFileService;
        }

        // TODO: This desperately needs to be a TryLoad function.
        public T LoadGame<T>(string fileName)
        {
            try
            {
                T loadedData = _saveFileService.LoadFromFile<T>(fileName);

                return loadedData;
            }
            catch (Exception ex) when (ex is InvalidDataException or InvalidSaveException )
            {
                Debug.LogException(ex);
                // This is annoying as hell, I can't mute this.
                System.Diagnostics.Debugger.Break();
            }
            
            return default;
        }

        public void SaveGame<T>(string fileName, T saveData)
        {
            _saveFileService.SaveToFile(saveData, fileName);
        }
        
        public bool SaveFileExists(string fileName)
        {
            return _saveFileService.SaveFileExists(fileName);
        }
    }
}
