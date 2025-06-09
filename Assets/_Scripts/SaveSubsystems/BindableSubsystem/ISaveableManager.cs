namespace _Project.SaveSubsystems.Bindable
{
    public interface ISaveableManager
    {
        public void BindSaveable(Saveable saveable);
        public void UnbindSaveable(Saveable saveable);
    }
}