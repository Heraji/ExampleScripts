// Audio manager by Jakob E. Husted - An extended version that integrates all Wwise functionality exist, but requires Wwise to work.
// Contact me at Jakob.Hust@gmail.com if you need this version. 

namespace HustedAudioManager
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public SoundEvent[] soundEvents;
        // public WwiseEvent[] wwiseEvents;
        // NOTE: Other types of sound event classes can be referenced here or they can extend the normal SoundEvent class with this functionality (Like supporting Wwise)

        private AudioSource thisAudioSource;

        private void Awake()
        {
            thisAudioSource = GetComponent<AudioSource>();
            thisAudioSource.playOnAwake = false;
        }

        private void Start()
        {
            if (soundEvents != null)
                for (int i = 0; i < soundEvents.Length; i++)
                {
                    soundEvents[i].thisAudioManager = this;
                    soundEvents[i].soundEventListener = new SoundVoidEventListener();
                    soundEvents[i].soundEventListener.GameEvent = soundEvents[i].triggerEvent;
                    soundEvents[i].soundEventListener.SoundEventClass = soundEvents[i];
                    soundEvents[i].soundEventListener.Enable();
                    soundEvents[i].SetUpEvent(i);
                    soundEvents[i].audioSource = thisAudioSource;

                    if (soundEvents[i].runOnStart)
                        soundEvents[i].EventRaised(0);
                }
        }

        public void StartCoroutine(int id)
        {
            if (soundEvents[id] != null)
                StartCoroutine(soundEvents[id].WaitForDelay());
        }

        public void ExternalRaiseAll()
        {
            for (int i = 0; i < soundEvents.Length; i++)
            {
                soundEvents[i].EventRaised(0);
            }
        }

        public void ExternalRaiseAtIndex(int index)
        {
            if (index >= 0 && index < soundEvents.Length)
                soundEvents[index].EventRaised(0);
        }

        private void OnDisable()
        {
            if (soundEvents != null)
            {
                for (int i = 0; i < soundEvents.Length; i++)
                {
                    soundEvents[i].Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (soundEvents != null)
            {
                for (int i = 0; i < soundEvents.Length; i++)
                {
                    soundEvents[i].Enable();
                }
            }
        }
    }
}