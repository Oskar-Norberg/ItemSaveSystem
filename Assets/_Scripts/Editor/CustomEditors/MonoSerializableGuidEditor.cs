using UnityEditor;
using UnityEngine;

namespace _Project.SaveSystem.Editor.CustomEditors
{
    [CustomEditor(typeof(MonoSerializableGuid))]
    public class MonoSerializableGuidEditor : UnityEditor.Editor
    {
        private MonoSerializableGuid _currentTarget;
        private bool wasTargetChangedThisFrame;
        private string _newGuidString;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SetCurrentTarget();
            
            if (!_currentTarget)
            {
                return;
            }
            
            if (wasTargetChangedThisFrame)
            {
                OnTargetChanged();
            }

            ShowCurrentGUID();
            SetGUID();
            ResetGUID();
        }

        private void SetCurrentTarget()
        {
            var newTarget = target as MonoSerializableGuid;

            if (target)
            {
                wasTargetChangedThisFrame = newTarget != _currentTarget;
            }
            
            _currentTarget = newTarget;
        }

        private void OnTargetChanged()
        {
            _newGuidString = _currentTarget.GUIDString;
        }
        
        private void ShowCurrentGUID()
        {
            var monoSerializableGuid = _currentTarget;
            EditorGUILayout.LabelField("Current GUID", monoSerializableGuid.GUIDString);
        }

        private void SetGUID()
        {
            if (string.IsNullOrEmpty(_newGuidString))
            {
                _newGuidString = System.Guid.NewGuid().ToString();
            }
            
            // TODO: validate guid format.
            _newGuidString = EditorGUILayout.TextField("Set GUID", _newGuidString);
            
            if (GUILayout.Button("Set GUID"))
            {
                var monoSerializableGuid = _currentTarget;
                monoSerializableGuid.SetGUID(_newGuidString);
                EditorUtility.SetDirty(monoSerializableGuid);
            }
        }

        private void ResetGUID()
        {
            if (GUILayout.Button("Reset GUID"))
            {
                var monoSerializableGuid = _currentTarget;
                monoSerializableGuid.Reset();
                EditorUtility.SetDirty(monoSerializableGuid);
                
                // Reset the new guid field to the new guid.
                // TODO: Would it be overkill to make an internal event for this?
                _newGuidString = System.Guid.NewGuid().ToString();
            }
        }
    }
}