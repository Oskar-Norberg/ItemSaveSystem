using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;

namespace ringo.SaveSystem.Subsystem
{
    public abstract class SaveSubsystem : ISaveSubsystem
    {
        public abstract SerializableGuid GUID { get; }
        
        public abstract object GetSaveData();

        public abstract void Load(object saveData);

        void ISaveSubsystem.Register(ISaveLoader saveManager)
        {
            saveManager.RegisterSaveSubsystem(this);
        }

        void ISaveSubsystem.Unregister(ISaveLoader saveManager)
        {
            saveManager.UnregisterSaveSubsystem(this);
        }
    }
}