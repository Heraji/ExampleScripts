// Example Script by Jakob Elkjær Husted
// Place this script and a door type script of your choice on your door object.
// This is just an example script to show how the door system works.
namespace DoorSystem
{
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class DoorExampleHandler : MonoBehaviour
    {
        public IDoor Door;

        private void Awake()
        {
            Door = GetComponent<IDoor>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DoorExampleHandler))]
    public class DoorExampleHandler_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This script is not necessary for the door system. It just acts as an example ofg usage.", MessageType.Info);

            var script = target as DoorExampleHandler;

            if (GUILayout.Button("Open Door"))
            {
                script.Door?.OpenDoor();
            }
            if (GUILayout.Button("Close Door"))
            {
                script.Door?.CloseDoor();
            }
            if (GUILayout.Button("Unlock Door"))
            {
                script.Door?.UnlockDoor();
            }
            if (GUILayout.Button("Lock Door"))
            {
                script.Door?.LockDoor();
            }
        }
    }
#endif
}