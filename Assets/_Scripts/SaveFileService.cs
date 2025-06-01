using System.IO;
using _Project.SaveSystem.Exceptions;
using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem
{
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