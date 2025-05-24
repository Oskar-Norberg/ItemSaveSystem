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

            HasPrefabOverride();
            IsDuplicate();

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

        private void HasPrefabOverride()
        {
            // Not a part of a prefab.
            if (!PrefabUtility.IsPartOfAnyPrefab(_currentTarget))
                return;
            
            var modifications = PrefabUtility.GetPropertyModifications(_currentTarget);
            
            bool guidModified = false;
            
            if (modifications != null && modifications.Length > 0)
            {
                foreach (var modification in modifications)
                {
                    if (modification.propertyPath.Contains("guid") || modification.propertyPath.Contains("GUID"))
                        guidModified = true;
                }
            }
            
            if (guidModified)
            {
                EditorGUILayout.HelpBox("GUID differs from prefab.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("GUID has not been changed from prefab.", MessageType.Info);
            }
        }

        private void IsDuplicate()
        {
            bool isDuplicate = false;
            
            var monoGuidObjects = FindObjectsByType(typeof(MonoSerializableGuid), FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            foreach (var monoGuidObject in monoGuidObjects)
            {
                var monoGuid = monoGuidObject as MonoSerializableGuid;
                
                if (!monoGuid)
                    continue;
                
                if (monoGuid == _currentTarget)
                    continue;
            
                if (!Equals(monoGuid.GUID, _currentTarget.GUID)) 
                    continue;
                
                isDuplicate = true;;
            }
            
            if (isDuplicate)
            {
                EditorGUILayout.HelpBox("This GUID is a duplicate of another ID in this scene. Ignore if this is intended.", MessageType.Info);
            }

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
            if (GUILayout.Button("Generate new GUID"))
            {
                _currentTarget.Reset();
                EditorUtility.SetDirty(_currentTarget);
                
                // Reset the new guid field to the new guid.
                // TODO: Would it be overkill to make an internal event for this?
                _newGuidString = _currentTarget.GUIDString;
            }
        }
    }
}