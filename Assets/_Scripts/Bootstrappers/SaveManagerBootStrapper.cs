using _Project.SaveSystem.DataLoading.Binary;
using _Project.SaveSystem.Interfaces;
using _Project.SaveSystem.Interfaces.DataLoading.JSON;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SaveManagerBootStrapper : MonoBehaviour
    {
        enum SerializerType
        {
            JSON,
            Binary,
        }
        
        [SerializeField] private SerializerType serializerType;

        private void Awake()
        {
            ISerializer serializer;
            
            switch (serializerType)
            {
                case SerializerType.Binary:
                    serializer = new BinarySerializer();
                    break;
                // Default fallback to JSON.
                default:
                case SerializerType.JSON:
                    serializer = new JSONSerializer();
                    break;
            }
            
            var saveFileService = new SaveFileService(serializer);
            var saveManager = new SaveManager(saveFileService);
            ServiceLocator.Instance.Register<SaveManager>(saveManager);
        }
    }
}