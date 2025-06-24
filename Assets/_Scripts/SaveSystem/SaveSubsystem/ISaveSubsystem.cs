using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;

namespace ringo.SaveSystem.Subsystem
{
    public interface ISaveSubsystem : IGUIDProvider
    {
        // TODO: Subsystems need to have a staging/order of execution. For example, scene loader needs to be executed before the SaveableManager.
        int ExecutionPriority { get; }
        
        // TODO: Explicitly forward the GUID from IGUIDProvider for clarity.
        
        object GetSaveData();
        void Load(object saveData);

        protected void Register(ISaveLoader saveManager);
        protected void Unregister(ISaveLoader saveManager);
    }
}