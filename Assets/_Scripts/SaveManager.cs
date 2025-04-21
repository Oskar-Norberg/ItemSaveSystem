using System.Collections.Generic;
using _Project.SaveSystem.DataLoading.Common;
using _Project.SaveSystem.Events;
using _Project.SaveSystem.Interfaces;
using ringo.EventSystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{   
    public class SaveManager : MonoBehaviour
    {
        private NoArgumentEventHandler<SaveGameRequest> _saveGameRequestHandler;
        private NoArgumentEventHandler<LoadGameRequest> _loadGameRequestHandler;
        
        private List<Saveable> _saveables = new();

        private SaveFileService _saveFileService;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
            _saveGameRequestHandler = new NoArgumentEventHandler<SaveGameRequest>(SaveGame);
            _loadGameRequestHandler = new NoArgumentEventHandler<LoadGameRequest>(LoadGame);
        }

        private void Start()
        {
            // TODO: Consider moving to service locator.
            _saveFileService = new SaveFileService(ServiceLocator.Instance.GetService<ISerializer>());
        }

        private void OnEnable()
        {
            _saveGameRequestHandler.Activate();
            _loadGameRequestHandler.Activate();
        }

        private void OnDisable()
        {
            _saveGameRequestHandler.Deactivate();
            _loadGameRequestHandler.Deactivate();
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

        private void LoadGame()
        {
            HeadSaveData loadedData = _saveFileService.LoadFromFile(0);
            
            if (loadedData == null)
            {
                Debug.LogWarning("No savefile found.");
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
                    Debug.LogWarning($"Saveable {saveable.GUIDString} not found in loaded data.");
                }
            }
            
            EventBus.Publish(new LoadGameResponse());
        }

        private void SaveGame()
        {
            HeadSaveData headSaveData = new HeadSaveData();
            
            foreach (var saveable in _saveables)
            {
                SubSaveData subSaveData = new SubSaveData(
                    saveable.GUID, 
                    saveable.SaveableType, 
                    saveable.GetSaveData()
                    );
                
                headSaveData.AddSubContainer(subSaveData);
            }
            
            _saveFileService.SaveToFile(0, headSaveData);
            
            EventBus.Publish(new SaveGameResponse());
        }
        
        
    }
}
