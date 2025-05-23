using System.Collections.Generic;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Interfaces;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    // TODO: Consider decoupling this class from the EventBus and ServiceLocator entirely. Having this as either a singleton or a static class might be better.
    public class SaveManager : MonoBehaviour
    {
        private List<Saveable> _saveables = new();

        private SaveFileService _saveFileService;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
        }

        private void Start()
        {
            // TODO: Consider moving to service locator.
            _saveFileService = new SaveFileService(ServiceLocator.Instance.GetService<ISerializer>());
        }

        public void BindSaveable(Saveable saveable)
        {
            _saveables.Add(saveable);
        }

        public void UnbindSaveable(Saveable saveable)
        {
            if (!_saveables.Remove(saveable))
            {
                Debug.LogWarning($"Saveable {saveable.GUIDString} not found in saveables list.");
            }
        }

        public void LoadGame(string fileName)
        {
            HeadSaveData loadedData = _saveFileService.LoadFromFile(fileName);
            
            if (loadedData == null)
            {
                Debug.LogWarning($"Save file {fileName} does not exist.");
                return;
            }

            foreach (var saveable in _saveables)
            {
                if (loadedData.TryGetDataByGUID(saveable.GUID, out var data))
                {
                    saveable.Load(data);
                }
                else
                {
                    Debug.Log($"Saveable {saveable.GUIDString} not found in loaded data.");
                }
            }
        }

        public void SaveGame(string fileName, bool overrideSave = false)
        {
            HeadSaveData headSaveData = new HeadSaveData();
            
            foreach (var saveable in _saveables)
            {
                SubSaveData subSaveData = new SubSaveData(
                    saveable.GUID,
                    saveable.GetSaveData()
                    );
                
                headSaveData.AddSubContainer(subSaveData);
            }
            
            _saveFileService.SaveToFile( headSaveData, fileName, overrideSave);
        }
        
        public bool SaveFileExists(string fileName)
        {
            return _saveFileService.SaveFileExists(fileName);
        }
    }
}
