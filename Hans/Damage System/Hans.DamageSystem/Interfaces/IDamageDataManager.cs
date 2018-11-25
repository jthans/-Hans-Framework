using Hans.DamageSystem.Models;
using System.Collections.Generic;

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
        ///  Adds a layer mask to a given entity.
        /// </summary>
        /// <param name="entityId">The entity to add this layer mask to.</param>
        /// <param name="layerMask">The layer mask details that will be added.</param>
        void AddLayer(string entityId, LayerMask layerMask);

        /// <summary>
        ///  Applies damage (or healing, if the numbers are positive) to an entity.
        /// </summary>
        /// <param name="entityId">The entity this affects.</param>
        /// <param name="damageToApply">The damage we're applying to the entity.</param>
        /// <returns>The new damage values for the entity.</returns>
        Dictionary<string, decimal> ApplyDamage(string entityId, Dictionary<string, decimal> damageToApply);

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

        /// <summary>
        ///  Gets all layers for a particular entity, to be calculated against.
        /// </summary>
        /// <param name="entityId">The entity that we're grabbing layers for.</param>
        /// <returns>The layers existing on a particular entity.</returns>
        LayerMask[] GetLayersForEntity(string entityId);

        /// <summary>
        ///  Removes a particular layer from the entity requested.
        /// </summary>
        /// <param name="entityId">The entity to remove a layer from.</param>
        /// <param name="layerId">The layer to remove.</param>
        void RemoveLayer(string entityId, string layerId);

        /// <summary>
        ///  Updates a given layer in the manager, with the new values.
        /// </summary>
        /// <param name="entityId">On which entity to update the layer.</param>
        /// <param name="layerMask">The layer configuration. to be updated.</param>
        void UpdateLayer(string entityId, LayerMask layerMask);
    }
}
