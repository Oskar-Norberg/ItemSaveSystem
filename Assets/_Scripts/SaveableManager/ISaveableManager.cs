namespace _Project.SaveSystem
{
    public interface ISaveableManager
    {
        public void BindSaveable(Saveable saveable);
        public void UnbindSaveable(Saveable saveable);
        
        public void Save(string fileName, bool doOverride = false);
        public void Load(string fileName);
    }
}