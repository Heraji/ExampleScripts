namespace DoorSystem
{
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class Lock : MonoBehaviour
    {
        [Tooltip("Can be changed to your preference")] public string KeyTag = "Key";
        [Tooltip("Change to match the key's ID to be used on this lock")] public int KeyId = 0;

        private IDoor _doorReference;

        private void Start()
        {
            _doorReference = GetComponent<IDoor>() ?? GetComponentInParent<IDoor>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag(KeyTag)) return;
            IKey key = col.GetComponent<IKey>();

            CheckKey(key);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!col.gameObject.CompareTag(KeyTag)) return;
            IKey key = col.gameObject.GetComponent<IKey>();

            CheckKey(key);
        }

        private void CheckKey(IKey key)
        {
            if (key == null) return;

            if (key.GetKeyID() == KeyId)
                _doorReference?.UnlockDoor();
        }
    }
}