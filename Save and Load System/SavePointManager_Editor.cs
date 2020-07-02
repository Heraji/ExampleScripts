// Script by Jakob Elkjær Husted
namespace SaveLoadSystem
{
    #if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(SavePointManager))]
    public class SavePointManager_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = target as SavePointManager;

            EditorGUILayout.Space();

            if (script.savePoints != null)
            {
                EditorGUILayout.LabelField(script.savePoints.Count.ToString() + " SavePoints Active");
            }
            else
            {
                EditorGUILayout.LabelField("0 SavePoints Active");
            }

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("Please only create new savepoints by using the 'Add SavePoint' button. IMPORTANT: The first savepoint must be in front of the player's start position of the level! (not on the player)", MessageType.Info);

            EditorGUILayout.HelpBox("Currently a bug, IF PREFAB: Deleting SavePoints will only work if prefab is unpacked!", MessageType.Warning);

            if (GUILayout.Button("Add SavePoint"))
            {
                script.AddSavePoint();
            }

            EditorGUILayout.LabelField("Loads saved game (must be in scene where last saved)");
            if (GUILayout.Button("Load Game"))
            {
                script.LoadGame();
            }

            EditorGUILayout.LabelField("Loads to scene and then loads saved game");
            if (GUILayout.Button("Continue Game"))
            {
                script.Continue();
            }

            if (GUILayout.Button("Delete Save"))
            {
                script.DeleteSave();
            }

            serializedObject.ApplyModifiedProperties();
            
            if (GUI.changed)
                EditorUtility.SetDirty(script);
        }
    }
    #endif
}