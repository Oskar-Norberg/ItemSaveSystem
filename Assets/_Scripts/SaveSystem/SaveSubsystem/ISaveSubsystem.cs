using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;

namespace ringo.SaveSystem.Subsystem
{
    public interface ISaveSubsystem : IGUIDProvider
    {
        public LoadStage SystemLoadStage { get; }
        
        // TODO: Explicitly forward the GUID from IGUIDProvider for clarity.
        
        object GetSaveData();
        void Load(object saveData);

        protected void Register(ISaveLoader saveManager);
        protected void Unregister(ISaveLoader saveManager);
    }
}