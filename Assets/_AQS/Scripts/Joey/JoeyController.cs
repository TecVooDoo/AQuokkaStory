using UnityEngine;
using AQS.Core;

namespace AQS.Joey
{
    /// <summary>
    /// Runtime controller for a single Joey instance.
    /// Manages state transitions and energy. Does NOT handle movement or
    /// decision-making -- that's JoeyBrain + JoeyGroundFollower.
    ///
    /// Lifecycle:
    ///   FollowingInLine -> (pouch) -> InPouch -> Aiming -> Launched -> (auto-recall) -> FollowingInLine/Depleted
    ///   Depleted -> (energy recovers) -> FollowingInLine
    /// </summary>
    public sealed class JoeyController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private JoeyDefinition definition;

        [Header("Events")]
        [SerializeField] private GameEvent onJoeyLaunched;
        [SerializeField] private GameEvent onJoeyRecalled;
        [SerializeField] private GameEventFloat onEnergyChanged;

        private JoeyState currentState;
        private JoeyEnergy energy;
        private float cooldownTimer;

        public JoeyDefinition Definition => definition;
        public JoeyState CurrentState => currentState;
        public JoeyEnergy Energy => energy;
        public bool IsOnCooldown => cooldownTimer > 0f;

        private void Awake()
        {
            if (definition != null)
            {
                energy = new JoeyEnergy(definition);
                // Scene-placed Joeys start following, not in pouch
                currentState = JoeyState.FollowingInLine;
            }
        }

        private void Update()
        {
            if (energy == null) return;

            energy.Tick(Time.deltaTime, currentState);

            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;

            if (currentState == JoeyState.Depleted && HasEnoughEnergy())
            {
                TransitionTo(JoeyState.FollowingInLine);
            }
        }

        /// <summary>
        /// Initialize this Joey with a definition. Called by PouchManager on spawn.
        /// </summary>
        public void Initialize(JoeyDefinition joeyDef)
        {
            definition = joeyDef;
            energy = new JoeyEnergy(definition);
            currentState = JoeyState.FollowingInLine;
        }

        /// <summary>Transition to InPouch state. Called by JoeyBrain.OnPouched.</summary>
        public void EnterPouch()
        {
            TransitionTo(JoeyState.InPouch);
        }

        /// <summary>Transition out of pouch. Called by JoeyBrain.OnUnpouched.</summary>
        public void ExitPouch()
        {
            if (currentState == JoeyState.InPouch)
            {
                TransitionTo(JoeyState.FollowingInLine);
            }
        }

        public bool TryStartAiming()
        {
            if (currentState != JoeyState.InPouch) return false;
            if (IsOnCooldown) return false;
            if (!HasEnoughEnergy()) return false;

            TransitionTo(JoeyState.Aiming);
            return true;
        }

        public void CancelAiming()
        {
            if (currentState != JoeyState.Aiming) return;
            TransitionTo(JoeyState.InPouch);
        }

        public bool TryLaunch()
        {
            if (currentState != JoeyState.Aiming) return false;

            AbilityDefinition ability = definition.Ability;
            if (ability == null) return false;

            if (!energy.TrySpend(ability.EnergyCost))
            {
                TransitionTo(JoeyState.InPouch);
                return false;
            }

            cooldownTimer = ability.Cooldown;
            TransitionTo(JoeyState.Launched);

            if (onJoeyLaunched != null)
                onJoeyLaunched.Raise();

            if (onEnergyChanged != null)
                onEnergyChanged.Raise(energy.NormalizedEnergy);

            return true;
        }

        /// <summary>
        /// Auto-recall after ability. Goes to FollowingInLine or Depleted.
        /// Called by JoeyBrain.OnRecalled.
        /// </summary>
        public void Recall()
        {
            if (currentState != JoeyState.Launched) return;

            JoeyState returnState = HasEnoughEnergy()
                ? JoeyState.FollowingInLine
                : JoeyState.Depleted;

            TransitionTo(returnState);

            if (onJoeyRecalled != null)
                onJoeyRecalled.Raise();
        }

        private void TransitionTo(JoeyState newState)
        {
            if (currentState == newState) return;
            currentState = newState;
        }

        private bool HasEnoughEnergy()
        {
            AbilityDefinition ability = definition.Ability;
            if (ability == null) return true;
            return energy.CurrentEnergy >= ability.EnergyCost;
        }
    }
}
