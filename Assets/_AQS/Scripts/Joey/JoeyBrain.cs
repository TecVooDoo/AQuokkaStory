using System.Collections.Generic;
using UnityEngine;

namespace AQS.Joey
{
    /// <summary>
    /// Autonomous decision-making for a Joey in the world.
    /// Reads JoeyGroundFollower state and drives JoeyController transitions.
    ///
    /// Chain-follow: Joeys form a line. The closest to Mom follows her directly.
    /// Each subsequent Joey follows the one in front of it. Order is recalculated
    /// each frame based on distance to Mom.
    ///
    /// Sprint 2 states: Idle, Following, WaitingAtEdge, InPouch, Launched.
    /// Future: InteractWithJoey, Kidnapped, FormingLadder.
    /// </summary>
    [RequireComponent(typeof(JoeyController))]
    [RequireComponent(typeof(JoeyGroundFollower))]
    public sealed class JoeyBrain : MonoBehaviour
    {
        // --- Static registry of all active JoeyBrains for chain-follow ordering ---
        private static readonly List<JoeyBrain> activeJoeys = new List<JoeyBrain>();
        private static float lastChainUpdateTime = -1f;

        [Header("References")]
        [Tooltip("Mom's transform. Set at scene placement or by spawner.")]
        [SerializeField] private Transform mom;

        [Header("Proximity")]
        [Tooltip("Distance within which Mom can pouch this Joey")]
        [SerializeField] private float pouchRange = 2f;

        [Tooltip("Distance beyond which this Joey loses track of Mom")]
        [SerializeField] private float loseTrackRange = 15f;

        private JoeyController controller;
        private JoeyGroundFollower groundFollower;
        private Rigidbody rb;

        /// <summary>True if Mom is close enough to pick up this Joey.</summary>
        public bool IsInPouchRange => mom != null
            && Vector3.Distance(transform.position, mom.position) <= pouchRange;

        /// <summary>True if this Joey can currently be pouched (in range + correct state).</summary>
        public bool CanBepouched
        {
            get
            {
                if (!IsInPouchRange) return false;
                JoeyState state = controller.CurrentState;
                return state == JoeyState.FollowingInLine
                    || state == JoeyState.Depleted;
            }
        }

        public Transform Mom
        {
            get => mom;
            set
            {
                mom = value;
                if (groundFollower != null)
                {
                    groundFollower.FollowTarget = value;
                    groundFollower.MomTransform = value;
                }
            }
        }

        private void Awake()
        {
            controller = GetComponent<JoeyController>();
            groundFollower = GetComponent<JoeyGroundFollower>();
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            if (!activeJoeys.Contains(this))
                activeJoeys.Add(this);
        }

        private void OnDisable()
        {
            activeJoeys.Remove(this);
        }

        private void Start()
        {
            if (mom != null && groundFollower != null)
            {
                groundFollower.MomTransform = mom;
                // FollowTarget is set by UpdateChainFollow each frame
            }
        }

        private void Update()
        {
            // One Joey per frame updates the chain-follow order for all
            if (mom != null && Time.time != lastChainUpdateTime)
            {
                lastChainUpdateTime = Time.time;
                UpdateChainFollow();
            }

            JoeyState state = controller.CurrentState;

            switch (state)
            {
                case JoeyState.InPouch:
                    UpdateInPouch();
                    break;

                case JoeyState.FollowingInLine:
                    UpdateFollowing();
                    break;

                case JoeyState.Depleted:
                    // Depleted behaves like Following but visually different (future)
                    UpdateFollowing();
                    break;

                case JoeyState.Aiming:
                case JoeyState.Launched:
                    // Controlled by JoeyLauncher -- brain does nothing
                    break;
            }
        }

        /// <summary>
        /// Sort all active following Joeys by distance to Mom, then assign
        /// chain-follow targets: closest follows Mom, each next follows the one ahead.
        /// </summary>
        private static void UpdateChainFollow()
        {
            // Gather Joeys that are actively following (not pouched, not launched)
            // Reuse activeJoeys list -- it already has all enabled brains
            Transform momRef = null;

            // Sort by Z distance to Mom (lateral axis)
            // Use insertion sort -- list is tiny (2-7 Joeys max)
            for (int i = 0; i < activeJoeys.Count; i++)
            {
                JoeyBrain brain = activeJoeys[i];
                if (brain.mom == null) continue;
                momRef = brain.mom;
                break;
            }

            if (momRef == null) return;

            // Build ordered list of following Joeys
            // We sort activeJoeys in-place by distance to Mom
            activeJoeys.Sort((a, b) =>
            {
                bool aFollowing = IsFollowingState(a);
                bool bFollowing = IsFollowingState(b);

                // Non-following Joeys go to end
                if (!aFollowing && !bFollowing) return 0;
                if (!aFollowing) return 1;
                if (!bFollowing) return -1;

                float distA = Mathf.Abs(a.transform.position.z - momRef.position.z);
                float distB = Mathf.Abs(b.transform.position.z - momRef.position.z);
                return distA.CompareTo(distB);
            });

            // Assign chain targets
            Transform previousTarget = momRef;
            for (int i = 0; i < activeJoeys.Count; i++)
            {
                JoeyBrain brain = activeJoeys[i];
                if (!IsFollowingState(brain)) continue;

                brain.groundFollower.FollowTarget = previousTarget;
                previousTarget = brain.transform;
            }
        }

        private static bool IsFollowingState(JoeyBrain brain)
        {
            JoeyState state = brain.controller.CurrentState;
            return state == JoeyState.FollowingInLine || state == JoeyState.Depleted;
        }

        private void UpdateInPouch()
        {
            // Joey is inside Mom -- hide visuals, disable physics
            groundFollower.EnableFollow(false);

            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Track Mom's position so we appear at her feet on unpouch
            transform.position = mom != null ? mom.position : transform.position;
        }

        private void UpdateFollowing()
        {
            groundFollower.EnableFollow(true);

            if (rb != null && rb.isKinematic)
            {
                rb.isKinematic = false;
            }

            // GroundFollower handles all movement logic, edge detection,
            // and out-of-range stopping. Brain just needs to read its state
            // for future use (UI indicators, SFX, idle triggers, etc.)
        }

        /// <summary>
        /// Called by PouchManager when Mom pouches this Joey.
        /// </summary>
        public void OnPouched()
        {
            controller.EnterPouch();
            groundFollower.EnableFollow(false);

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Hide the visual (disable renderers)
            SetRenderersEnabled(false);
        }

        /// <summary>
        /// Called by PouchManager when Mom unpouches this Joey.
        /// Joey appears at Mom's feet.
        /// </summary>
        public void OnUnpouched()
        {
            controller.ExitPouch();

            // Place at Mom's feet
            Vector3 dropPos = mom != null
                ? mom.position - mom.forward * 0.5f
                : transform.position;

            groundFollower.PlaceAt(dropPos);
            groundFollower.EnableFollow(true);

            if (rb != null)
            {
                rb.isKinematic = false;
            }

            SetRenderersEnabled(true);
        }

        /// <summary>
        /// Called by JoeyLauncher before launch. Prepares Joey for flight.
        /// </summary>
        public void OnLaunchPrepare()
        {
            groundFollower.EnableFollow(false);
            SetRenderersEnabled(true);

            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        /// <summary>
        /// Called by JoeyLauncher on auto-recall. Joey lands where it is.
        /// </summary>
        public void OnRecalled()
        {
            controller.Recall();
            groundFollower.EnableFollow(true);
            SetRenderersEnabled(true);

            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        private void SetRenderersEnabled(bool enabled)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = enabled;
            }
        }
    }
}
