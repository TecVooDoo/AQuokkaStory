namespace AQS.Joey
{
    /// <summary>
    /// Runtime state of a Joey instance.
    /// </summary>
    public enum JoeyState
    {
        /// <summary>Walking behind Mom in the follow line. Not launchable. Default state.</summary>
        FollowingInLine,
        /// <summary>In Mom's pouch, ready to aim/launch. Not visible in follow line.</summary>
        InPouch,
        /// <summary>Player is holding aim input, trajectory showing.</summary>
        Aiming,
        /// <summary>Airborne / executing ability.</summary>
        Launched,
        /// <summary>Recovering energy in the follow line after recall.</summary>
        Depleted
    }

    /// <summary>
    /// Which Joey archetype this is. Maps to GDD roster.
    /// </summary>
    public enum JoeyRole
    {
        Normal,
        Lead,
        Ballet,
        Ninja,
        Helium,
        GI,
        Kazoo
    }
}
