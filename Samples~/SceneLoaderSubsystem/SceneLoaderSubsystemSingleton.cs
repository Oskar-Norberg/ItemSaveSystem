using ringo.SaveSystem.Singleton;
using UnityEngine;

namespace ringo.SaveModules.Subsystems.SceneLoader
{
    public class SceneLoaderSubsystemSingleton : SceneLoaderSubsystem
    {
        private void Start()
        {
            var saveLoader = SaveLoaderSingleton.Instance;
            if (saveLoader)
            {
                saveLoader.RegisterSaveSubsystem(this);
            }
            else
            {
                Debug.LogError("SaveLoaderSingleton is not initialized. SceneLoaderSubsystemSingleton cannot register.");
            }
        }

        private void OnDestroy()
        {
            var saveLoader = SaveLoaderSingleton.Instance;
            if (saveLoader)
            {
                saveLoader.UnregisterSaveSubsystem(this);
            }
            else
            {
                Debug.LogError("SaveLoaderSingleton is not initialized. SceneLoaderSubsystemSingleton cannot unregister.");
            }
        }
    }
}