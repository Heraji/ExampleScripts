// Event Manager by Jakob Elkjær Husted. Feel free to add more event functionality to call in event class.
namespace HustedEventManager
{
    using System.Collections;
    using System.Collections.Generic;
    using Team1_GraduationGame.Interaction;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class EventManager : MonoBehaviour
    {
        public ThisEventSystem[] events;

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

    [System.Serializable]
    public class ThisEventSystem
    {
        private bool hasFired = false;
        private WaitForSeconds _delayForFire, _coolDownDelay, _smallDelay = new WaitForSeconds(0.3f);

        [HideInInspector] public GameObject attachedManager;
        [HideInInspector] public bool activeInInspector = false;
        [HideInInspector] public string eventName = "NotNamedEvent";

        public enum myFuncEnum
        {
            ExternalFire,
            OnCollision,
            OnCollisionWithTag,
            OnObjectDestroy,
            TimedEvent,
            OnObjectMoving,
            OnStart
        };

        [HideInInspector] public UnityEvent eventToFire;

        [HideInInspector] public myFuncEnum function;

        [HideInInspector] public GameObject thisGameObject;
        [HideInInspector] public int gameObjectAmount = 0;
        [HideInInspector] public GameObject[] theseGameObjects;
        [HideInInspector] public string collisionTag = "";
        [HideInInspector] public bool isTrigger = true, fireOnce = true;
        [HideInInspector] public float fireCooldown = 0.0f, delayForFire = 0.0f;

        public void SetUpEvent()
        {
            _delayForFire = new WaitForSeconds(delayForFire);
            _coolDownDelay = new WaitForSeconds(fireCooldown);
        }

        public void OnCollisionWithTag()
        {
            if (thisGameObject != null)
            {
                if (thisGameObject.GetComponent<ColliderChecker>() == null)
                {
                    thisGameObject.AddComponent<ColliderChecker>();
                }

                if (thisGameObject.GetComponent<ColliderChecker>() != null)
                {

                    if (hasFired)
                    {
                        #if UNITY_EDITOR
                        Debug.Log(eventName + " event already fired");
                        #endif
                    }
                    else
                    {
                        thisGameObject.GetComponent<ColliderChecker>().SetUpColliderChecker(eventName, fireCooldown, fireOnce,
                            isTrigger, attachedManager, collisionTag);
                    }
                }
                else
                {
                    Debug.LogError(eventName + " error: ColliderChecker script missing!");
                }
            }
            else
            {
                Debug.LogError("EventSystem Error: No object attached for event " + eventName);
            }
        }

        public void OnCollision()
        {
            if (thisGameObject != null)
            {
                if (thisGameObject.GetComponent<ColliderChecker>() == null)
                {
                    thisGameObject.AddComponent<ColliderChecker>();
                }

                if (thisGameObject.GetComponent<ColliderChecker>() != null)
                {

                    if (hasFired)
                    {
                        #if UNITY_EDITOR
                        Debug.Log(eventName + " event already fired");
                        #endif
                    }
                    else
                    {
                        thisGameObject.GetComponent<ColliderChecker>()
                            .SetUpColliderChecker(eventName, fireCooldown, fireOnce, isTrigger, attachedManager);
                    }

                }
                else
                {
                    Debug.LogError(eventName + " error: ColliderChecker script missing!");
                }
            }
            else
            {
                Debug.LogError("EventSystem Error: No object attached for event " + eventName);
            }
        }

        public IEnumerator OnObjectDestroy()
        {
            if (hasFired)
            {
                Debug.Log(eventName + " event already fired");
            }
            else
            {
                bool loop = true;
                while (loop)
                {
                    yield return _smallDelay;

                    if (thisGameObject == null)
                    {
                        yield return _delayForFire;
                        loop = false;
                        eventToFire.Invoke();
                    }
                }
            }
        }

        public IEnumerator ExternalFire()
        {
            yield return _delayForFire;

            if (hasFired)
            {
                #if UNITy_EDITOR
                Debug.Log(eventName + " event already fired");
                #endif
            }
            else
            {
                eventToFire.Invoke();
                hasFired = true;
            }

            if (!fireOnce)
            {
                yield return _coolDownDelay;
                hasFired = false;
            }
        }

        public IEnumerator TimedEvent()
        {
            bool loop = true;
            while (loop)
            {
                yield return _delayForFire;

                eventToFire.Invoke();

                if (fireOnce)
                {
                    loop = false;
                }

                yield return _coolDownDelay;
            }
        }

        public IEnumerator OnObjectMoving()
        {
            Vector3 tempPos = thisGameObject.transform.position;
            bool loop = true;
            while (loop)
            {
                if (thisGameObject.transform.position != tempPos)
                {
                    yield return _delayForFire;

                    eventToFire.Invoke();

                    if (fireOnce)
                    {
                        loop = false;
                    }

                    yield return _smallDelay; // must as a minimum have this delay to prevent crash
                    yield return _coolDownDelay;
                }
            }
        }
    }
}