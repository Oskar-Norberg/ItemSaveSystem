using _Project.SaveSystem.DataLoading.Common;
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

        public T LoadGame<T>(string fileName)
        {
            T loadedData = _saveFileService.LoadFromFile<T>(fileName);
            
            if (loadedData == null)
            {
                Debug.LogWarning($"Save file {fileName} does not exist.");
            }
            
            return loadedData;
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
