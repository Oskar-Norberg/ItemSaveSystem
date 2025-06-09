using _Project.SaveSystem.SaveLoader;
using UnityEngine;

namespace _Project.SaveSystem.Subsystem
{
    public abstract class MonoSaveSubsystem : MonoBehaviour, ISaveSubsystem
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