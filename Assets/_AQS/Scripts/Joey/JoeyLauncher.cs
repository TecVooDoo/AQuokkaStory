using UnityEngine;
using UnityEngine.InputSystem;

namespace AQS.Joey
{
    /// <summary>
    /// Handles aiming and launching the active Joey in a mortar arc.
    /// Hold LaunchJoey to aim (slow-mo + trajectory), release to fire.
    /// After launch, Joey auto-recalls to the back of the follow line.
    ///
    /// Attach to Mom (same GO as MAnimal). Requires a PouchManager reference.
    /// </summary>
    public sealed class JoeyLauncher : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PouchManager pouchManager;
        [SerializeField] private Transform launchOrigin;
        [SerializeField] private PlayerInput playerInput;

        [Header("Launch Config")]
        [Tooltip("Base launch force applied to the Joey")]
        [SerializeField] private float launchForce = 15f;

        [Tooltip("Angle above horizontal in degrees")]
        [SerializeField] [Range(10f, 80f)] private float launchAngle = 45f;

        [Tooltip("Time-slow multiplier during aiming (GDD: 0.80-0.85)")]
        [SerializeField] [Range(0.1f, 1f)] private float aimTimeScale = 0.85f;

        [Tooltip("Max seconds the aim can be held (prevents indefinite slow-mo)")]
        [SerializeField] private float maxAimDuration = 3f;

        [Header("Auto-Recall")]
        [Tooltip("Seconds after launch before Joey auto-recalls to back of line")]
        [SerializeField] private float autoRecallDelay = 3f;

        private InputAction launchAction;
        private bool isAiming;
        private float aimTimer;
        private float recallTimer;
        private JoeyController launchedJoey;

        private void OnEnable()
        {
            if (playerInput == null)
                playerInput = GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                launchAction = playerInput.actions["LaunchJoey"];

                if (launchAction != null)
                {
                    launchAction.started += OnLaunchStarted;
                    launchAction.canceled += OnLaunchReleased;
                }
            }
        }

        private void OnDisable()
        {
            if (launchAction != null)
            {
                launchAction.started -= OnLaunchStarted;
                launchAction.canceled -= OnLaunchReleased;
            }

            if (isAiming)
            {
                CancelAim();
            }
        }

        private void Update()
        {
            if (isAiming)
            {
                aimTimer += Time.unscaledDeltaTime;
                if (aimTimer >= maxAimDuration)
                {
                    FireActiveJoey();
                }
            }

            // Auto-recall timer
            if (launchedJoey != null)
            {
                recallTimer += Time.deltaTime;
                if (recallTimer >= autoRecallDelay)
                {
                    AutoRecall();
                }
            }
        }

        /// <summary>Hold starts aiming (slow-mo).</summary>
        private void OnLaunchStarted(InputAction.CallbackContext context)
        {
            if (pouchManager == null || pouchManager.IsPouchEmpty) return;

            JoeyController activeJoey = pouchManager.ActiveJoey;
            if (activeJoey == null) return;

            if (activeJoey.TryStartAiming())
            {
                isAiming = true;
                aimTimer = 0f;
                Time.timeScale = aimTimeScale;
                Time.fixedDeltaTime = 0.02f * aimTimeScale;
            }
        }

        /// <summary>Release fires the Joey.</summary>
        private void OnLaunchReleased(InputAction.CallbackContext context)
        {
            if (isAiming)
            {
                FireActiveJoey();
            }
        }

        private void FireActiveJoey()
        {
            if (pouchManager == null) return;

            JoeyController activeJoey = pouchManager.ActiveJoey;
            if (activeJoey == null)
            {
                CancelAim();
                return;
            }

            RestoreTimeScale();
            isAiming = false;

            if (activeJoey.TryLaunch())
            {
                // Prepare brain for launch (enables renderers, disables follow)
                JoeyBrain brain = activeJoey.GetComponent<JoeyBrain>();
                if (brain != null)
                    brain.OnLaunchPrepare();

                LaunchJoeyPhysics(activeJoey);
                launchedJoey = activeJoey;
                recallTimer = 0f;

                // Tell PouchManager the pouch is now empty
                pouchManager.OnJoeyLaunched(activeJoey);
            }
        }

        private void LaunchJoeyPhysics(JoeyController joey)
        {
            joey.transform.position = launchOrigin != null
                ? launchOrigin.position
                : transform.position + Vector3.up;

            // Mortar arc in 2.5D (Z/Y plane -- Z is lateral, X is locked depth)
            float facingSign = transform.forward.z >= 0f ? 1f : -1f;
            float angleRad = launchAngle * Mathf.Deg2Rad;

            Vector3 launchDirection = new Vector3(
                0f,
                Mathf.Sin(angleRad),
                facingSign * Mathf.Cos(angleRad)
            ).normalized;

            float force = launchForce;
            AbilityDefinition ability = joey.Definition.Ability;
            if (ability != null && ability.LaunchForceOverride > 0f)
            {
                force = ability.LaunchForceOverride;
            }

            Rigidbody rb = joey.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(launchDirection * force, ForceMode.VelocityChange);
            }
        }

        /// <summary>
        /// Auto-recall: stop physics, tell PouchManager to put Joey at back of line.
        /// </summary>
        private void AutoRecall()
        {
            if (launchedJoey == null) return;

            // Stop launch physics -- Joey stays where it landed
            Rigidbody rb = launchedJoey.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }

            // PouchManager tells the JoeyBrain to resume world behavior
            pouchManager.OnJoeyRecalled(launchedJoey);

            launchedJoey = null;
            recallTimer = 0f;
        }

        private void CancelAim()
        {
            if (pouchManager != null)
            {
                JoeyController activeJoey = pouchManager.ActiveJoey;
                if (activeJoey != null)
                {
                    activeJoey.CancelAiming();
                }
            }

            RestoreTimeScale();
            isAiming = false;
            aimTimer = 0f;
        }

        private void RestoreTimeScale()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}
