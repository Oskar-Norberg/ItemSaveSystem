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
                
                if (saveableManager == null)
                {
                    Debug.LogError("SaveManager not found in ServiceLocator.");
                    return;
                }
                saveableManager.Save(saveFileName, overrideSave);
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                var saveableManager = ServiceLocator.Instance.GetService<ISaveableManager>();
                
                if (saveableManager == null)
                {
                    Debug.LogError("SaveManager not found in ServiceLocator.");
                    return;
                }
                
                saveableManager.Load(saveFileName);
            }
        }
    }
}