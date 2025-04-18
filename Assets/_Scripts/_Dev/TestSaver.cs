using System;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour
    {
        [SerializeField] private TestSaveData testSaveData;
        [SerializeField] private TestSaveData2 testSaveData2;
        
        private void Awake()
        {
            GetComponent<Saveable>().BindSaveData(testSaveData);
            GetComponent<Saveable>().BindSaveData(testSaveData2);
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