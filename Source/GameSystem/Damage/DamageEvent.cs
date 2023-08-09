namespace Source.GameSystem.Damage
{
    public enum DamageEvent
    {
        /// <summary>
        /// Once the damage is initialized
        /// </summary>
        DAMAGE,

        /// <summary>
        /// After Warcraft 3 engine armor calculation
        /// </summary>
        ARMOR,

        /// <summary>
        /// Before that damage is finalized (is dealt)
        /// </summary>
        DAMAGED,

        /// <summary>
        /// After the damage is dealt
        /// </summary>
        AFTER,

        /// <summary>
        /// On specific target that damage multiple times
        /// </summary>
        SOURCE,

        /// <summary>
        /// On damage that deal lethal blow!
        /// </summary>
        LETHAL
    }
}
