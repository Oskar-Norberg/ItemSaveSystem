using System;
using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour
    {
        [SerializeField] private TestSaveData testSaveData;
        
        private void Awake()
        {
            GetComponent<Saveable>().BindSaveData(testSaveData);
        }
    }
    
    [Serializable]
    public class TestSaveData : SaveData
    {
        public int X, Y, Z;
    }
}