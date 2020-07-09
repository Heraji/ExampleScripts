namespace DoorSystem
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Key : MonoBehaviour, IKey
    {
        [Header("This object must have a tag to be identified by the lock.")]
        [Tooltip("Change to match the ID on the corresponding lock")] public int KeyId = 0;

        public int GetKeyID()
        {
            return KeyId;
        }
    }
}