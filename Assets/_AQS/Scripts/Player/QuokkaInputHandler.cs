using UnityEngine;
using UnityEngine.InputSystem;

namespace AQS.Player
{
    /// <summary>
    /// Polls input each frame instead of using callbacks.
    /// More reliable for composite bindings (WASD) and direction changes.
    /// </summary>
    [RequireComponent(typeof(QuokkaController))]
    [RequireComponent(typeof(PlayerInput))]
    public class QuokkaInputHandler : MonoBehaviour
    {
        private QuokkaController controller;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;

        private void Awake()
        {
            controller = GetComponent<QuokkaController>();
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];

            jumpAction.started += OnJumpStarted;
            jumpAction.canceled += OnJumpCanceled;
        }

        private void OnDisable()
        {
            if (jumpAction != null)
            {
                jumpAction.started -= OnJumpStarted;
                jumpAction.canceled -= OnJumpCanceled;
            }
        }

        private void Update()
        {
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            controller.SetMoveInput(moveValue.x);
        }

        private void OnJumpStarted(InputAction.CallbackContext context)
        {
            controller.OnJumpPressed();
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            controller.OnJumpReleased();
        }
    }
}
