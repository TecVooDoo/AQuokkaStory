using UnityEngine;

namespace AQS.Joey
{
    /// <summary>
    /// Data asset defining a Joey archetype. One per Joey in the roster.
    /// Create via Assets > Create > AQS > Joey > Joey Definition.
    /// </summary>
    [CreateAssetMenu(fileName = "New JoeyDefinition", menuName = "AQS/Joey/Joey Definition")]
    public class JoeyDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string joeyName;
        [SerializeField] private JoeyRole role;
        [SerializeField] [TextArea(2, 4)] private string description;
        [SerializeField] private Sprite portrait;

        [Header("Ability")]
        [SerializeField] private AbilityDefinition ability;

        [Header("Energy")]
        [Tooltip("Max energy pool (GDD default: 100, upgradeable +25 x3)")]
        [SerializeField] private float maxEnergy = 100f;

        [Tooltip("Energy regen per second while in pouch (GDD: 30)")]
        [SerializeField] private float inPouchRegenRate = 30f;

        [Tooltip("Energy regen per second while out of pouch (GDD: 10)")]
        [SerializeField] private float outOfPouchRegenRate = 10f;

        [Header("Passive Buff on Mom")]
        [Tooltip("Multiplier applied to Mom's move speed (1.0 = no change)")]
        [SerializeField] private float moveSpeedMultiplier = 1f;

        [Tooltip("Multiplier applied to Mom's jump height (1.0 = no change)")]
        [SerializeField] private float jumpHeightMultiplier = 1f;

        [Tooltip("Multiplier applied to Mom's defense (1.0 = no change)")]
        [SerializeField] private float defenseMultiplier = 1f;

        [Tooltip("Multiplier applied to Mom's fall speed (1.0 = no change)")]
        [SerializeField] private float fallSpeedMultiplier = 1f;

        [Header("Drawback")]
        [SerializeField] [TextArea(1, 3)] private string drawbackDescription;

        [Tooltip("Multiplier applied to damage Mom takes (1.0 = normal, 2.0 = double)")]
        [SerializeField] private float damageTakenMultiplier = 1f;

        [Tooltip("Multiplier applied to other Joeys' regen rate (1.0 = normal)")]
        [SerializeField] private float otherJoeyRegenMultiplier = 1f;

        [Header("Audio")]
        [Tooltip("Master Audio group name for this Joey's instrument stem")]
        [SerializeField] private string instrumentGroupName;

        [Header("Visuals")]
        [Tooltip("Prefab to instantiate for this Joey (raccoon cub prototype)")]
        [SerializeField] private GameObject joeyPrefab;

        // --- Public API (read-only) ---
        public string JoeyName => joeyName;
        public JoeyRole Role => role;
        public string Description => description;
        public Sprite Portrait => portrait;
        public AbilityDefinition Ability => ability;

        public float MaxEnergy => maxEnergy;
        public float InPouchRegenRate => inPouchRegenRate;
        public float OutOfPouchRegenRate => outOfPouchRegenRate;

        public float MoveSpeedMultiplier => moveSpeedMultiplier;
        public float JumpHeightMultiplier => jumpHeightMultiplier;
        public float DefenseMultiplier => defenseMultiplier;
        public float FallSpeedMultiplier => fallSpeedMultiplier;

        public string DrawbackDescription => drawbackDescription;
        public float DamageTakenMultiplier => damageTakenMultiplier;
        public float OtherJoeyRegenMultiplier => otherJoeyRegenMultiplier;

        public string InstrumentGroupName => instrumentGroupName;
        public GameObject JoeyPrefab => joeyPrefab;
    }
}
