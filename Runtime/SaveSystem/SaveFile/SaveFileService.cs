using System;
using System.IO;
using ringo.SaveSystem.DataLoading.Serialization;
using ringo.SaveSystem.Exceptions;
using UnityEngine;

namespace ringo.SaveSystem.Services
{
    // TODO: Make ISaveFileService interface.
    public class SaveFileService
    {
        private readonly ISerializer _serializer;
        
        public SaveFileService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        // TODO: Write documentation.
        public void SaveToFile<T>(T saveData, string fileName)
        {
            string serializedOutput = _serializer.Serialize(saveData);
            File.WriteAllText(GetPathString(fileName), serializedOutput);
        }

        // TODO: Consider if this should be a TryLoad function rather than throwing an exception.
        public T LoadFromFile<T>(string fileName)
        {
            if (!File.Exists(GetPathString(fileName)))
            {
                throw new SaveNotFoundException($"Save file {fileName} does not exist.");
            }
            
            string serializedData = File.ReadAllText(GetPathString(fileName));

            try
            {
                T saveData = _serializer.Deserialize<T>(serializedData);

                return saveData;
            }
            catch (InvalidDataException)
            {
                throw new InvalidSaveException($"Save file {fileName} is invalid.");
            }
            catch (Exception e)
            {
                throw new InvalidDataException("An error occurred while loading the save file.", e);
            }
        }
        
        public bool SaveFileExists(string fileName)
        {
            return File.Exists(GetPathString(fileName));
        }
        
        private string GetPathString(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}