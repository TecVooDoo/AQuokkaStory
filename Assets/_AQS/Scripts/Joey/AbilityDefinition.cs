using UnityEngine;

namespace AQS.Joey
{
    /// <summary>
    /// Data asset defining a single Joey ability. Referenced by JoeyDefinition.
    /// Create via Assets > Create > AQS > Joey > Ability Definition.
    /// </summary>
    [CreateAssetMenu(fileName = "New AbilityDefinition", menuName = "AQS/Joey/Ability Definition")]
    public class AbilityDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string abilityName;
        [SerializeField] [TextArea(2, 4)] private string description;

        [Header("Cost")]
        [Tooltip("Energy consumed per use (GDD range: 25-75)")]
        [SerializeField] [Range(0f, 100f)] private float energyCost = 25f;

        [Tooltip("Seconds before the ability can be used again")]
        [SerializeField] [Min(0f)] private float cooldown;

        [Header("Damage")]
        [SerializeField] private float baseDamage;
        [SerializeField] private float aoERadius;

        [Header("Physics")]
        [Tooltip("Override launch force for this ability (0 = use launcher default)")]
        [SerializeField] private float launchForceOverride;

        [Tooltip("Gravity scale during flight (1 = normal arc, <1 = floaty)")]
        [SerializeField] [Min(0f)] private float gravityScale = 1f;

        // --- Public API (read-only) ---
        public string AbilityName => abilityName;
        public string Description => description;
        public float EnergyCost => energyCost;
        public float Cooldown => cooldown;
        public float BaseDamage => baseDamage;
        public float AoERadius => aoERadius;
        public float LaunchForceOverride => launchForceOverride;
        public float GravityScale => gravityScale;
    }
}
