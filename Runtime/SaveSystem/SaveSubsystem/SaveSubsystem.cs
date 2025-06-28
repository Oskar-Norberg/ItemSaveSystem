using System.Threading.Tasks;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;

namespace ringo.SaveSystem.Subsystem
{
    /// <summary>
    /// Base class for a non-MonoBehaviour SaveSubsystem.
    /// </summary>
    /// <typeparam name="T">T is the type of data to save.</typeparam>
    public abstract class SaveSubsystem<T> : ISaveSubsystem
    {
        public LoadStage SystemLoadStage { get; }
        
        public abstract SerializableGuid GUID { get; }

        public object GetSaveData()
        {
            return GetSaveDataTyped();
        }

        public Task Load(object saveData)
        {
            if (saveData is not T typedSaveData)
            {
                throw new System.InvalidCastException($"SaveData of type {saveData.GetType()} is not type {typeof(T)}");
            }
            
            return LoadTyped(typedSaveData);
        }

        void ISaveSubsystem.Register(ISaveLoader saveManager)
        {
            saveManager.RegisterSaveSubsystem(this);
        }

        void ISaveSubsystem.Unregister(ISaveLoader saveManager)
        {
            saveManager.UnregisterSaveSubsystem(this);
        }
        
        // The only two functions that consumers of this class should need to implement.
        protected abstract T GetSaveDataTyped();
        protected abstract Task LoadTyped(T saveData);
    }
}