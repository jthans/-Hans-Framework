namespace Hans.DamageSystem.Interfaces
{
    /// <summary>
    ///  Any class inheriting this interface can be responsible for managing damage storage/modification and returning
    ///     requested results to the <see cref="DamageController" />  This is CACHE-FOCUSED, meaning only simple cache focused
    ///     items will be handled with this interface.  Anything else should be done outside of this class.
    /// </summary>
    public interface IDamageDataManager
    {
        /// <summary>
        ///  Begins tracking damage/effects on the entity requested.
        /// </summary>
        /// <param name="entityId">The entity we're going to track.</param>
        /// <param name="startHealth"></param>
        void BeginTrackingDamage(string entityId, decimal startHealth);

        /// <summary>
        ///  Stops tracking damage for an entity - saves calculations in the cache if it's not needed.
        /// </summary>
        /// <param name="entityId">The entity to stop tracking damage for.</param>
        void EndTrackingDamage(string entityId);
    }
}
