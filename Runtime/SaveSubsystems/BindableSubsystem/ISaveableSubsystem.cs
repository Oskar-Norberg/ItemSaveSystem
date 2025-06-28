namespace ringo.SaveModules.Subsystems.Bindable
{
    public interface ISaveableSubsystem
    {
        public void BindSaveable(Saveable saveable);
        public void UnbindSaveable(Saveable saveable);
    }
}