namespace HustedAudioManager
{
    using UnityEngine;

    public abstract class SoundEventListener<T, E, SE> :
        IGameEventListener<T> where E : BaseGameEvent<T> where SE : SoundEvent
    {
        [SerializeField] private E gameEvent;
        [SerializeField] private SE soundEventClass;

        public E GameEvent
        {
            get { return gameEvent; }
            set { gameEvent = value; }
        }

        public SE SoundEventClass
        {
            get { return soundEventClass; }
            set { soundEventClass = value; }
        }

        public void Enable()
        {
            if (gameEvent == null)
            {
                return;
            }

            GameEvent.RegisterListener(this);
        }

        public void Disable()
        {
            if (gameEvent == null)
            {
                return;
            }

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (item.GetType() == typeof(Void))
            {
                SoundEventClass.EventRaised(0);
            }
            else if (item.GetType() == typeof(float))
            {
                float tempFloat = float.Parse(item.ToString());
                SoundEventClass.EventRaised(tempFloat);
            }
        }
    }

    public class SoundVoidEventListener : SoundEventListener<Void, VoidEvent, SoundEvent>
    {
    }
}