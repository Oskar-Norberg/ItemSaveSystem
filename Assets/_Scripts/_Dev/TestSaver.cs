using _Project.SaveSystem.Interfaces;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour
    {
        [SerializeField] TestSaveData testSaveData;
        
        private void Awake()
        {
            GetComponent<Saveable>().BindSaveData(testSaveData);
        }

        public void Load(TestSaveData data)
        {
            testSaveData = data;
        }
    }
    
    [System.Serializable]
    public class TestSaveData : ISaveData
    {
        public int x, y, z;
        
        public object GetData()
        {
            return x + " " + y + " " + z;
        }
    }
}