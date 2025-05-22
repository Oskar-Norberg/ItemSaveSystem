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
                var saveManager = ServiceLocator.Instance.GetService<SaveManager>();
                
                if (saveManager == null)
                {
                    Debug.LogError("SaveManager not found in ServiceLocator.");
                    return;
                }
                saveManager.SaveGame(saveFileName, overrideSave);
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                var saveManager = ServiceLocator.Instance.GetService<SaveManager>();
                
                if (saveManager == null)
                {
                    Debug.LogError("SaveManager not found in ServiceLocator.");
                    return;
                }
                saveManager.LoadGame(saveFileName);
            }
        }
    }
}