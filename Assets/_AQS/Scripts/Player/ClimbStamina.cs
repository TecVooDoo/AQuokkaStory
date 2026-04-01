using UnityEngine;
using MalbersAnimations;
using MalbersAnimations.Controller;

namespace AQS.Player
{
    /// <summary>
    /// Drains Stamina while climbing. When Stamina is empty, forces the animal
    /// out of the Climb state (falls). Regeneration resumes automatically on exit.
    /// </summary>
    [RequireComponent(typeof(MAnimal))]
    public sealed class ClimbStamina : MonoBehaviour
    {
        [Header("Config")]
        [Tooltip("Stat ID for Stamina (must match the Stats component)")]
        [SerializeField] private StatID staminaID;

        [Tooltip("State ID for Climb (must match the MAnimal state list)")]
        [SerializeField] private StateID climbStateID;

        private MAnimal animal;
        private Stats stats;
        private Stat staminaStat;
        private bool isClimbing;

        private void Awake()
        {
            animal = GetComponent<MAnimal>();
            stats = GetComponentInChildren<Stats>();
        }

        private void OnEnable()
        {
            animal.OnStateChange.AddListener(OnStateChanged);

            if (stats != null && staminaID != null)
            {
                staminaStat = stats.Stat_Get(staminaID);
                if (staminaStat != null)
                {
                    staminaStat.OnStatEmpty.AddListener(OnStaminaEmpty);
                }
            }
        }

        private void OnDisable()
        {
            animal.OnStateChange.RemoveListener(OnStateChanged);

            if (staminaStat != null)
            {
                staminaStat.OnStatEmpty.RemoveListener(OnStaminaEmpty);
            }
        }

        private void OnStateChanged(int newStateID)
        {
            if (staminaStat == null || climbStateID == null) return;

            bool nowClimbing = newStateID == climbStateID.ID;

            if (nowClimbing && !isClimbing)
            {
                // Entered Climb -- start draining stamina
                staminaStat.Degenerate = true;
                isClimbing = true;
            }
            else if (!nowClimbing && isClimbing)
            {
                // Exited Climb -- stop drain, regen resumes via Stat property
                staminaStat.Degenerate = false;
                isClimbing = false;
            }
        }

        private void OnStaminaEmpty()
        {
            if (!isClimbing) return;

            // Force exit Climb state -- animal will Fall
            State climbState = animal.State_Get(climbStateID);
            if (climbState != null && climbState.IsActiveState)
            {
                climbState.AllowExit();
            }
        }
    }
}
