using System;
using ringo.SaveSystem.Attributes;
using ringo.SaveSystem.GUID;
using ringo.SaveSystem.Managers;
using ringo.SaveSystem.Subsystem;
using ringo.SceneSystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace ringo.SaveModules.Subsystems.SceneLoader
{
    public class SceneLoaderSubsystem : MonoSaveSubsystem
    {
        public override SerializableGuid GUID => _guid;
        private SerializableGuid _guid = new("SceneLoaderSubsystem");
        
        private ISaveLoader _saveLoader;

        private void Start()
        {
            _saveLoader = GlobalServiceLocator.Instance.GetService<ISaveLoader>();
            _saveLoader.RegisterSaveSubsystem(this);
        }

        private void OnDestroy()
        {
            _saveLoader.UnregisterSaveSubsystem(this);
        }

        public override object GetSaveData()
        {
            return new SceneLoaderData { SceneGroup = SceneManager.CurrentSceneGroup };
        }

        public override void Load(object saveData)
        {
            var sceneLoaderData = saveData as SceneLoaderData;
            if (sceneLoaderData == null)
            {
                Debug.Log("No SceneGroup data found to load.");
                return;
            }

            SceneManager.LoadSceneGroup(sceneLoaderData.SceneGroup);
        }
    }
    
    [Serializable]
    [SaveData("SceneLoaderData")]
    class SceneLoaderData
    {
        public SceneGroup SceneGroup;
    }
}