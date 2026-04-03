using UnityEngine;
using AQS.Core;

namespace AQS.Joey
{
    /// <summary>
    /// Manages a single Joey's energy pool. Regen rate varies by JoeyState.
    /// Energy level drives instrument stem volume (0-100%).
    /// </summary>
    public sealed class JoeyEnergy
    {
        private readonly JoeyDefinition definition;

        private float currentEnergy;
        private float regenMultiplier = 1f;

        public float CurrentEnergy => currentEnergy;
        public float MaxEnergy => definition.MaxEnergy;
        public float NormalizedEnergy => definition.MaxEnergy > 0f ? currentEnergy / definition.MaxEnergy : 0f;

        public JoeyEnergy(JoeyDefinition definition)
        {
            this.definition = definition;
            currentEnergy = definition.MaxEnergy;
        }

        /// <summary>
        /// External multiplier applied to regen (e.g. Ninja drawback reduces other Joeys' regen).
        /// </summary>
        public void SetRegenMultiplier(float multiplier)
        {
            regenMultiplier = Mathf.Max(0f, multiplier);
        }

        /// <summary>
        /// Tick energy regen/drain based on current state. Call from Update.
        /// </summary>
        public void Tick(float deltaTime, JoeyState state)
        {
            float regenRate = state switch
            {
                JoeyState.InPouch => definition.InPouchRegenRate,
                JoeyState.FollowingInLine => definition.OutOfPouchRegenRate,
                JoeyState.Launched => 0f,
                JoeyState.Depleted => definition.OutOfPouchRegenRate,
                _ => 0f
            };

            currentEnergy += regenRate * regenMultiplier * deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0f, definition.MaxEnergy);
        }

        /// <summary>
        /// Try to spend energy for an ability. Returns false if not enough.
        /// </summary>
        public bool TrySpend(float amount)
        {
            if (currentEnergy < amount)
                return false;

            currentEnergy -= amount;
            return true;
        }

        /// <summary>
        /// Force energy to a specific value (e.g. on rescue, reset).
        /// </summary>
        public void SetEnergy(float value)
        {
            currentEnergy = Mathf.Clamp(value, 0f, definition.MaxEnergy);
        }
    }
}
