using UnityEngine;

namespace AQS.Joey
{
    /// <summary>
    /// Ground-based follow movement for Joeys. Hops toward a target using
    /// Rigidbody physics. Detects ledges and gaps it can't cross and stops.
    ///
    /// 2.5D orientation: Z is lateral movement, X is depth (locked), Y is up.
    /// Camera looks down -X.
    ///
    /// Requires: Rigidbody (non-kinematic when following), CapsuleCollider.
    /// </summary>
    public sealed class JoeyGroundFollower : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Lateral move speed (along Z)")]
        [SerializeField] private float moveSpeed = 3f;

        [Tooltip("Force applied per hop")]
        [SerializeField] private float hopForce = 4f;

        [Tooltip("Seconds between hops while moving")]
        [SerializeField] private float hopInterval = 0.4f;

        [Header("Follow")]
        [Tooltip("Distance at which Joey starts moving toward target")]
        [SerializeField] private float followStartDistance = 2.5f;

        [Tooltip("Distance at which Joey stops (close enough)")]
        [SerializeField] private float followStopDistance = 1.2f;

        [Tooltip("Distance beyond which Joey gives up following (out of range)")]
        [SerializeField] private float maxFollowRange = 15f;

        [Header("Edge Detection")]
        [Tooltip("How far ahead to check for ground (along Z)")]
        [SerializeField] private float edgeCheckDistance = 0.6f;

        [Tooltip("How far down to raycast for ground at the edge")]
        [SerializeField] private float edgeCheckDepth = 1.5f;

        [Tooltip("Max height difference Joey can step up")]
        [SerializeField] private float maxStepHeight = 0.4f;

        [Tooltip("Max height Joey can hop up to (without help from other Joeys)")]
        [SerializeField] private float maxHopHeight = 0.8f;

        [Header("Wall Detection")]
        [Tooltip("How far ahead to check for walls (along Z)")]
        [SerializeField] private float wallCheckDistance = 0.4f;

        [Tooltip("Max wall height Joey can hop over")]
        [SerializeField] private float maxWallHopHeight = 0.8f;

        [Header("Ground Check")]
        [Tooltip("Layers considered ground")]
        [SerializeField] private LayerMask groundLayers = ~0;

        [Tooltip("Radius of the ground check sphere")]
        [SerializeField] private float groundCheckRadius = 0.15f;

        [Tooltip("Offset below center for ground check")]
        [SerializeField] private float groundCheckOffset = 0.05f;

        [Header("Mom Dodge")]
        [Tooltip("Distance at which Joey reacts to Mom approaching")]
        [SerializeField] private float dodgeDetectRange = 1.2f;

        [Tooltip("Upward hop force when dodging Mom")]
        [SerializeField] private float dodgeHopForce = 3f;

        [Tooltip("Push-back force along Z when dodging Mom")]
        [SerializeField] private float dodgePushForce = 2f;

        [Tooltip("Cooldown between dodge hops")]
        [SerializeField] private float dodgeCooldown = 0.8f;

        private Transform followTarget;
        private Transform momTransform;
        private Rigidbody rb;
        private CapsuleCollider capsule;
        private float hopTimer;
        private float dodgeTimer;
        private bool isGrounded;
        private bool isBlocked;
        private bool isFollowEnabled = true;
        private bool isOutOfRange;
        private Vector3 lastGroundedPosition;

        /// <summary>True if grounded and a ledge/wall prevents forward movement.</summary>
        public bool IsBlocked => isBlocked;

        /// <summary>True if the target is beyond maxFollowRange.</summary>
        public bool IsOutOfRange => isOutOfRange;

        /// <summary>True if the Joey is on the ground.</summary>
        public bool IsGrounded => isGrounded;

        /// <summary>True if within follow stop distance of target.</summary>
        public bool IsNearTarget => followTarget != null
            && LateralDistance(transform.position, followTarget.position) <= followStopDistance;

        public Transform FollowTarget
        {
            get => followTarget;
            set => followTarget = value;
        }

        /// <summary>Set Mom's transform for dodge detection.</summary>
        public Transform MomTransform
        {
            get => momTransform;
            set => momTransform = value;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();
        }

        /// <summary>Enable or disable follow movement.</summary>
        public void EnableFollow(bool enabled)
        {
            isFollowEnabled = enabled;

            if (rb != null && !enabled)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }

        /// <summary>Place Joey at a specific world position (unpouch, recall).</summary>
        public void PlaceAt(Vector3 position)
        {
            transform.position = position;
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }
            lastGroundedPosition = position;
        }

        private void FixedUpdate()
        {
            if (!isFollowEnabled || followTarget == null) return;

            CheckGrounded();

            if (!isGrounded) return;

            lastGroundedPosition = transform.position;

            // Dodge Mom if she's walking toward us
            if (dodgeTimer > 0f)
                dodgeTimer -= Time.fixedDeltaTime;

            if (dodgeTimer <= 0f && CheckMomApproaching())
            {
                DodgeMom();
                return;
            }

            float lateralDist = LateralDistance(transform.position, followTarget.position);

            isOutOfRange = lateralDist > maxFollowRange;

            if (isOutOfRange || lateralDist <= followStopDistance)
            {
                isBlocked = false;
                return;
            }

            // Hysteresis: don't start moving until Mom is beyond start distance
            if (lateralDist < followStartDistance)
            {
                // Already close enough -- just stop
                Vector3 vel = rb.linearVelocity;
                vel.z = 0f;
                rb.linearVelocity = vel;
                return;
            }

            // Move direction along Z axis (2.5D lateral)
            float directionZ = Mathf.Sign(followTarget.position.z - transform.position.z);

            // Check for blocking obstacles
            bool edgeSafe = CheckEdgeAhead(directionZ);
            bool wallClear = CheckWallAhead(directionZ);

            isBlocked = !edgeSafe || !wallClear;

            if (isBlocked)
            {
                Vector3 vel = rb.linearVelocity;
                vel.z = 0f;
                rb.linearVelocity = vel;
                return;
            }

            // Move along Z
            Vector3 currentVel = rb.linearVelocity;
            currentVel.z = directionZ * moveSpeed;
            rb.linearVelocity = currentVel;

            // Hop periodically for visual bounce
            hopTimer -= Time.fixedDeltaTime;
            if (hopTimer <= 0f)
            {
                rb.AddForce(Vector3.up * hopForce, ForceMode.VelocityChange);
                hopTimer = hopInterval;
            }

            // Face movement direction (2.5D: rotate to face along Z)
            if (Mathf.Abs(directionZ) > 0.01f)
            {
                Vector3 forward = new Vector3(0f, 0f, directionZ);
                transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }
        }

        /// <summary>
        /// Check if there is ground ahead along Z.
        /// </summary>
        private bool CheckEdgeAhead(float directionZ)
        {
            Vector3 origin = transform.position + Vector3.up * 0.1f;
            origin.z += directionZ * edgeCheckDistance;

            bool groundAhead = Physics.Raycast(
                origin,
                Vector3.down,
                out RaycastHit hit,
                edgeCheckDepth + 0.1f,
                groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (!groundAhead)
                return false;

            float heightDiff = transform.position.y - hit.point.y;

            if (heightDiff < -maxStepHeight)
                return true; // Ground above us -- wall check handles this

            if (heightDiff > edgeCheckDepth)
                return false;

            return true;
        }

        /// <summary>
        /// Check if a wall blocks forward movement along Z.
        /// </summary>
        private bool CheckWallAhead(float directionZ)
        {
            float halfHeight = capsule != null ? capsule.height * 0.5f : 0.5f;
            Vector3 origin = transform.position + Vector3.up * halfHeight;
            Vector3 direction = new Vector3(0f, 0f, directionZ);

            bool wallHit = Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                wallCheckDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (!wallHit) return true;

            Vector3 topCheckOrigin = transform.position + Vector3.up * (maxWallHopHeight + 0.1f);
            topCheckOrigin.z += directionZ * wallCheckDistance;

            bool canHopOver = !Physics.Raycast(
                topCheckOrigin,
                direction,
                wallCheckDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (!canHopOver) return false;

            Vector3 wallTopCheck = transform.position + Vector3.up * maxWallHopHeight;
            wallTopCheck.z += directionZ * (wallCheckDistance + 0.1f);

            bool groundOnTop = Physics.Raycast(
                wallTopCheck,
                Vector3.down,
                out RaycastHit topHit,
                maxWallHopHeight,
                groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (groundOnTop)
            {
                float stepUpHeight = topHit.point.y - transform.position.y;
                return stepUpHeight <= maxHopHeight;
            }

            return false;
        }

        private void CheckGrounded()
        {
            float checkY = capsule != null
                ? capsule.center.y - capsule.height * 0.5f
                : 0f;

            Vector3 checkPos = transform.position + Vector3.up * (checkY + groundCheckRadius + groundCheckOffset);

            isGrounded = Physics.CheckSphere(
                checkPos,
                groundCheckRadius,
                groundLayers,
                QueryTriggerInteraction.Ignore
            );
        }

        /// <summary>
        /// Returns true if Mom is close and moving toward this Joey along Z.
        /// </summary>
        private bool CheckMomApproaching()
        {
            if (momTransform == null) return false;

            float dist = LateralDistance(transform.position, momTransform.position);
            if (dist > dodgeDetectRange) return false;

            Rigidbody momRb = momTransform.GetComponent<Rigidbody>();
            if (momRb == null) return false;

            float momVelZ = momRb.linearVelocity.z;
            if (Mathf.Abs(momVelZ) < 0.5f) return false;

            float dirToJoey = Mathf.Sign(transform.position.z - momTransform.position.z);
            float momMoveDir = Mathf.Sign(momVelZ);

            return Mathf.Approximately(dirToJoey, momMoveDir);
        }

        /// <summary>
        /// Hop backward along Z out of Mom's path.
        /// </summary>
        private void DodgeMom()
        {
            float awayDirZ = Mathf.Sign(transform.position.z - momTransform.position.z);

            if (Mathf.Abs(awayDirZ) < 0.01f)
                awayDirZ = Mathf.Sign(momTransform.forward.z);

            Vector3 dodgeForce = new Vector3(0f, dodgeHopForce, awayDirZ * dodgePushForce);
            rb.AddForce(dodgeForce, ForceMode.VelocityChange);

            dodgeTimer = dodgeCooldown;
        }

        /// <summary>Lateral distance along Z axis only (2.5D movement axis).</summary>
        private float LateralDistance(Vector3 a, Vector3 b)
        {
            return Mathf.Abs(a.z - b.z);
        }
    }
}
