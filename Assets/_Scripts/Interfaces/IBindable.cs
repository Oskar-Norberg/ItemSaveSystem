namespace _Project.SaveSystem.Interfaces
{
    public interface IBindable<T> where T : SaveData
    {
        public T GetSaveData();
        public void LoadSaveData(T saveData);
    }
}