using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using _Project.SaveSystem;
using _Project.SaveSystem.Events;
using _Project.SaveSystem.Interfaces;
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
            StringBuilder jsonStringBuilder = new StringBuilder();
            
            foreach (var saveable in _saveables)
            {
                var saveDatas = saveable.GetSaveData();

                JSONContainer container = new JSONContainer
                {
                    GUID = saveable.GUID,
                    SaveableType = saveable.SaveableType,
                    Data = saveDatas
                };
                
                string jsonString = JsonConvert.SerializeObject(container, Formatting.Indented);
                jsonStringBuilder.Append(jsonString);
            }
            
            SaveToFile(jsonStringBuilder.ToString());
            
            EventBus.Publish(new SaveGameResponse());
        }

        private void SaveToFile(string text)
        {
            File.WriteAllText(GetPathString(), text);
        }
        
        private static string GetPathString()
        {
            return Application.persistentDataPath + "/" + SaveFileName;
        }
    }
}

[System.Serializable]
public struct JSONContainer
{
    public string GUID;
    public SaveableType SaveableType;
    public Dictionary<string, SaveData> Data;

    public JSONContainer(string guid, SaveableType saveableType, Dictionary<string, SaveData> data)
    {
        GUID = guid;
        Data = data;
        SaveableType = saveableType;
    }
}