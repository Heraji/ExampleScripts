// Event Manager by Jakob Elkjær Husted. Feel free to add more event functionality to call in the Event class.
namespace HustedEventManager
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class EventManager : MonoBehaviour
    {
        public Event[] events;

        void Start()
        {
            Invoke("DelayedStart", PlayerPrefs.GetInt("loadGameOnAwake") == 1 ? 0.0f : 0.3f);
        }

        void DelayedStart()
        {
            for (int i = 0; i < events.Length; i++)
            {
                if (events[i].eventName != "NotNamedEvent" || events[i].eventToFire != null)
                {
                    events[i].attachedManager = this.gameObject;
                    events[i].SetUpEvent();
                    StartClassCoroutine(i, (int) events[i].function);
                }
                else
                {
                    #if UNITY_EDITOR
                    Debug.LogWarning("EventManager Notice: event number " + i + " is not set up correctly!");
                    #endif
                }

                if ((int) events[i].function == 6)
                {
                    events[i].eventToFire?.Invoke();
                }
            }
        }

        public void Fire(string eventName)
        {
            bool isFound = false;

            for (int i = 0; i < events.Length; i++)
            {
                if (eventName == events[i].eventName)
                {
                    isFound = true;
                    StartCoroutine(events[i].ExternalFire());
                }
            }

            if (isFound == false)
            {
                Debug.LogError("EventManager Error: " + eventName +
                               " not found in events! Please note that event names are case sensitive.");
            }
        }

        public void PlayerTeleportToPoint(GameObject pointObj)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null && pointObj != null)
            {
                player.transform.position = pointObj.transform.position;
            }
        }

        void StartClassCoroutine(int eventNum, int funcNum)
        {
            switch (funcNum)
            {
                case 0:
                    // ExternalFire selected, no need to run any functions
                    break;
                case 1:
                    events[eventNum].OnCollision();
                    break;
                case 2:
                    events[eventNum].OnCollisionWithTag();
                    break;
                case 3:
                    StartCoroutine(events[eventNum].OnObjectDestroy());
                    break;
                case 4:
                    StartCoroutine(events[eventNum].TimedEvent());
                    break;
                case 5:
                    StartCoroutine(events[eventNum].OnObjectMoving());
                    break;
                default:
                    break;
            }
        }

        public void ResetScene()
        {
            #if UNITY_EDITOR
            Debug.Log("EventManager: Reset Scene");
            #endif
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}