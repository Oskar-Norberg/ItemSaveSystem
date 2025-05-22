using System.IO;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveFileService
    {
        private ISerializer _serializer;
        
        public SaveFileService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        // TODO: Implement save slots
        // TODO: Don't completely overwrite the file. Append to it instead.
        // TODO: Write documentation.
        public void SaveToFile(HeadSaveData saveData, string fileName, bool overrideSave)
        {
            // Concatenate the new data with the previous data
            if (!overrideSave)
            {
                HeadSaveData previousData = LoadFromFile(fileName);
                
                // TODO: Add error-checking for if the file doesn't exist or is invalid. If this happens, copy the previous data to a backup-file with date as name.
                if (previousData != null)
                {
                    saveData += previousData;
                }
            }

            string serializedOutput = _serializer.Serialize(saveData);
            File.WriteAllText(GetPathString(fileName), serializedOutput);
        }

        public HeadSaveData LoadFromFile(string fileName)
        {
            string serializedData = File.ReadAllText(GetPathString(fileName));

            return _serializer.Deserialize<HeadSaveData>(serializedData);
        }
        
        private string GetPathString(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}