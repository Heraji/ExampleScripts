namespace HustedEventManager
{
    using System.Collections;
    using System.Collections.Generic;
    using Team1_GraduationGame.Events;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(EventManager))]
    public class EventManager_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var script = target as EventManager;

            if (script.events == null || script.events.Length == 0)
                EditorGUILayout.HelpBox("Add events by writing the desired amount of events below. Then click the event name to open the event settings.", MessageType.None);

            DrawDefaultInspector(); // for other non-HideInInspector fields


            if (script.events != null)
                for (int i = 0; i < script.events.Length; i++)
                {
                    if (script.events[i].eventName == "")
                        script.events[i].eventName = "NotNamedEvent";

                    script.events[i].activeInInspector = EditorGUILayout.Foldout(script.events[i].activeInInspector, script.events[i].eventName);

                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    if (script.events[i].activeInInspector)
                    {
                        if (GUILayout.Button("Test Fire '" + script.events[i].eventName + "' Event"))
                        {
                            script.events[i].eventToFire.Invoke();
                        }

                        GUILayout.Space(10);

                        script.events[i].eventName = EditorGUILayout.TextField("Event Name:", script.events[i].eventName);

                        SerializedProperty functionProp = serializedObject.FindProperty("events.Array.data[" + i + "].function");
                        EditorGUILayout.PropertyField(functionProp);

                        script.events[i].delayForFire = EditorGUILayout.FloatField("Fire Delay", script.events[i].delayForFire);


                        switch ((int)script.events[i].function)
                        {
                            case 0:
                                // External fire //
                                script.events[i].fireOnce =
                                    EditorGUILayout.Toggle("Fire only once?", script.events[i].fireOnce);
                                script.events[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.events[i].fireCooldown);
                                EditorGUILayout.HelpBox("This is an event you need to call from a script.\n" +
                                                        "You simply do that by calling the ExternalFire function in the EventManager script and give the specific event name as argument. " +
                                                        "If more events have the same name, they will also be fired. NOTICE, the names are case sensitive!", MessageType.Info);
                                break;
                            case 1:
                            case 2:
                            {
                                // Collision // 
                                script.events[i].fireOnce =
                                    EditorGUILayout.Toggle("Fire only once?", script.events[i].fireOnce);
                                script.events[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.events[i].fireCooldown);
                                script.events[i].isTrigger = EditorGUILayout.Toggle("Is Trigger?", script.events[i].isTrigger);
                                script.events[i].thisGameObject = EditorGUILayout.ObjectField("Collider Object", script.events[i].thisGameObject, typeof(GameObject), true) as GameObject;

                                switch ((int) script.events[i].function)
                                {
                                    case 1:
                                        // Collision // 
                                        EditorGUILayout.HelpBox("This fires an event when the selected object collides with anything. It required that one of the objects has a Rigidbody.", MessageType.Info);
                                        break;
                                    case 2:
                                        // Collision with tag //
                                        script.events[i].collisionTag = EditorGUILayout.TextField("Collision Tag Name", script.events[i].collisionTag);
                                        EditorGUILayout.HelpBox("This fires an event when the selected object collides with a specific tag. NOTICE, the tag names are case sensitive!. It is also required that one of the objects has a Rigidbody.", MessageType.Info);
                                        break;
                                }

                                break;
                            }
                            case 3:
                                // Check if object destroyed //
                                script.events[i].thisGameObject = EditorGUILayout.ObjectField("GameObject", script.events[i].thisGameObject, typeof(GameObject), true) as GameObject;
                                EditorGUILayout.HelpBox("Attach the GameObject you want to listen to. When this specific object is destroyed, the event will fire.", MessageType.Info);
                                break;
                            case 4:
                            case 5:
                                script.events[i].fireOnce =
                                    EditorGUILayout.Toggle("Fire only once?", script.events[i].fireOnce);
                                script.events[i].fireCooldown = EditorGUILayout.FloatField("Fire Cooldown", script.events[i].fireCooldown);
                            

                                switch ((int) script.events[i].function)
                                {
                                    case 4:
                                        // Timed event //
                                        EditorGUILayout.HelpBox("Specify the time interval for when the event should be fired", MessageType.Info);
                                        break;
                                    case 5:
                                        // Object moving // 
                                        EditorGUILayout.HelpBox("Cooldown controls how often it checks for movement. Lower values requires more performance!", MessageType.Info);
                                        script.events[i].thisGameObject = EditorGUILayout.ObjectField("GameObject", script.events[i].thisGameObject, typeof(GameObject), true) as GameObject;
                                        EditorGUILayout.HelpBox("Fires an event when the selected gameobject is moving", MessageType.Info);
                                        break;
                                }

                                break;
                            case 6:
                                EditorGUILayout.HelpBox("This event will fire during start()", MessageType.Info);
                                break;
                        }

                        GUILayout.Space(10);

                        SerializedProperty fireProp = serializedObject.FindProperty("events.Array.data[" + i + "].eventToFire");
                        EditorGUILayout.PropertyField(fireProp);
                        serializedObject.ApplyModifiedProperties();
                    }

                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
            }
        }
    }
#endif
}