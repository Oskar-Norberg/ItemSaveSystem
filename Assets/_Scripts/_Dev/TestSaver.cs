using System;
using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour, IBindable<TestSaveData>, IBindable<TestSaveData2>
    {
        [SerializeField] private TestSaveData testSaveData;
        [SerializeField] private TestSaveData2 testSaveData2;
        
        TestSaveData IBindable<TestSaveData>.GetSaveData()
        {
            return testSaveData;
        }
        
        TestSaveData2 IBindable<TestSaveData2>.GetSaveData()
        {
            return testSaveData2;
        }
        
        public void LoadSaveData(TestSaveData saveData)
        {
            testSaveData = saveData;
        }

        public void LoadSaveData(TestSaveData2 saveData)
        {
            testSaveData2 = saveData;
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