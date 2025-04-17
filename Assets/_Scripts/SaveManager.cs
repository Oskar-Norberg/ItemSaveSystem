using System.Collections.Generic;
using _Project.SaveSystem.Events;
using ringo.EventSystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{   
    public class SaveManager : MonoBehaviour
    {
        private NoArgumentEventHandler<SaveGameRequest> _saveGameRequestHandler;
        
        private List<Saveable> _saveables = new();
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
            _saveGameRequestHandler = new NoArgumentEventHandler<SaveGameRequest>(SaveGame);
        }

        private void OnEnable()
        {
            _saveGameRequestHandler.Activate();
        }

        private void OnDisable()
        {
            _saveGameRequestHandler.Deactivate();
        }

        public void BindSaveable(Saveable saveable)
        {
            _saveables.Add(saveable);
        }

        private void SaveGame()
        {
            print("Saving!");
            
            foreach (var saveable in _saveables)
            {
                var saveDatas = saveable.GetSaveData();
                foreach (var data in saveDatas)
                {
                    print(data);
                }
            }
            
            EventBus.Publish(new SaveGameResponse());
        }
    }
}
