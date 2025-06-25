using ringo.SaveSystem.DataLoading.Serialization;
using ringo.SaveSystem.DataLoading.Serialization.Binary;
using ringo.SaveSystem.DataLoading.Serialization.JSON;
using ringo.SaveSystem.Managers;
using ringo.SaveSystem.Services;
using ringo.ServiceLocator;
using UnityEngine;

namespace ringo.SaveSystem.Bootstrappers
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
            GlobalServiceLocator.Instance.Register<SaveManager>(saveManager);
        }
    }
}