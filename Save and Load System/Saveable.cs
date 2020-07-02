// This script must be attached to all gameobjects you want to save. 
namespace SaveLoadSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Saveable : MonoBehaviour
    {
        public bool savePosition = true, saveRotation = true;

        private ContainerClass containerClass;

        public string JsonToString(Vector3 position)
        {
            return JsonUtility.ToJson(position);
        }

        public string JsonToString(Quaternion rotation)
        {
            return JsonUtility.ToJson(rotation);
        }

        public string JsonToString(bool isActive)
        {
            return JsonUtility.ToJson(isActive);
        }

        public string JsonToString(ContainerClass container)
        {
            return JsonUtility.ToJson(container);
        }

        public string ThisContainerToString()
        {
            return JsonUtility.ToJson(containerClass);
        }

        public void ConstructContainer()
        {
            containerClass = new ContainerClass();
        }

        public void ConstructContainer(ContainerClass container)
        {
            containerClass = container;
        }

        public ContainerClass GetContainer()
        {
            return containerClass;
        }

        public class ContainerClass
        {
            public string Name;
            public Vector3 Position;
            public Quaternion Rotation;
            public bool IsActive;
            // More saveable variables can be added later to this container. 
        }
    }
}