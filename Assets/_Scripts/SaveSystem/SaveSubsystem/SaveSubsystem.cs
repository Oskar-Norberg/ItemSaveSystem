using System.Threading.Tasks;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;

namespace ringo.SaveSystem.Subsystem
{
    public abstract class SaveSubsystem : ISaveSubsystem
    {
        public LoadStage SystemLoadStage { get; }
        
        public abstract SerializableGuid GUID { get; }


        public abstract object GetSaveData();

        public abstract Task Load(object saveData);

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