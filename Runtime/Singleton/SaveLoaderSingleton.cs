using ringo.SaveSystem.Managers;
using UnityEngine;

namespace ringo.SaveSystem.Singleton
{
    public class SaveLoaderSingleton : SaveLoader
    {
        public static SaveLoaderSingleton Instance => _instance;
        private static SaveLoaderSingleton _instance;

        private new void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                Debug.LogWarning("Duplicate SaveLoaderSingleton found");
                return;
            }
            
            _instance = this;
            base.Awake();
        }
    }
}