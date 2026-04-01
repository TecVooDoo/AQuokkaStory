using UnityEngine;
using UnityEngine.InputSystem;
using MalbersAnimations.Controller;
using MalbersAnimations;

namespace AQS.Test
{
    /// <summary>
    /// Attach to Snake_AC_Test. Logs raw keyboard state, PlayerInput action state,
    /// and what MAnimal actually receives. Press Tab to dump a full snapshot.
    /// </summary>
    public class SnakeInputDebug : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Log every frame that has any input. Can be noisy.")]
        public bool logContinuous = false;

        [Tooltip("Key to dump a full input snapshot on demand.")]
        public KeyCode dumpKey = KeyCode.Tab;

        private MAnimal _animal;
        private PlayerInput _playerInput;
        private int _lastStateId = -99;
        private float _startTime;

        private void Awake()
        {
            _animal = GetComponent<MAnimal>();
            _playerInput = GetComponent<PlayerInput>();

            if (_animal == null)
                Debug.LogError("[SnakeInputDebug] No MAnimal on " + gameObject.name);
            if (_playerInput == null)
                Debug.LogWarning("[SnakeInputDebug] No PlayerInput on " + gameObject.name);
            else
                Debug.Log($"[SnakeInputDebug] PlayerInput found. ActionMap={_playerInput.currentActionMap?.name ?? "null"} | DefaultMap={_playerInput.defaultActionMap}");

            _startTime = Time.time;
        }

        private void Update()
        {
            bool wDown = Keyboard.current != null && Keyboard.current.wKey.isPressed;
            bool sDown = Keyboard.current != null && Keyboard.current.sKey.isPressed;
            bool aDown = Keyboard.current != null && Keyboard.current.aKey.isPressed;
            bool dDown = Keyboard.current != null && Keyboard.current.dKey.isPressed;
            bool upDown   = Keyboard.current != null && Keyboard.current.upArrowKey.isPressed;
            bool downDown = Keyboard.current != null && Keyboard.current.downArrowKey.isPressed;

            bool anyKeyNew = wDown || sDown || aDown || dDown || upDown || downDown;

            // Also check old input system horizontal/vertical axes
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool anyKeyOld = Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f;

            if (logContinuous && (anyKeyNew || anyKeyOld))
                DumpSnapshot("CONTINUOUS");

            if (Input.GetKeyDown(dumpKey))
                DumpSnapshot("MANUAL DUMP");

            // Log state changes
            if (_animal != null)
            {
                int currentId = _animal.ActiveState != null ? (_animal.ActiveState.ID != null ? _animal.ActiveState.ID.ID : -1) : -1;
                if (currentId != _lastStateId)
                {
                    float t = Time.time - _startTime;
                    Debug.Log($"[SnakeInputDebug] STATE CHANGED: {_lastStateId} -> {currentId} ({_animal.ActiveState?.name}) | t=+{t:F3}s | Grounded={_animal.Grounded}");
                    _lastStateId = currentId;
                }
            }
        }

        private void DumpSnapshot(string label)
        {
            // --- Raw keyboard (new Input System) ---
            bool kb = Keyboard.current != null;
            string keys = kb
                ? $"W={Keyboard.current.wKey.isPressed} S={Keyboard.current.sKey.isPressed} A={Keyboard.current.aKey.isPressed} D={Keyboard.current.dKey.isPressed} Up={Keyboard.current.upArrowKey.isPressed} Dn={Keyboard.current.downArrowKey.isPressed}"
                : "Keyboard.current=null";

            // --- Old Input System axes ---
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // --- PlayerInput ---
            string piInfo = "none";
            if (_playerInput != null)
            {
                string mapName = _playerInput.currentActionMap?.name ?? "null";
                InputAction moveAction = _playerInput.currentActionMap?.FindAction("Move");
                string moveVal = moveAction != null
                    ? moveAction.ReadValue<Vector2>().ToString("F2")
                    : "no Move action";
                piInfo = $"map={mapName} | Move={moveVal} | enabled={_playerInput.inputIsActive}";
            }

            // --- MAnimal ---
            string animalInfo = "none";
            string gateInfo = "none";
            if (_animal != null)
            {
                Vector3 mas = _animal.MovementAxisSmoothed;
                Vector3 mar = _animal.MovementAxis;
                State activeState = _animal.ActiveState;
                string stateName = activeState != null ? $"{activeState.name}[{activeState.ID?.ID}]" : "null";
                animalInfo = $"State={stateName} | MovDet={_animal.MovementDetected} | Axis=({mar.x:F3},{mar.y:F3},{mar.z:F3}) | AxisSmoothed=({mas.x:F3},{mas.y:F3},{mas.z:F3}) | Grounded={_animal.Grounded}";

                // Gate check: why can't Locomotion activate?
                if (activeState != null)
                {
                    gateInfo = $"ActiveState.CanExit={activeState.CanExit} | IsPending={activeState.IsPending} | IsPersistent={activeState.IsPersistent} | IgnoreLower={activeState.IgnoreLowerStates}";
                }

                // Animator layer 0 state info
                Animator anim = GetComponent<Animator>();
                if (anim != null)
                {
                    AnimatorStateInfo current = anim.GetCurrentAnimatorStateInfo(0);
                    AnimatorStateInfo next = anim.GetNextAnimatorStateInfo(0);
                    bool inTransition = anim.IsInTransition(0);
                    int curTagHash = current.tagHash;
                    int nextTagHash = next.tagHash;
                    int idleHash = Animator.StringToHash("Idle");
                    int locoHash = Animator.StringToHash("Locomotion");
                    int fallHash = Animator.StringToHash("Fall");

                    string curTag = curTagHash == idleHash ? "Idle" : curTagHash == locoHash ? "Locomotion" : curTagHash == fallHash ? "Fall" : curTagHash.ToString();
                    string nxtTag = nextTagHash == 0 ? "none" : nextTagHash == idleHash ? "Idle" : nextTagHash == locoHash ? "Locomotion" : nextTagHash == fallHash ? "Fall" : nextTagHash.ToString();

                    gateInfo += $"\n  Animator: curTag={curTag} | nextTag={nxtTag} | inTransition={inTransition} | curHash={current.fullPathHash} | nextHash={next.fullPathHash}";
                    // Check if MAnimal's Animator matches ours
                    Animator malbersAnim = null;
                    System.Reflection.FieldInfo animField = _animal.GetType().GetField("Anim", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (animField == null)
                    {
                        System.Reflection.PropertyInfo animProp = _animal.GetType().GetProperty("Anim", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (animProp != null) malbersAnim = animProp.GetValue(_animal) as Animator;
                    }
                    else
                    {
                        malbersAnim = animField.GetValue(_animal) as Animator;
                    }
                    string animCompare = malbersAnim != null
                        ? $"MalbersAnimID={malbersAnim.GetInstanceID()} MyAnimID={anim.GetInstanceID()} match={malbersAnim.GetInstanceID() == anim.GetInstanceID()}"
                        : "MalbersAnim=NULL";
                    gateInfo += $"\n  AnimStateTag={_animal.currentAnimTag} | IdleHash={idleHash} | Sleep={_animal.Sleep} | {animCompare}";
                }
            }

            Debug.Log(
                $"[SnakeInputDebug] {label}\n" +
                $"  NewInputSys keys: {keys}\n" +
                $"  OldInputSys:      H={h:F3} V={v:F3}\n" +
                $"  PlayerInput:      {piInfo}\n" +
                $"  MAnimal:          {animalInfo}\n" +
                $"  Gates:            {gateInfo}"
            );
        }
    }
}
