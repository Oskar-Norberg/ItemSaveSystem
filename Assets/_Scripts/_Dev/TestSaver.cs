using _Project.SaveSubsystems.Bindable;
using UnityEngine;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour, IBindable
    {
        [SerializeField] private TestSaveData testSaveData;

        private void Start()
        {
            testSaveData ??= new TestSaveData();

            GetComponent<Saveable>().Bind("TestData", this);
        }
        
        private void OnDestroy()
        {
            GetComponent<Saveable>().Unbind("TestData");
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
}