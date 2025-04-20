using System;
using System.Collections.Generic;
using System.Linq;
using _Project.SaveSystem.Interfaces;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    [Serializable]
    [RequireComponent(typeof(MonoSerializableGuid))]
    public class Saveable : MonoBehaviour, IGUIDHolder
    {
        public SaveableType SaveableType => saveableType;

        public string GUIDString => _monoSerializableGuid.GUIDString;
        public SerializableGuid GUID => _monoSerializableGuid.GUID;
        
        [SerializeField] private SaveableType saveableType;
        
        // String is name of bond.
        private Dictionary<string, IBindable> _bonds = new();
        
        private MonoSerializableGuid _monoSerializableGuid;
        
        private void Awake()
        {
            _monoSerializableGuid = GetComponent<MonoSerializableGuid>();
            
            // TODO: Unbind on destroy.
            ServiceLocator.Instance.GetService<SaveManager>().BindSaveable(this);
        }

        public void Bind(string bindName, IBindable bindable)
        {
            if (!_bonds.TryAdd(bindName, bindable))
            {
                Debug.LogWarning($"Bond {bindName} already exists in {gameObject.name}");
            }
        }
        
        public void Unbind(string bindName)
        {
            if (!_bonds.Remove(bindName))
            {
                Debug.LogWarning($"Bond {bindName} does not exist in {gameObject.name}");
            }
        }
        
        public void Load(Dictionary<string, SaveData> loadedData)
        {
            foreach (var kvp in loadedData)
            {
                if (_bonds.TryGetValue(kvp.Key, out var bindable))
                {
                    bindable.LoadSaveData(kvp.Value);
                }
                else
                {
                    Debug.LogWarning($"No bond found for {kvp.Key} in {gameObject.name}");
                }
            }
        }
        
        public Dictionary<string, SaveData> GetSaveData()
        {
            // TODO: Consider making this a list of SaveData instead of a dictionary.
            // TODO: This will generate a bit of garbage.
            return _bonds.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetSaveData());
        }
    }
}