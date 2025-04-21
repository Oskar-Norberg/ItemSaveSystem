using System.Collections.Generic;
using System.IO;
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

        public void UnbindSaveable(Saveable saveable)
        {
            if (!_saveables.Remove(saveable))
            {
                Debug.LogWarning($"Saveable {saveable.GUIDString} not found in saveables list.");
            }
        }

        private void LoadGame()
        {
            if (!File.Exists(GetPathString()))
            {
                Debug.LogWarning("Save file does not exist.");
                return;
            }
            
            // TODO: Move this to a separate Serializer/Deserializer class.
            JsonSerializer jsonSerializer = new JsonSerializer();
            StreamReader streamReader = new StreamReader(GetPathString());
            JsonReader reader = new JsonTextReader(streamReader);
            
            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
                
            HeadJSONContainer headJSONContainer = jsonSerializer.Deserialize<HeadJSONContainer>(reader);
            
            reader.Close();
            streamReader.Close();
            
            LoadedData = new LoadedData(headJSONContainer);

            foreach (var saveable in _saveables)
            {
                if (LoadedData.TryGetDataByGUID(saveable.GUIDString, out var data))
                {
                    saveable.Load(data);
                }
                else
                {
                    Debug.LogWarning($"Saveable {saveable.GUIDString} not found in loaded data.");
                }
            }
            
            EventBus.Publish(new LoadGameResponse());
        }

        private void SaveGame()
        {
            HeadJSONContainer headJSONContainer = new HeadJSONContainer();
            
            foreach (var saveable in _saveables)
            {
                var saveDatas = saveable.GetSaveData();
            
                SubJSONContainer container = new SubJSONContainer
                {
                    GUID = saveable.GUIDString,
                    SaveableType = saveable.SaveableType,
                    Data = saveDatas
                };
                
                headJSONContainer.AddSubContainer(container);
            }
            
            JsonSerializer jsonSerializer = new JsonSerializer();
            StreamWriter streamWriter = new StreamWriter(GetPathString());
            JsonWriter writer = new JsonTextWriter(streamWriter);

            jsonSerializer.TypeNameHandling = TypeNameHandling.Objects;
            writer.Formatting = Formatting.Indented;
            
            jsonSerializer.Serialize(writer, headJSONContainer);
            
            writer.Close();
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
