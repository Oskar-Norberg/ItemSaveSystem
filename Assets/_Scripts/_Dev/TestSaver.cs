using System;
using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour, IBindable
    {
        [SerializeField] private TestSaveData testSaveData;

        private void Start()
        {
            GetComponent<Saveable>().Bind("TestData", this);
        }

        public SaveData GetSaveData()
        {
            return testSaveData;
        }

        public void LoadSaveData(SaveData saveData)
        {
            if (saveData is not TestSaveData testSaveData)
            {
                Debug.LogError($"Save data is not of type {nameof(TestSaveData)}");
                return;
            }
            
            this.testSaveData = testSaveData;
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