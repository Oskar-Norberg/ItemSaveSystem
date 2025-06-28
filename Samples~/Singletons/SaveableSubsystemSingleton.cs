using ringo.SaveModules.Subsystems.Bindable;
using UnityEngine;

namespace ringo.SaveSystem.Singleton
{
    public class SaveableSubsystemSingleton : SaveableSubsystem
    {
        public static SaveableSubsystemSingleton Instance => _instance;
        private static SaveableSubsystemSingleton _instance;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                Debug.LogWarning("Duplicate SaveableSubsystemSingleton found");
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