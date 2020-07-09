namespace DoorSystem
{
    using UnityEngine;

    public class VanishingDoor : MonoBehaviour, IDoor
    {
        public bool DoorLocked;
        private MeshRenderer _mesh;
        private Collider[] _colliders;

        private IDoorSound _doorSound;

        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            _colliders = GetComponents<Collider>();
            _doorSound = GetComponent<IDoorSound>();
        }

        public void OpenDoor()
        {
            if (DoorLocked)
                return;

            VanishDoor(true);
            _doorSound?.PlayOpenDoor();
        }

        public void CloseDoor()
        {
            if (DoorLocked)
                return;

            VanishDoor(false);
            _doorSound?.PlayCloseDoor();
        }

        public void UnlockDoor()
        {
            if (!DoorLocked)
                return;

            DoorLocked = false;
            _doorSound?.PlayUnlockDoor();
        }

        public void LockDoor()
        {
            if (DoorLocked)
                return;

            DoorLocked = true;
            _doorSound?.PlayLockDoor();
        }

        private void VanishDoor(bool toggle)
        {
            _mesh.enabled = !toggle;

            foreach (var collider in _colliders)
            {
                collider.enabled = !toggle;
            }
        }
    }
}