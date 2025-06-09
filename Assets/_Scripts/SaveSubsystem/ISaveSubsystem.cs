using _Project.SaveSystem.Interfaces;
using _Project.SaveSystem.SaveLoader;

namespace _Project.SaveSystem.Subsystem
{
    public interface ISaveSubsystem : IGUIDHolder
    {
        // TODO: Subsystems need to have a staging/order of execution. For example, scene loader needs to be executed before the SaveableManager.
        object GetSaveData();
        void Load(object saveData);

        protected void Register(ISaveLoader saveManager);
        protected void Unregister(ISaveLoader saveManager);
    }
}