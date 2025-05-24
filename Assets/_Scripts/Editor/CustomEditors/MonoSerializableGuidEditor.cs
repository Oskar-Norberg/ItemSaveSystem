using UnityEditor;

namespace _Project.SaveSystem.Editor.CustomEditors
{
    [CustomEditor(typeof(MonoSerializableGuid))]
    public class MonoSerializableGuidEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            
        }
    }
}