using System.Collections.Generic;
using UnityEngine;

namespace AQS.Core
{
    /// <summary>
    /// Event channel with no payload. ScriptableObject-based pub/sub.
    /// Create instances via Assets > Create > AQS > Events > Game Event.
    /// Listeners register/unregister at runtime; raising broadcasts to all.
    /// </summary>
    [CreateAssetMenu(fileName = "New GameEvent", menuName = "AQS/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }

    /// <summary>
    /// Event channel carrying a typed payload.
    /// Subclass for each concrete payload type needed.
    /// </summary>
    public abstract class GameEvent<T> : ScriptableObject
    {
        private readonly List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

        public void Raise(T payload)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(payload);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void UnregisterListener(IGameEventListener<T> listener)
        {
            listeners.Remove(listener);
        }
    }

    public interface IGameEventListener<T>
    {
        void OnEventRaised(T payload);
    }

    [CreateAssetMenu(fileName = "New IntEvent", menuName = "AQS/Events/Int Event")]
    public class GameEventInt : GameEvent<int> { }

    [CreateAssetMenu(fileName = "New FloatEvent", menuName = "AQS/Events/Float Event")]
    public class GameEventFloat : GameEvent<float> { }
}
