namespace DoorSystem
{
    public interface IDoorSound
    {
        void PlayOpenDoor();
        void PlayCloseDoor();
        void PlayLockDoor();
        void PlayUnlockDoor();
    }
}