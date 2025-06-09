using ringo.SaveModules.Subsystems.Bindable;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.SaveSystem._Dev
{
    [RequireComponent(typeof(Saveable))]
    public class TestSaver : MonoBehaviour, IBindable
    {
        [FormerlySerializedAs("testSaveData")] [SerializeField] private TestBindableData testBindableData;

        private void Start()
        {
            testBindableData ??= new TestBindableData();

            GetComponent<Saveable>().Bind("TestData", this);
        }
        
        private void OnDestroy()
        {
            GetComponent<Saveable>().Unbind("TestData");
        }

        public object GetSaveData()
        {
            return testBindableData;
        }

        public void LoadSaveData(object bindableData)
        {
            if (bindableData is not TestBindableData testSaveData)
            {
                Debug.LogError($"Save data is not of type {nameof(TestBindableData)}");
                return;
            }
            
            this.testBindableData = testSaveData;
        }
    }
}