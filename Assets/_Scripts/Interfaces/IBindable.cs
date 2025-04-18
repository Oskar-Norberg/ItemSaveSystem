namespace _Project.SaveSystem.Interfaces
{
    // TODO: Would be nice if I could put a RequireComponent Saveable, but alas. This is an interface.
    public interface IBindable<T> where T : SaveData
    {
        public T GetSaveData();
        public void LoadSaveData(T saveData);
    }
}