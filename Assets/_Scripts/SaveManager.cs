using System.Collections.Generic;
using System.IO;
using System.Text;
using _Project.SaveSystem;
using _Project.SaveSystem.Events;
using Newtonsoft.Json;
using ringo.EventSystem;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{   
    public class SaveManager : MonoBehaviour
    {
        const string SaveFileName = "save.json";
        
        private NoArgumentEventHandler<SaveGameRequest> _saveGameRequestHandler;
        
        private List<Saveable> _saveables = new();
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
            _saveGameRequestHandler = new NoArgumentEventHandler<SaveGameRequest>(SaveGame);
        }

        private void OnEnable()
        {
            _saveGameRequestHandler.Activate();
        }

        private void OnDisable()
        {
            _saveGameRequestHandler.Deactivate();
        }

        public void BindSaveable(Saveable saveable)
        {
            _saveables.Add(saveable);
        }

        private void SaveGame()
        {
            HeadJSONContainer headJSONContainer = new HeadJSONContainer();
            
            foreach (var saveable in _saveables)
            {
                var saveDatas = saveable.GetSaveData();

                SubJSONContainer container = new SubJSONContainer
                {
                    GUID = saveable.GUID,
                    SaveableType = saveable.SaveableType,
                    Data = saveDatas
                };
                
                headJSONContainer.AddSubContainer(container);
            }
            
            SaveToFile(JsonConvert.SerializeObject(headJSONContainer, Formatting.Indented));
            EventBus.Publish(new SaveGameResponse());
        }

        private void SaveToFile(string text)
        {
            // Currently just overriding the file.
            // Should probably only override changes.
            File.WriteAllText(GetPathString(), text);
        }
        
        private static string GetPathString()
        {
            return Application.persistentDataPath + "/" + SaveFileName;
        }
    }
}

[System.Serializable]
public class HeadJSONContainer
{
    public List<SubJSONContainer> SubContainers = new();
    
    public void AddSubContainer(SubJSONContainer subContainer)
    {
        SubContainers.Add(subContainer);
    }
}

[System.Serializable]
public struct SubJSONContainer
{
    public string GUID;
    public SaveableType SaveableType;
    public Dictionary<string, SaveData> Data;

    public SubJSONContainer(string guid, SaveableType saveableType, Dictionary<string, SaveData> data)
    {
        GUID = guid;
        Data = data;
        SaveableType = saveableType;
    }
}