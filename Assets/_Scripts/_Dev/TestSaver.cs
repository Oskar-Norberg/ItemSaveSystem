using System;
using _Project.SaveSystem.Events;
using ringo.EventSystem;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour
    {
        [SerializeField] private TestSaveData testSaveData;
        [SerializeField] private TestSaveData2 testSaveData2;
        
        private ArgumentEventHandler<LoadGameResponse> _loadGameResponseHandler;

        private void Awake()
        {
            _loadGameResponseHandler = new ArgumentEventHandler<LoadGameResponse>(Load);
        }
        
        private void Start()
        {
            GetComponent<Saveable>().BindSaveData(testSaveData);
            GetComponent<Saveable>().BindSaveData(testSaveData2);
        }

        private void OnEnable()
        {
            _loadGameResponseHandler.Activate();
        }

        private void OnDisable()
        {
            _loadGameResponseHandler.Deactivate();
        }

        private void Load(LoadGameResponse loadGameResponse)
        {
            LoadedData loadedData = loadGameResponse.LoadedData;
            
            // TODO: Saveable type is only stored on the saveable which makes it harder to access from here.
            // TODO: But this shouldnt really be done from here anyway. This should be done either in the saveable or in the save manager.
            Saveable saveable = GetComponent<Saveable>();
            SaveableType saveableType = saveable.SaveableType;
            TestSaveData loadedTestSaveData = loadedData.GetSaveData<TestSaveData>(saveable.SaveableType, saveable.GUID);
            
            testSaveData = loadedTestSaveData;
        }
    }
    
    [Serializable]
    public class TestSaveData : SaveData
    {
        public int X, Y, Z;
    }
    
    [Serializable]
    public class TestSaveData2 : SaveData
    {
        public string A, B, C;
    }
}