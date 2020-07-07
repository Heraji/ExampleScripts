namespace HustedEventManager
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public class Event
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
                #if UNITY_EDITOR
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