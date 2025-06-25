using ringo.SaveModules.Subsystems.Bindable;
using UnityEngine;

namespace ringo.SaveSystem.Singleton
{
    public class SaveableManagerSingleton : SaveableManager
    {
        public static SaveableManagerSingleton Instance => _instance;
        private static SaveableManagerSingleton _instance;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                Debug.LogWarning("Duplicate SaveableManagerSingleton found");
                return;
            }
            
            _instance = this;
        }

        private void Start()
        {
            SaveLoaderSingleton.Instance.RegisterSaveSubsystem(this);
        }
        
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
            
            SaveLoaderSingleton.Instance.UnregisterSaveSubsystem(this);
        }
    }
}