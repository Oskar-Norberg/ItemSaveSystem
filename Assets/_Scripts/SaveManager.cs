using System;
using System.Collections.Generic;
using System.IO;
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
        public LoadedData LoadedData { get; private set; }
        
        const string SaveFileName = "save.json";
        
        private NoArgumentEventHandler<SaveGameRequest> _saveGameRequestHandler;
        private NoArgumentEventHandler<LoadGameRequest> _loadGameRequestHandler;
        
        private List<Saveable> _saveables = new();
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
            _saveGameRequestHandler = new NoArgumentEventHandler<SaveGameRequest>(SaveGame);
            _loadGameRequestHandler = new NoArgumentEventHandler<LoadGameRequest>(LoadGame);
        }

        private void OnEnable()
        {
            _saveGameRequestHandler.Activate();
            _loadGameRequestHandler.Activate();
        }

        private void OnDisable()
        {
            _saveGameRequestHandler.Deactivate();
            _loadGameRequestHandler.Deactivate();
        }

        public void BindSaveable(Saveable saveable)
        {
            _saveables.Add(saveable);
        }

        private void LoadGame()
        {
            if (!File.Exists(GetPathString()))
            {
                Debug.LogWarning("Save file does not exist.");
                return;
            }
            
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
            
            StreamReader streamReader = new StreamReader(GetPathString());
                
            JsonReader reader = new JsonTextReader(streamReader);
            HeadJSONContainer headJSONContainer = jsonSerializer.Deserialize<HeadJSONContainer>(reader);
            streamReader.Close();
            
            LoadedData = new LoadedData(headJSONContainer);
            
            EventBus.Publish(new LoadGameResponse(LoadedData));
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
            
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
            
            StreamWriter streamWriter = new StreamWriter(GetPathString());
            
            JsonWriter writer = new JsonTextWriter(streamWriter);
            writer.Formatting = Formatting.Indented;
            
            jsonSerializer.Serialize(writer, headJSONContainer);
            writer.Flush();
            streamWriter.Close();
            
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

// TODO: theres a lot of voodoo going on with the dataTypeName, should really be passed around as a type instead.
public class LoadedData
{
    // TODO: Implement a way to get saveables by type.
    // private Dictionary<SaveableType, Dictionary<string, SubJSONContainer>> _saveablesByType = new();
    
    // String is the GUID of the saveable.
    private Dictionary<string, SubJSONContainer> _saveDatas = new();

    public LoadedData(HeadJSONContainer headJsonContainer)
    {
        // Populate _saveDatas with all subcontainers.
        foreach (var subContainer in headJsonContainer.SubContainers)
        {
            _saveDatas[subContainer.GUID] = subContainer;
        }

        // // Group subcontainers by SaveableType.
        // foreach (SaveableType type in Enum.GetValues(typeof(SaveableType)))
        // {
        //     var subContainers = new Dictionary<string, SubJSONContainer>();
        //
        //     foreach (var saveData in _saveDatas)
        //     {
        //         if (saveData.Value.SaveableType == type)
        //         {
        //             subContainers[saveData.Key] = saveData.Value;
        //         }
        //     }
        //
        //     _saveablesByType[type] = subContainers;
        // }
    }

    public T GetSaveData<T>(SaveableType type, string guid) where T : SaveData
    {
        if (_saveDatas.TryGetValue(guid, out var saveData))
        {
            // TODO O(n). This should be a dictionary. Will most likely be called quite frequently.
            foreach (var data in saveData.Data)
            {
                string typeName = typeof(T).Name;
                if (data.Key == typeName)
                    return data.Value as T;
            }
        }

        Debug.LogWarning("No save data found for GUID: " + guid + ", type: " + type + ", data type: " + typeof(T).Name);
        return null;
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