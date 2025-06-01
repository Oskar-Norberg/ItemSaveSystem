namespace _Project.SaveSystem
{
    public interface ISaveableManager
    {
        public void BindSaveable(Saveable saveable);
        public void UnbindSaveable(Saveable saveable);
    }
}