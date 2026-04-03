using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AQS.Core;

namespace AQS.Joey
{
    /// <summary>
    /// Manages pouching/unpouching of nearby Joeys.
    ///
    /// PouchManager does NOT own a list of all Joeys. Instead it discovers
    /// nearby JoeyBrain instances via OverlapSphere each time the player
    /// tries to cycle. Only Joeys within pouch range and in a valid state
    /// can be pouched.
    ///
    /// The pouch holds at most ONE Joey at a time.
    /// After launch or unpouch, the pouch is empty until the player cycles.
    ///
    /// Attach to Mom's root GameObject.
    /// </summary>
    public sealed class PouchManager : MonoBehaviour
    {
        [Header("Config")]
        [Tooltip("Transform used as Mom's position for proximity checks")]
        [SerializeField] private Transform momTransform;

        [Tooltip("Radius to scan for nearby Joeys when cycling")]
        [SerializeField] private float scanRadius = 3f;

        [Tooltip("Layer(s) Joey GameObjects live on")]
        [SerializeField] private LayerMask joeyLayer = ~0;

        [Header("Input")]
        [SerializeField] private PlayerInput playerInput;

        [Header("Events")]
        [SerializeField] private GameEvent onActiveJoeyChanged;
        [SerializeField] private GameEvent onJoeyRescued;
        [SerializeField] private GameEvent onPouchEmptied;

        private JoeyController pouchedJoey;

        private InputAction cycleNextAction;
        private InputAction cyclePrevAction;

        // Reusable buffer for OverlapSphere (no per-frame alloc)
        private readonly Collider[] scanBuffer = new Collider[16];

        // Cached list of nearby pouchable Joeys (rebuilt on each scan)
        private readonly List<JoeyBrain> nearbyJoeys = new List<JoeyBrain>();

        /// <summary>The Joey in the pouch. Null if empty.</summary>
        public JoeyController ActiveJoey => pouchedJoey;

        /// <summary>True if no Joey is pouched.</summary>
        public bool IsPouchEmpty => pouchedJoey == null;

        private void Awake()
        {
            if (momTransform == null)
                momTransform = transform;
        }

        private void OnEnable()
        {
            if (playerInput == null)
                playerInput = GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                cycleNextAction = playerInput.actions["CycleJoeyNext"];
                cyclePrevAction = playerInput.actions["CycleJoeyPrev"];

                if (cycleNextAction != null)
                    cycleNextAction.performed += OnCycleNext;
                if (cyclePrevAction != null)
                    cyclePrevAction.performed += OnCyclePrev;
            }
        }

        private void OnDisable()
        {
            if (cycleNextAction != null)
                cycleNextAction.performed -= OnCycleNext;
            if (cyclePrevAction != null)
                cyclePrevAction.performed -= OnCyclePrev;
        }

        /// <summary>
        /// Cycle: pouch the nearest available Joey.
        /// If pouch is occupied, unpouch current Joey first.
        /// </summary>
        public void CycleNext()
        {
            if (pouchedJoey != null)
            {
                UnpouchCurrent();
            }

            JoeyBrain nearest = FindNearestPouchableJoey();
            if (nearest != null)
            {
                PouchJoey(nearest);
            }
        }

        /// <summary>
        /// Reverse cycle: same as CycleNext but picks the farthest pouchable Joey.
        /// Gives the player a way to pick a specific Joey if multiple are nearby.
        /// </summary>
        public void CyclePrev()
        {
            if (pouchedJoey != null)
            {
                UnpouchCurrent();
            }

            JoeyBrain farthest = FindFarthestPouchableJoey();
            if (farthest != null)
            {
                PouchJoey(farthest);
            }
        }

        /// <summary>
        /// Called by JoeyLauncher when a Joey is launched.
        /// Clears the pouch.
        /// </summary>
        public void OnJoeyLaunched(JoeyController joey)
        {
            if (pouchedJoey == joey)
            {
                pouchedJoey = null;

                if (onPouchEmptied != null)
                    onPouchEmptied.Raise();
            }
        }

        /// <summary>
        /// Called by JoeyLauncher on auto-recall.
        /// Joey lands at its current position and resumes world behavior.
        /// PouchManager just clears its reference -- the Joey's brain handles the rest.
        /// </summary>
        public void OnJoeyRecalled(JoeyController joey)
        {
            JoeyBrain brain = joey.GetComponent<JoeyBrain>();
            if (brain != null)
            {
                brain.OnRecalled();
            }
        }

        private void PouchJoey(JoeyBrain brain)
        {
            pouchedJoey = brain.GetComponent<JoeyController>();
            brain.OnPouched();

            if (onActiveJoeyChanged != null)
                onActiveJoeyChanged.Raise();
        }

        private void UnpouchCurrent()
        {
            if (pouchedJoey == null) return;

            JoeyBrain brain = pouchedJoey.GetComponent<JoeyBrain>();
            if (brain != null)
            {
                brain.OnUnpouched();
            }

            pouchedJoey = null;

            if (onPouchEmptied != null)
                onPouchEmptied.Raise();
        }

        /// <summary>
        /// Scan for Joeys near Mom that are in a pouchable state.
        /// </summary>
        private void ScanNearbyJoeys()
        {
            nearbyJoeys.Clear();

            int hitCount = Physics.OverlapSphereNonAlloc(
                momTransform.position,
                scanRadius,
                scanBuffer,
                joeyLayer,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < hitCount; i++)
            {
                JoeyBrain brain = scanBuffer[i].GetComponent<JoeyBrain>();
                if (brain == null)
                    brain = scanBuffer[i].GetComponentInParent<JoeyBrain>();

                if (brain == null) continue;
                if (!brain.CanBepouched) continue;

                // Avoid duplicates (multiple colliders on same Joey)
                if (nearbyJoeys.Contains(brain)) continue;

                nearbyJoeys.Add(brain);
            }
        }

        private JoeyBrain FindNearestPouchableJoey()
        {
            ScanNearbyJoeys();

            JoeyBrain nearest = null;
            float nearestDist = float.MaxValue;

            for (int i = 0; i < nearbyJoeys.Count; i++)
            {
                float dist = Vector3.Distance(momTransform.position, nearbyJoeys[i].transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = nearbyJoeys[i];
                }
            }

            return nearest;
        }

        private JoeyBrain FindFarthestPouchableJoey()
        {
            ScanNearbyJoeys();

            JoeyBrain farthest = null;
            float farthestDist = 0f;

            for (int i = 0; i < nearbyJoeys.Count; i++)
            {
                float dist = Vector3.Distance(momTransform.position, nearbyJoeys[i].transform.position);
                if (dist > farthestDist)
                {
                    farthestDist = dist;
                    farthest = nearbyJoeys[i];
                }
            }

            return farthest;
        }

        private void OnCycleNext(InputAction.CallbackContext context)
        {
            CycleNext();
        }

        private void OnCyclePrev(InputAction.CallbackContext context)
        {
            CyclePrev();
        }
    }
}
