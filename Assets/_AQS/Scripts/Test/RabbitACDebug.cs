using UnityEngine;
using MalbersAnimations.Controller;

namespace AQS.Test
{
    /// <summary>
    /// Attach to a GameObject with MAnimal. Logs detailed AC state info focused on
    /// the Locomotion -> Idle handoff delay. Also runs a forced-idle isolation test.
    /// </summary>
    public class RabbitACDebug : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Log every frame while in stopping phase. Can be noisy.")]
        public bool logEveryFrame = false;

        [Tooltip("After MovementDetected goes false, wait this many seconds then force State 0 (Idle).")]
        public float forceIdleDelay = 0.1f;

        [Tooltip("Enable the forced-idle isolation test.")]
        public bool enableForceIdleTest = true;

        [Tooltip("Key to manually trigger Force Idle.")]
        public KeyCode forceIdleKey = KeyCode.F;

        private MAnimal _animal;
        private Animator _anim;
        private bool _wasMoving;
        private float _movStopTime = -1f;
        private bool _forceIdleFired;
        private int _lastStateID = -1;

        private void Awake()
        {
            _animal = GetComponent<MAnimal>();
            _anim = GetComponentInChildren<Animator>();

            if (_animal == null)
                Debug.LogError("[RabbitACDebug] No MAnimal found on " + gameObject.name);
        }

        private void Update()
        {
            if (_animal == null || _anim == null) return;

            bool movDet = _animal.MovementDetected;
            int stateID = _animal.ActiveState != null ? (_animal.ActiveState.ID != null ? _animal.ActiveState.ID.ID : -1) : -1;

            // Detect movement stop
            if (_wasMoving && !movDet)
            {
                _movStopTime = Time.time;
                _forceIdleFired = false;
                Debug.Log($"[{gameObject.name}] *** MOVEMENT STOPPED *** t=+0.000");
                LogDetailedState(0f);
            }
            _wasMoving = movDet;

            // Log during the stopping phase
            if (_movStopTime > 0f)
            {
                float elapsed = Time.time - _movStopTime;

                if (logEveryFrame)
                    LogDetailedState(elapsed);

                // Forced-idle isolation test
                if (enableForceIdleTest && !_forceIdleFired && elapsed >= forceIdleDelay)
                {
                    _forceIdleFired = true;
                    Debug.Log($"[{gameObject.name}] *** FORCING State_Force(0) at t=+{elapsed:F3}s ***");
                    _animal.State_Force(0);
                }

                // Stop tracking after 3 seconds
                if (elapsed > 3f)
                    _movStopTime = -1f;
            }

            // Log any state change
            if (stateID != _lastStateID)
            {
                float elapsed = _movStopTime > 0f ? Time.time - _movStopTime : 0f;
                Debug.Log($"[{gameObject.name}] STATE CHANGED: {_lastStateID} -> {stateID} | t=+{elapsed:F3}s");
                _lastStateID = stateID;

                // Log full state detail on every change
                if (_movStopTime > 0f)
                    LogDetailedState(elapsed);
            }

            // Manual force key
            if (Input.GetKeyDown(forceIdleKey))
            {
                Debug.Log($"[{gameObject.name}] Manual State_Force(0) via [{forceIdleKey}]");
                _animal.State_Force(0);
            }
        }

        private void LogDetailedState(float elapsed)
        {
            if (_animal == null || _anim == null) return;

            AnimatorStateInfo curInfo  = _anim.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = _anim.GetNextAnimatorStateInfo(0);
            bool animInTrans = _anim.IsInTransition(0);

            string curTag  = GetTag(curInfo);
            string nextTag = animInTrans ? GetTag(nextInfo) : "none";

            Vector3 mas = _animal.MovementAxisSmoothed;

            State  active     = _animal.ActiveState;
            int    activeID   = active != null && active.ID != null ? active.ID.ID : -1;
            string activeName = active != null ? active.name : "null";
            bool   canExit    = active != null && active.CanExit;
            bool   inCore     = active != null && active.InCoreAnimation;
            bool   isPending  = active != null && active.IsPending;

            int  modeID  = _animal.ActiveMode != null && _animal.ActiveMode.ID != null ? _animal.ActiveMode.ID.ID : -1;
            bool isMode  = _animal.IsPlayingMode;
            int  stance  = _animal.Stance;

            Debug.Log(
                $"[{gameObject.name}] t=+{elapsed:F3}s | " +
                $"ACState={activeID}({activeName}) | CanExit={canExit} | InCore={inCore} | Pending={isPending} | " +
                $"Grounded={_animal.Grounded} | MovDet={_animal.MovementDetected} | " +
                $"MAS=({mas.x:F3},{mas.y:F3},{mas.z:F3}) | " +
                $"ACInTrans={_animal.InTransition} | AnimInTrans={animInTrans} | " +
                $"AnimTag={curTag}({curInfo.normalizedTime % 1:F2})->{nextTag} | " +
                $"Mode={modeID} Playing={isMode} | Stance={stance}"
            );
        }

        private string GetTag(AnimatorStateInfo info)
        {
            if (info.IsTag("Idle"))       return "Idle";
            if (info.IsTag("Locomotion")) return "Locomotion";
            if (info.IsTag("Fall"))       return "Fall";
            return "OTHER";
        }
    }
}
