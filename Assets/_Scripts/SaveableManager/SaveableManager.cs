using System.Collections.Generic;
using _Project.SaveSystem.DataLoading.Common;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveableManager : MonoBehaviour, ISaveableManager
    {
        private List<Saveable> _saveables = new();
        
        private SaveManager _saveManager;

        private void Awake()
        {
            ServiceLocator.Instance.Register<ISaveableManager>(this);
        }

        private void Start()
        {
            _saveManager = ServiceLocator.Instance.GetService<SaveManager>();
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

        public void Save(string fileName, bool doOverride = false)
        {
            HeadSaveData headSaveData = new HeadSaveData();
            
            foreach (var saveable in _saveables)
            {
                SubSaveData subSaveData = new SubSaveData(
                    saveable.GetSaveData()
                );
                
                headSaveData.AddSubContainer(saveable.GUID, subSaveData);
            }
            
            _saveManager.SaveGame(fileName, headSaveData, doOverride);
        }

        public void Load(string fileName)
        {
            SaveManager saveManager = ServiceLocator.Instance.GetService<SaveManager>();
            HeadSaveData loadedData = saveManager.LoadGame(fileName);
            
            LoadSaveables(loadedData);
        }

        public void Load(HeadSaveData saveData)
        {
            LoadSaveables(saveData);
        }

        private void LoadSaveables(HeadSaveData saveData)
        {
            foreach (var saveable in _saveables)
            {
                if (saveData.TryGetDataByGUID(saveable.GUID, out var data))
                {
                    saveable.Load(data);
                }
                else
                {
                    Debug.Log($"Saveable {saveable.GUIDString} not found in loaded data.");
                }
            }
        }
    }
}