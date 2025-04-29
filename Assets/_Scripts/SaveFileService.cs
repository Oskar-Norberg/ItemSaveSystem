using System.IO;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Interfaces;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveFileService
    {
        private const string SaveFileName = "creatively_named_save_file";
        
        private ISerializer _serializer;
        
        public SaveFileService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        // TODO: Implement save slots
        // TODO: Don't completely overwrite the file. Append to it instead.
        // TODO: Write documentation.
        public void SaveToFile(uint slot, HeadSaveData saveData, bool overrideSave)
        {
            // Concatenate the new data with the previous data
            if (!overrideSave)
            {
                HeadSaveData previousData = LoadFromFile(slot);
                
                // TODO: Add error-checking for if the file doesn't exist or is invalid. If this happens, copy the previous data to a backup-file with date as name.
                if (previousData != null)
                {
                    saveData += previousData;
                }
            }

            string serializedOutput = _serializer.Serialize(saveData);
            File.WriteAllText(GetPathString(), serializedOutput);
        }

        public HeadSaveData LoadFromFile(uint slot)
        {
            string serializedData = File.ReadAllText(GetPathString());

            return _serializer.Deserialize<HeadSaveData>(serializedData);
        }
        
        private static string GetPathString()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }
    }
}