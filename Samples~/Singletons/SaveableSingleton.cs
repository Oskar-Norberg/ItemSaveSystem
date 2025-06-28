using ringo.SaveModules.Subsystems.Bindable;

namespace ringo.SaveSystem.Singleton
{
    public class SaveableSingleton : Saveable
    {
        private void Start()
        {
            SaveableSubsystemSingleton.Instance.BindSaveable(this);
        }

        private void OnDestroy()
        {
            var saveableManager = SaveableSubsystemSingleton.Instance;
            
            if (saveableManager)
                saveableManager.UnbindSaveable(this);
        }
    }
}