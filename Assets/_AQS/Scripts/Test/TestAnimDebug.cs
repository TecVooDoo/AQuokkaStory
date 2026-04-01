using UnityEngine;
using UnityEngine.InputSystem;

namespace AQS.Test
{
    public class TestAnimDebug : MonoBehaviour
    {
        private Animator animator;
        private MalbersAnimations.Controller.MAnimal animal;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private int lastStateHash;
        private bool wasInputActive;
        private float releaseTime;
        private bool trackAfterRelease;

        private void Start()
        {
            animator = GetComponent<Animator>();
            animal = GetComponent<MalbersAnimations.Controller.MAnimal>();
            playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
                moveAction = playerInput.actions["Move"];
            Debug.Log("AnimDebug v4: Tracking animator clip + AC state after release.");
        }

        private void Update()
        {
            Vector2 inputVal = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
            bool inputActive = inputVal.sqrMagnitude > 0.01f;

            if (inputActive && !wasInputActive)
            {
                Debug.Log("AnimDebug: === INPUT START ===");
                trackAfterRelease = false;
            }

            if (!inputActive && wasInputActive)
            {
                Debug.Log("AnimDebug: === INPUT STOP ===");
                releaseTime = Time.time;
                trackAfterRelease = true;
            }

            wasInputActive = inputActive;

            // After release, log every frame for 2 seconds
            if (trackAfterRelease && (Time.time - releaseTime) < 2f)
            {
                AnimatorStateInfo layer0 = animator.GetCurrentAnimatorStateInfo(0);
                AnimatorStateInfo layer1 = animator.GetCurrentAnimatorStateInfo(1);

                string tag0 = "?";
                if (layer0.IsTag("Idle")) tag0 = "Idle";
                else if (layer0.IsTag("Locomotion")) tag0 = "Locomotion";
                else if (layer0.IsTag("Fall")) tag0 = "Fall";
                else if (layer0.IsTag("Jump")) tag0 = "Jump";

                string tag1 = "?";
                if (layer1.IsTag("Empty")) tag1 = "Empty";
                else if (layer1.IsTag("Null")) tag1 = "Null";

                bool inTransition0 = animator.IsInTransition(0);
                bool inTransition1 = animator.IsInTransition(1);

                // AC internal
                int acStateID = animal.ActiveStateID != null ? animal.ActiveStateID.ID : -1;
                bool movDetected = animal.MovementDetected;

                // Animator parameters
                int stateParam = animator.GetInteger("State");
                bool movParam = animator.GetBool("Movement");
                float vertParam = animator.GetFloat("Vertical");

                // Log every 5 frames
                if (Time.frameCount % 5 == 0)
                {
                    float dt = Time.time - releaseTime;
                    Debug.Log("AnimDebug: [+" + dt.ToString("F3") + "s] " +
                             "L0=" + tag0 + "(nTime=" + layer0.normalizedTime.ToString("F2") + " trans=" + inTransition0 + ") " +
                             "L1=" + tag1 + "(trans=" + inTransition1 + ") " +
                             "ACState=" + acStateID + " " +
                             "MovDet=" + movDetected + " " +
                             "Params: State=" + stateParam + " Mov=" + movParam + " Vert=" + vertParam.ToString("F3"));
                }
            }

            // Track state changes
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            int currentHash = stateInfo.shortNameHash;
            if (currentHash != lastStateHash)
            {
                string stateName = "?";
                if (stateInfo.IsTag("Idle")) stateName = "Idle";
                else if (stateInfo.IsTag("Locomotion")) stateName = "Locomotion";
                else if (stateInfo.IsTag("Fall")) stateName = "Fall";
                else if (stateInfo.IsTag("Jump")) stateName = "Jump";

                Debug.Log("AnimDebug: STATE CHANGE -> " + stateName +
                         " nTime=" + stateInfo.normalizedTime.ToString("F2") +
                         " ACState=" + (animal.ActiveStateID != null ? animal.ActiveStateID.ID.ToString() : "?"));
                lastStateHash = currentHash;
            }
        }
    }
}
