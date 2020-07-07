namespace HustedAudioManager
{
    using System.Collections;
    using UnityEngine;

    [System.Serializable]
    public class SoundEvent
    {
        #region Class Variables
        [HideInInspector] public AudioManager thisAudioManager;
        [HideInInspector] public AudioClip audioClip;
        [HideInInspector] public AudioSource audioSource;
        [HideInInspector] public int soundEventId;
        [HideInInspector] public SoundVoidEventListener soundEventListener;
        [HideInInspector] public float triggerDelay = 0.0f, soundVolume = 1.0f, soundPitch = 1.0f;
        [HideInInspector] public VoidEvent triggerEvent;
        [HideInInspector] public bool runOnce, runOnStart, loopSound, instantiated;

        private bool eventFired = false;
        private float parsedValue = 0;
        private WaitForSeconds triggerDelayWait;
        #endregion

        public void SetUpEvent(int id)
        {
            triggerDelayWait = new WaitForSeconds(triggerDelay);
            soundEventId = id;
        }

        public void ResetEventFired()
        {
            eventFired = false;
        }

        public void Disable()
        {
            soundEventListener?.Disable();
        }

        public void Enable()
        {
            soundEventListener?.Enable();
        }

        public void EventRaised(float value)
        {
            parsedValue = value;

            if (!eventFired)
            {
                if (triggerDelay <= 0)
                    PlaySound();
                else if (triggerDelay > 0)
                    thisAudioManager.StartCoroutine(soundEventId);
                // Co-routines must be started from mono-behaviour class
            }

            if (runOnce)
            {
                eventFired = true;
                soundEventListener.Disable();
            }
        }

        private void PlaySound()
        {
            if (audioClip == null || audioSource == null)
                return;

            audioSource.loop = loopSound;
            audioSource.clip = audioClip;
            audioSource.volume = soundVolume;
            audioSource.pitch = soundPitch;
            audioSource.Play();
            //Debug.Log("Playing Sound: " + audioClip.name);
        }

        public IEnumerator WaitForDelay()
        {
            yield return triggerDelayWait;
            PlaySound();
        }
    }
}