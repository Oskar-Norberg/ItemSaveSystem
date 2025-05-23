using _Project.SaveSystem.DataLoading.Binary;
using _Project.SaveSystem.Interfaces;
using _Project.SaveSystem.Interfaces.DataLoading.JSON;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{
    public class SerializerBootStrapper : MonoBehaviour
    {
        enum SerializerType
        {
            JSON,
            Binary,
        }
        
        [SerializeField] private SerializerType serializerType;

        private void Awake()
        {
            switch (serializerType)
            {
                case SerializerType.Binary:
                    ServiceLocator.Instance.Register<ISerializer>(new BinarySerializer());
                    break;
                // Default fallback to JSON.
                default:
                case SerializerType.JSON:
                    ServiceLocator.Instance.Register<ISerializer>(new JSONSerializer());
                    break;
            }
        }
    }
}