using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;
using UnityEngine;

namespace ringo.SaveSystem.Subsystem
{
    public abstract class MonoSaveSubsystem : MonoBehaviour, ISaveSubsystem
    {
        public LoadStage SystemLoadStage => systemLoadStage;
        
        [SerializeField] private LoadStage systemLoadStage;

        // TODO: make this own a non-mono SaveLoader to avoid duplicate code.
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