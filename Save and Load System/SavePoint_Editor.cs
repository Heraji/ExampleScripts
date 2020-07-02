namespace SaveLoadSystem
{
#if UNITY_EDITOR

    using UnityEditor;
    using UnityEngine;

    #region Custom Inspector
    [CustomEditor(typeof(SavePoint))]
    public class SavePoint_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = target as SavePoint;

            if (script.gameObject.GetComponent<Collider>() == null)
            {
                EditorGUILayout.HelpBox("Please attach a collider below for this waypoint to work. Then scale it to your preferences.", MessageType.Warning);

                SerializedProperty colliderSelectProp = serializedObject.FindProperty("attachCollider");
                EditorGUILayout.PropertyField(colliderSelectProp);
                serializedObject.ApplyModifiedProperties();

                if ((int)script.attachCollider != 0)
                {
                    script.AttachCollider((int)script.attachCollider);
                    script.attachCollider = SavePoint.colliderTypes.None;
                }
            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
            }
        }
    }
    #endregion
#endif
}