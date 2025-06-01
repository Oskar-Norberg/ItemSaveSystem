namespace _Project.SaveSystem.SaveSubsystem
{
    public abstract class SaveSubsystem : ISaveSubsystem
    {
        public abstract SaveData GetSaveData();
        public abstract void Load(SaveData saveData);

        public void Register(SaveManager saveManager)
        {
            saveManager.RegisterSaveSubsystem(this);
        }
        
        public void Unregister(SaveManager saveManager)
        {
            saveManager.UnregisterSaveSubsystem(this);
        }
    }
}