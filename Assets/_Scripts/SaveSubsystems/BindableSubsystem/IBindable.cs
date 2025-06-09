namespace ringo.SaveModules.Subsystems.Bindable
{
    // TODO: Would be nice if I could put a RequireComponent Saveable, but alas. This is an interface.
    // TODO: Make this generic so that it can be used with any type of SaveData.
    public interface IBindable
    {
        public object GetSaveData();
        public void LoadSaveData(object bindableData);
    }
}