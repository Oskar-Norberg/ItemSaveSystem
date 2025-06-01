using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem.SaveGameEventSender
{
    public class SaveGameEventSender : MonoBehaviour
    {
        [SerializeField] private string saveFileName = "TheData";
        [SerializeField] private bool overrideSave = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                var saveableManager = ServiceLocator.Instance.GetService<ISaveableManager>();
                var saveableData = saveableManager.GetSaveData();
                
                var saveManager = ServiceLocator.Instance.GetService<SaveManager>();
                saveManager.SaveGame(saveFileName, saveableData, overrideSave);
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                var saveManager = ServiceLocator.Instance.GetService<SaveManager>();
                var loadedData = saveManager.LoadGame(saveFileName);
                
                var saveableManager = ServiceLocator.Instance.GetService<ISaveableManager>();
                saveableManager.Load(loadedData);
            }
        }
    }
}