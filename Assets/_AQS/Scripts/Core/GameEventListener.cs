using UnityEngine;
using UnityEngine.Events;

namespace AQS.Core
{
    /// <summary>
    /// MonoBehaviour that wires a GameEvent (void) SO to a UnityEvent in the inspector.
    /// Place on any GameObject, assign the event SO, and configure the response in inspector.
    /// Registers on enable, unregisters on disable.
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private UnityEvent response = new UnityEvent();

        private void OnEnable()
        {
            if (gameEvent != null)
            {
                gameEvent.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (gameEvent != null)
            {
                gameEvent.UnregisterListener(this);
            }
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}
