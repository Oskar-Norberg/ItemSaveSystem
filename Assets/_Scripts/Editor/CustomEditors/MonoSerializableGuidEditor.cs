using UnityEditor;
using UnityEngine;

namespace _Project.SaveSystem.Editor.CustomEditors
{
    [CustomEditor(typeof(MonoSerializableGuid))]
    public class MonoSerializableGuidEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            ShowCurrentGUID();
            ResetGUID();
        }
        
        private void ShowCurrentGUID()
        {
            var monoSerializableGuid = (MonoSerializableGuid)target;
            EditorGUILayout.LabelField("Current GUID", monoSerializableGuid.GUIDString);
        }

        private void ResetGUID()
        {
            if (GUILayout.Button("Reset GUID"))
            {
                var monoSerializableGuid = (MonoSerializableGuid)target;
                monoSerializableGuid.Reset();
                EditorUtility.SetDirty(monoSerializableGuid);
            }
        }
    }
}