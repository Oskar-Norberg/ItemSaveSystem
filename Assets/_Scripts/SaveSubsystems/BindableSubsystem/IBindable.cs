namespace _Project.SaveSubsystems.Bindable
{
    // TODO: Would be nice if I could put a RequireComponent Saveable, but alas. This is an interface.
    // TODO: Make this generic so that it can be used with any type of SaveData.
    public interface IBindable
    {
        public SaveData GetSaveData();
        public void LoadSaveData(SaveData saveData);
    }
}