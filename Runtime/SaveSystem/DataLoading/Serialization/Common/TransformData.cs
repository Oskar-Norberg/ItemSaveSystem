using UnityEngine;

namespace ringo.SaveSystem.DataLoading.Serialization.Common
{
    public struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        
        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public void ApplyTo(Transform transform)
        {
            transform.position = Position;
            transform.rotation = Rotation;
            transform.localScale = Scale;
        }
    }
}