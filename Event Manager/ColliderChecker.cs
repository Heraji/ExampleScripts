// Script by Jakob Elkjær Husted. Used with EventManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HustedEventManager
{
    public class ColliderChecker : MonoBehaviour
    {
        private bool isActive, isColliding, isTrigger, fireOnce;
        private string tagName = "EventNone", eventToFire = "CollisionPlaceholderEvent";
        private float delayTime = 0f;
        private GameObject eventManagerObj;
        private EventManager parentEventManager;

        void Start()
        {
            if (gameObject.GetComponent<Collider>() == null)
                gameObject.AddComponent<Collider>();

            if (isTrigger)
                gameObject.GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (isActive)
            {
                if (isTrigger)
                {
                    if (tagName == "EventNone")
                        CollisionDetected();
                    else if (col.CompareTag(tagName))
                        CollisionDetected();
                }
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            if (isActive)
            {
                if (isTrigger == false)
                {
                    if (tagName == "EventNone")
                        CollisionDetected();
                    else if (col.gameObject.tag == tagName)
                        CollisionDetected();
                }
            }
        }

        private void CollisionDetected()
        {
            isColliding = true;
            parentEventManager?.Fire(eventToFire);
            DelayHandler();
        }

        private void DelayHandler()
        {
            if (fireOnce)
            {
                isActive = false;
            }
            else
            {
                Invoke("ColCooldown", delayTime);
            }
        }

        public void SetUpColliderChecker(string eName, float dTime, bool fOnce, bool triggerBool, GameObject eManObj, string tagString)
        {
            SetEventManagerObject(eManObj);
            SetEventName(eName);
            SetTag(tagString);
            SetDelayTime(dTime);
            fireOnce = fOnce;
            isTrigger = triggerBool;
            isActive = true;
            parentEventManager = eventManagerObj.GetComponent<EventManager>();
        }

        public void SetUpColliderChecker(string eName, float dTime, bool fOnce, bool triggerBool, GameObject eManObj)
        {
            SetEventManagerObject(eManObj);
            SetEventName(eName);
            SetDelayTime(dTime);
            fireOnce = fOnce;
            tagName = "EventNone";
            isTrigger = triggerBool;
            isActive = true;
            parentEventManager = eventManagerObj.GetComponent<EventManager>();
        }

        public bool IsColliding()
        {
            return isColliding;
        }

        public void SetTag(string tagString)
        {
            tagName = tagString;
        }

        public void SetEventName(string eName)
        {
            eventToFire = eName;
        }

        public void SetDelayTime(float delay)
        {
            delayTime = delay;
        }

        private void SetEventManagerObject(GameObject eManObj)
        {
            eventManagerObj = eManObj;
        }

        private void ColCooldown()
        {
            isActive = true;
        }
    }
}