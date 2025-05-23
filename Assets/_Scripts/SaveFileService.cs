using System.IO;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Exceptions;
using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem
{
    // TODO: Add file extension to file name.
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
                    HeadSaveData previousData = LoadFromFile(GetPathString(fileName));

                    // TODO: Add error-checking for if the file doesn't exist or is invalid. If this happens, copy the previous data to a backup-file with date as name.
                    if (previousData != null)
                    {
                        // TODO: Should this be a .Merge() function instead? Arithmetic operators on complex types are unintuitive.
                        saveData += previousData;
                    }
                }
                catch (SaveNotFoundException)
                {
                    Debug.Log("Save file not found, creating new one.");
                }
                catch (InvalidSaveException)
                {
                    // Old Save was invalid, so backup old save and create a new one in its place.
                    Debug.LogError("Invalid save. Backing up old one and overriding.");

                    string backupFileName = $"{fileName}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.backup";
                    File.Copy(GetPathString(fileName), GetPathString(backupFileName));
                }
            }
            
            string serializedOutput = _serializer.Serialize(saveData);
            File.WriteAllText(GetPathString(fileName), serializedOutput);
        }

        // TODO: Consider if this should be a TryLoad function rather than throwing an exception.
        public HeadSaveData LoadFromFile(string fileName)
        {
            if (!File.Exists(GetPathString(fileName)))
            {
                throw new SaveNotFoundException($"Save file {fileName} does not exist.");
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