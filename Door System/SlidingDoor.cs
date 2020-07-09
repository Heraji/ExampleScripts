namespace DoorSystem
{
    using UnityEngine;

    public class SlidingDoor : MonoBehaviour, IDoor
    {
        public bool DoorLocked;
        public float MoveSpeed = 1.0f;

        private bool _doorControl, _doorMoving, _doorState;
        private Vector3 _startPos, _endPos;
        private float _fraction;

        private IDoorSound _doorSound;

        private void Start()
        {
            _startPos = transform.position;
            _endPos = GetEndPosition();
            _doorSound = GetComponent<IDoorSound>();
        }

        public void OpenDoor()
        {
            if (DoorLocked || _doorState)
                return;

            _doorControl = true;
            _doorMoving = true;
            _doorState = true;
            _doorSound?.PlayOpenDoor();
        }

        public void CloseDoor()
        {
            if (DoorLocked || !_doorState)
                return;

            _doorControl = false;
            _doorMoving = true;
            _doorState = false;
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

        private Vector3 GetEndPosition()
        {
            return new Vector3(transform.position.x, transform.position.y - (transform.localScale.y - 0.01f), transform.position.z);
        }

        private void SlideDoor(bool toggle)
        {
            if (!(_fraction < 1))
            {
                _fraction = 0;
                _doorMoving = false;
                return;
            }

            _fraction += Time.deltaTime * MoveSpeed;
            transform.position = toggle ? Vector3.Lerp(_startPos, _endPos, _fraction) : Vector3.Lerp(_endPos, _startPos, _fraction);
        }

        private void FixedUpdate()
        {
            if (_doorMoving)
                SlideDoor(_doorControl);
        }
    }
}