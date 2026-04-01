using UnityEngine;

namespace AQS.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class QuokkaController : MonoBehaviour
    {
        [Header("Hop Movement")]
        [SerializeField] private float slowHopForce = 4f;
        [SerializeField] private float fastHopForce = 8f;
        [SerializeField] private float hopCooldown = 0.15f;
        [SerializeField] private float airControlMultiplier = 0.3f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float jumpCutMultiplier = 0.4f;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float jumpBufferTime = 0.12f;
        [SerializeField] private float fallGravityMultiplier = 2.5f;
        [SerializeField] private float maxFallSpeed = 20f;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayers;

        [Header("Facing")]
        [SerializeField] private Transform modelTransform;
        [SerializeField] private bool startFacingRight = true;

        private Rigidbody rb;
        private Animator animator;

        private float moveInput;
        private bool jumpPressed;
        private bool jumpHeld;

        private bool isGrounded;
        private int groundContactCount;
        private float lastGroundedTime;
        private float lastJumpPressTime;

        private float lastHopTime;
        private bool isFacingRight;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            isFacingRight = startFacingRight;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

            if (modelTransform == null)
            {
                modelTransform = transform.GetChild(0);
            }

            float startYAngle = startFacingRight ? 90f : -90f;
            modelTransform.localRotation = Quaternion.Euler(0f, startYAngle, 0f);
        }

        private void Update()
        {
            HandleJumpBuffer();
            UpdateFacing();
        }

        private void FixedUpdate()
        {
            ApplyHopMovement();
            ApplyGravityModifiers();
            ClampFallSpeed();
        }

        #region Input

        public void SetMoveInput(float horizontal)
        {
            moveInput = horizontal;
        }

        public void OnJumpPressed()
        {
            jumpPressed = true;
            jumpHeld = true;
            lastJumpPressTime = Time.time;
        }

        public void OnJumpReleased()
        {
            jumpHeld = false;
            Vector3 vel = rb.linearVelocity;
            if (vel.y > 0f)
            {
                vel.y *= jumpCutMultiplier;
                rb.linearVelocity = vel;
            }
        }

        #endregion

        #region Movement

        private void ApplyHopMovement()
        {
            if (Mathf.Abs(moveInput) < 0.1f)
            {
                if (isGrounded)
                {
                    Vector3 vel = rb.linearVelocity;
                    vel.x = Mathf.MoveTowards(vel.x, 0f, 20f * Time.fixedDeltaTime);
                    rb.linearVelocity = vel;
                }
                return;
            }

            if (isGrounded)
            {
                if (Time.time - lastHopTime >= hopCooldown)
                {
                    float hopStrength = Mathf.Abs(moveInput) > 0.7f ? fastHopForce : slowHopForce;
                    float direction = Mathf.Sign(moveInput);
                    Vector3 vel = rb.linearVelocity;
                    vel.x = direction * hopStrength;
                    rb.linearVelocity = vel;
                    lastHopTime = Time.time;
                }
            }
            else
            {
                Vector3 vel = rb.linearVelocity;
                float targetSpeed = moveInput * fastHopForce;
                vel.x = Mathf.MoveTowards(vel.x, targetSpeed, fastHopForce * airControlMultiplier * Time.fixedDeltaTime);
                rb.linearVelocity = vel;
            }
        }

        private void ApplyGravityModifiers()
        {
            if (rb.linearVelocity.y < 0f)
            {
                float fallMultiplied = Physics.gravity.y * (fallGravityMultiplier - 1f);
                rb.AddForce(new Vector3(0f, fallMultiplied, 0f), ForceMode.Acceleration);
            }
        }

        private void ClampFallSpeed()
        {
            if (rb.linearVelocity.y < -maxFallSpeed)
            {
                Vector3 vel = rb.linearVelocity;
                vel.y = -maxFallSpeed;
                rb.linearVelocity = vel;
            }
        }

        #endregion

        #region Jump

        private void HandleJumpBuffer()
        {
            if (!jumpPressed) return;
            jumpPressed = false;

            bool canCoyoteJump = !isGrounded && (Time.time - lastGroundedTime <= coyoteTime);
            if (isGrounded || canCoyoteJump)
            {
                ExecuteJump();
            }
        }

        private void ExecuteJump()
        {
            Vector3 vel = rb.linearVelocity;
            vel.y = jumpForce;
            rb.linearVelocity = vel;
            isGrounded = false;
            lastGroundedTime = 0f;
        }

        #endregion

        #region Ground Detection

        private void OnCollisionEnter(Collision collision)
        {
            if (IsGroundLayer(collision.gameObject.layer))
            {
                if (HasUpwardContact(collision))
                {
                    groundContactCount++;
                }
                UpdateGroundedState();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (IsGroundLayer(collision.gameObject.layer))
            {
                if (HasUpwardContact(collision) && !isGrounded)
                {
                    groundContactCount++;
                    UpdateGroundedState();
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsGroundLayer(collision.gameObject.layer))
            {
                groundContactCount = Mathf.Max(0, groundContactCount - 1);
                UpdateGroundedState();
            }
        }

        private bool HasUpwardContact(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint contact = collision.GetContact(i);
                if (contact.normal.y > 0.5f)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateGroundedState()
        {
            bool wasGrounded = isGrounded;
            isGrounded = groundContactCount > 0;

            if (isGrounded)
            {
                lastGroundedTime = Time.time;
                if (!wasGrounded && Time.time - lastJumpPressTime <= jumpBufferTime)
                {
                    ExecuteJump();
                }
            }
        }

        private bool IsGroundLayer(int layer)
        {
            return (groundLayers.value & (1 << layer)) != 0;
        }

        #endregion

        #region Facing

        private void UpdateFacing()
        {
            if (moveInput > 0.1f && !isFacingRight)
            {
                Flip();
            }
            else if (moveInput < -0.1f && isFacingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            float yAngle = isFacingRight ? 90f : -90f;
            modelTransform.localRotation = Quaternion.Euler(0f, yAngle, 0f);
        }

        #endregion

        #region Public API

        public bool IsGrounded => isGrounded;
        public bool IsFacingRight => isFacingRight;
        public float MoveInput => moveInput;
        public Vector3 Velocity => rb.linearVelocity;

        #endregion
    }
}
