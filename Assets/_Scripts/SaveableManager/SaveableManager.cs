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

        // TODO: maybe this calls for a new ISaveable interface that has a GetSaveData method?
        public HeadSaveData GetSaveData()
        {
            return GetSaveablesData();
        }

        public void Load(HeadSaveData saveData)
        {
            LoadSaveables(saveData);
        }

        private HeadSaveData GetSaveablesData()
        {
            HeadSaveData headSaveData = new HeadSaveData();
            
            foreach (var saveable in _saveables)
            {
                SubSaveData subSaveData = new SubSaveData(
                    saveable.GetSaveData()
                );
                
                headSaveData.AddSubContainer(saveable.GUID, subSaveData);
            }

            return headSaveData;
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