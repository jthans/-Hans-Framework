using Hans.DamageSystem.Enums;
using Hans.DamageSystem.Interfaces;
using Hans.DamageSystem.Models;
using Hans.DependencyInjection;
using Hans.Logging;
using Hans.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Hans.DamageSystem
{
    /// <summary>
    ///  Class that acts as the "Control Center" for anything damage related.  This will act as a mediator to various backing services, as
    ///     others are written to keep the data stored elsewhere for various reasons.
    /// </summary>
    /// <typeparam name="T">Damage model, used to encourage multiple types to be represented.</typeparam>
    public class DamageController<T> : MEFObject, IDamageController<T>
        where T : DamageUnit
    {
        #region Fields
        
        /// <summary>
        ///  The damage manager, that will allow us to communicate with the damage cache/storage.
        ///     NOTE: Public for Testing Purposes.
        /// </summary>
        [Import(typeof(IDamageDataManager))]
        public IDamageDataManager DamageManager;

        /// <summary>
        ///  Dictionary of damage type calculations, tied to their types.  If any extra calculations need to be done on a value, it's done here.
        /// </summary>
        private Dictionary<string, IDamageTypeCalculator> damageTypeCalculators;

        /// <summary>
        ///  The damage mapper, that will convert a generic model to arrays, and vice versa for us.
        /// </summary>
        private IDamageMapper<T> damageMapper;

        /// <summary>
        ///  Logger that is used to output any useful information happening here.
        /// </summary>
        private readonly ILogger log = LoggerManager.CreateLogger(typeof(DamageController<T>));

        #endregion

        /// <summary>
        ///  Initializes a new instance of the <see cref="DamageController" /> class.  
        /// </summary>
        public DamageController()
        {
            this.damageMapper = new DamageMapper<T>();
            this.ResolveDependencies();
        }

        #region Instance Methods

        /// <summary>
        ///  Adds a layer of absorption/enhancement for damage types to an entity, to be considered when applying damage to an entity.
        /// </summary>
        /// <param name="entityId">The entity that this layer will be applied to.</param>
        /// <param name="damageType">The type of damage that is being managed in this layer.</param>
        /// <param name="layerValue">How much this layer manages up to.</param>
        /// <param name="layerMask">
        ///     The layer mask, or proportion of damage that is affected by this addition.
        ///         For example, if this is 0.5, 50% of this damage type will be taken up to a certain value.  If 2.0, damage will be doubled for that type.
        /// </param>
        /// <param name="layerDuration">How long this layer exists on the entity.</param>
        public string AddLayer(string entityId, string damageType, decimal? layerValue, decimal? layerMask, decimal? layerDuration)
        {
            // Adds the layer to the damage manager.
            var entityLayer = new LayerMask(damageType, layerValue, layerDuration, layerMask);
            this.DamageManager.AddLayer(entityId, entityLayer);

            return entityLayer.Id;
        }

        /// <summary>
        ///  Calculates the elemental/damage type effects on the entity requested, taking special types and
        ///     shield/weakness layers into effect.  Passes the application that needs to occur to the manager.
        /// </summary>
        /// <param name="entityId">Entity that is receiving the damage.</param>
        /// <param name="damageAmt">The damage unit that's going to be applied to the character.</param>
        /// <returns></returns>
        public DamageUnit ApplyDamage(string entityId, DamageUnit damageAmt)
        {
            // Get a standard dictionary from the DamageUnit.
            var normalizedDamage = this.damageMapper.Normalize(damageAmt);

            // We need to track each damage type, and see if any modifiers need to occur based on current entity statistics.
            this.CalculateLayerEffects(entityId, ref normalizedDamage);
            this.CalculateDamageTypeEffects(ref normalizedDamage);

            // Apply the damage to the entity.
            var remainingDamage = this.DamageManager.ApplyDamage(entityId, normalizedDamage);
            return this.damageMapper.TranslateToModel(remainingDamage);
        }

        /// <summary>
        ///  Calculates a new damage matrix, based on the damage type modifiers determined in the calculations.
        /// </summary>
        /// <param name="inputDmg">The damage going into the effect calculations.</param>
        /// <returns>The number of effect calculations run.</returns>
        public int CalculateDamageTypeEffects(ref Dictionary<string, decimal> inputDmg)
        {
            var damageKeys = inputDmg.Keys.ToList();
            int typeEffects = 0;

            foreach (var damageType in damageKeys)
            {
                // We have a custom calculator for this damage type.
                if (this.damageTypeCalculators.ContainsKey(damageType))
                {
                    inputDmg = this.damageTypeCalculators[damageType].CalculateDamageTypeEffect(inputDmg);
                    typeEffects++;
                }
            }

            return typeEffects;
        }

        /// <summary>
        ///  Calculates the layer effects for a particular entity.
        /// </summary>
        /// <param name="inputDmg">The damage into the layer calculations.</param>
        /// <returns>The number of layer calculations that ran.</returns>
        public int CalculateLayerEffects(string entityId, ref Dictionary<string, decimal> inputDmg)
        {
            // Get all layers on thie entity, and iterate over them, calculating effects.
            var layersToIterate = this.DamageManager.GetLayersForEntity(entityId);
            foreach (var layer in layersToIterate)
            {
                decimal calcAmt = 0;
                decimal layerDmgAmt = Math.Min(inputDmg[layer.DamageType], layer.DamageCap);

                // Calculate the multiplied damage amount.
                if (layer.Multiplier != 0)
                {
                    calcAmt = layerDmgAmt * layer.Multiplier;
                }

                inputDmg[layer.DamageType] = inputDmg[layer.DamageType] - (layerDmgAmt - calcAmt);
                if (layer.DamageCap != 0)
                {
                    // Subtract the amount of damage handled from the cap, and remove it if it's negative or zero after.
                    layer.DamageCap -= layerDmgAmt;
                    if (layer.DamageCap <= 0)
                    {
                        this.DamageManager.RemoveLayer(entityId, layer.Id);
                    }
                    else
                    {
                        this.DamageManager.UpdateLayer(entityId, layer);
                    }
                }
            }

            return layersToIterate.Length;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  Resolves any dependencies needed for this class.
        /// </summary>
        private void ResolveDependencies()
        {
            this.log.LogMessage($"Resolving dependencies for the damage system.");
            this.damageTypeCalculators = MEFBootstrapper.ResolveManyWithMetaData<IDamageTypeCalculator, IDamageTypeMetadata>()
                                                        .ToDictionary(x => x.Metadata.DamageType, x => x.Value);
        }

        #endregion
    }
}
