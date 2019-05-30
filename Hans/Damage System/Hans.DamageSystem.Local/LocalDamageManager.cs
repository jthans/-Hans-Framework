using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Hans.DamageSystem.Interfaces;
using Hans.DamageSystem.Models;

namespace Hans.DamageSystem.Local
{
    /// <summary>
    ///  Damage Manager that manages any damage tracking locally within this class.  We don't want to reach out to any external services here.  This is a local-focused
    ///     library, for game testing and possibly a full release if it runs well enough.
    /// </summary>
    [Export(typeof(IDamageDataManager))]
    public class LocalDamageManager : IDamageDataManager
    {
        #region Properties

        /// <summary>
        ///  Dictionary that tracks the health existing for a particular entity.
        /// </summary>
        private Dictionary<string, Dictionary<string, decimal>> _healthCollection;

        /// <summary>
        ///  Collection of layers that are stored per entity.
        /// </summary>
        private Dictionary<string, LayerMask[]> _layerCollection;

        #endregion

        #region Constructors

        public LocalDamageManager()
        {
            this._healthCollection = new Dictionary<string, Dictionary<string, decimal>>();
            this._layerCollection = new Dictionary<string, LayerMask[]>();
        }

        #endregion

        /// <summary>
        ///  Adds a layer to the storage.
        /// </summary>
        /// <param name="entityId">Entity that receives the layer.</param>
        /// <param name="layerMask">Layer to save to the entity.</param>
        public void AddLayer(string entityId, LayerMask layerMask)
        {
            if (!this._layerCollection.ContainsKey(entityId))
            {
                if (this._layerCollection[entityId] == null)
                {
                    this._layerCollection[entityId] = new LayerMask[0];
                }

                this._layerCollection[entityId] = this._layerCollection[entityId].Concat(new LayerMask[] { layerMask }).ToArray();
            }
        }

        /// <summary>
        ///  Applies the calculated damage unit to the entity desired.
        /// </summary>
        /// <param name="entityId">Which entity to apply this damage to.</param>
        /// <param name="damageToApply">The damage to apply.</param>
        /// <returns>The new values for damage.</returns>
        public Dictionary<string, decimal> ApplyDamage(string entityId, Dictionary<string, decimal> damageToApply)
        {
            var currDmg = this._healthCollection[entityId];
            foreach (var healthMod in damageToApply)
            {
                currDmg[healthMod.Key] = Math.Max(currDmg[healthMod.Key] - healthMod.Value, 0);
            }

            this._healthCollection[entityId] = currDmg;
            return currDmg;
        }

        /// <summary>
        ///  Creates a record for the entity, where we can track the health they have remaining.
        /// </summary>
        /// <param name="entityId">The entity to begin tracking.</param>
        /// <param name="startHealth">The health they start with.</param>
        public void BeginTrackingDamage(string entityId, decimal startHealth)
        {
            if (!this._healthCollection.ContainsKey(entityId))
            {
                this._healthCollection.Add(entityId, new Dictionary<string, decimal>() { { "BaseHealth", startHealth } });
            }
        }

        /// <summary>
        ///  Stops tracking information for a particular entity.
        /// </summary>
        /// <param name="entityId">Which entity we don't care about anymore.</param>
        public void EndTrackingDamage(string entityId)
        {
            if (this._healthCollection.ContainsKey(entityId))
            {
                this._healthCollection.Remove(entityId);
            }
        }

        /// <summary>
        ///  Returns the array of layer masks that exists for a particular entity.
        /// </summary>
        /// <param name="entityId">The entity to get layers for.</param>
        /// <returns>The list of layers currently present on entity.</returns>
        public LayerMask[] GetLayersForEntity(string entityId)
        {
            return this._layerCollection[entityId];
        }

        /// <summary>
        ///  Removes a layer from an entity, usually when it's been exhausted.
        /// </summary>
        /// <param name="entityId">The ID of the entity to remove the layer from.</param>
        /// <param name="layerId">Which layer we're removing.</param>
        public void RemoveLayer(string entityId, string layerId)
        {
            this._layerCollection[entityId] = this._layerCollection[entityId].Where(x => x.Id != layerId).ToArray();
        }

        /// <summary>
        ///  Updates a layer to the one passed to this method.
        /// </summary>
        /// <param name="entityId">The entity that gets the updated layer.</param>
        /// <param name="layerMask">The new layer mask.</param>
        public void UpdateLayer(string entityId, LayerMask layerMask)
        {
            this.RemoveLayer(entityId, layerMask.Id);
            this.AddLayer(entityId, layerMask);
        }
    }
}
