using ringo.SaveModules.Subsystems.Bindable;

namespace ringo.SaveSystem.Singleton
{
    public class SaveableSingleton : Saveable
    {
        private void Start()
        {
            SaveableManagerSingleton.Instance.BindSaveable(this);
        }

        private void OnDestroy()
        {
            var saveableManager = SaveableManagerSingleton.Instance;
            
            if (saveableManager)
                saveableManager.UnbindSaveable(this);
        }
    }
}