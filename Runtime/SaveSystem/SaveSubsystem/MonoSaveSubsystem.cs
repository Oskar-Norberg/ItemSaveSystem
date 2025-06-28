using System.Threading.Tasks;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;
using UnityEngine;

namespace ringo.SaveSystem.Subsystem
{
    public abstract class MonoSaveSubsystem<T> : MonoBehaviour, ISaveSubsystem
    {
        public LoadStage SystemLoadStage => systemLoadStage;
        
        [SerializeField] private LoadStage systemLoadStage;

        // TODO: make this own a non-mono SaveLoader to avoid duplicate code.
        public abstract SerializableGuid GUID { get; }

        public object GetSaveData()
        {
            return GetSaveDataTyped();
        }

        public Task Load(object saveData)
        {
            if (saveData is not T typedSaveData)
            {
                Debug.LogError($"SaveData of type {saveData.GetType()} is not type {typeof(T)}");
                return Task.CompletedTask;
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
        // TODO: Annoying that these are written both here and in non-mono SaveSubsystem.
        // TODO: ^ This is actually really annoying. This NEEDS to be fixed.
        // TODO: Consider if this really needs to be async. Consumer shouldn't need to worry about async vs sync.
        protected abstract T GetSaveDataTyped();
        protected abstract Task LoadTyped(T saveData);
    }
}