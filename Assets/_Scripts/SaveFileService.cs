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
        
        public SaveFileService()
        {
            _serializer = ServiceLocator.Instance.GetService<ISerializer>();
        }

        // TODO: Implement save slots
        // TODO: Don't completely overwrite the file. Append to it instead.
        public void SaveToFile(uint slot, HeadSaveData saveData)
        {
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