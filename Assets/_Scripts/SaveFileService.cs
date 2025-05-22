using System.IO;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Exceptions;
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

        // TODO: Write documentation.
        public void SaveToFile(HeadSaveData saveData, string fileName, bool overrideSave)
        {
            // Concatenate the new data with the previous data
            if (!overrideSave)
            {
                try
                {
                    HeadSaveData previousData = LoadFromFile(fileName);

                    // TODO: Add error-checking for if the file doesn't exist or is invalid. If this happens, copy the previous data to a backup-file with date as name.
                    if (previousData != null)
                    {
                        saveData += previousData;
                    }
                }
                catch (SaveNotFoundException)
                {
                    Debug.Log("Save file not found, creating new one.");
                }
                catch (InvalidSaveException)
                {
                    Debug.LogError("Save file not found, creating new one.");
                    // TODO: back up the file with a date as name.
                }
            }

            string serializedOutput = _serializer.Serialize(saveData);
            File.WriteAllText(GetPathString(fileName), serializedOutput);
        }

        public HeadSaveData LoadFromFile(string fileName)
        {
            if (!File.Exists(GetPathString(fileName)))
            {
                throw new FileNotFoundException($"Save file {fileName} does not exist.");
            }
            
            string serializedData = File.ReadAllText(GetPathString(fileName));
            
            try 
            {
                HeadSaveData saveData = _serializer.Deserialize<HeadSaveData>(serializedData);

                return saveData;
            }
            catch (InvalidDataException)
            {
                throw new InvalidSaveException($"Save file {fileName} is invalid.");
            }
        }
        
        private string GetPathString(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}