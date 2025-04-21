using System.Collections.Generic;
using _Project.SaveSystem.Events;
using _Project.SaveSystem.Interfaces;
using _Project.SaveSystem.Interfaces.DataLoading;
using ringo.EventSystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{   
    public class SaveManager : MonoBehaviour
    {
        public ILoadedData LoadedData { get; private set; }
        
        private const string SaveFileName = "creatively_named_save_file";
        
        private NoArgumentEventHandler<SaveGameRequest> _saveGameRequestHandler;
        private NoArgumentEventHandler<LoadGameRequest> _loadGameRequestHandler;
        
        private List<Saveable> _saveables = new();

        private ISerializer _serializer;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
            _saveGameRequestHandler = new NoArgumentEventHandler<SaveGameRequest>(SaveGame);
            _loadGameRequestHandler = new NoArgumentEventHandler<LoadGameRequest>(LoadGame);
        }

        private void Start()
        {
            _serializer = ServiceLocator.Instance.GetService<ISerializer>();
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
            LoadedData = _serializer.Deserialize(GetPathString());

            foreach (var saveable in _saveables)
            {
                if (LoadedData.TryGetDataByGUID(saveable.GUID, out var data))
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
            bool success = _serializer.Serialize(_saveables, GetPathString());
            
            EventBus.Publish(new SaveGameResponse());
        }
        
        private static string GetPathString()
        {
            return Application.persistentDataPath + "/" + SaveFileName;
        }
    }
}
