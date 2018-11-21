using Hans.DamageSystem.Interfaces;
using Hans.DamageSystem.Models;
using Hans.DependencyInjection;
using Hans.Logging;
using Hans.Logging.Interfaces;
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
    public class DamageController<T> : MEFObject
        where T : DamageUnit
    {
        #region Fields
        
        /// <summary>
        ///  The damage manager, that will allow us to communicate with the damage cache/storage.
        /// </summary>
        [Import(typeof(IDamageDataManager))]
        private IDamageDataManager damageManager;

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

        public DamageUnit ApplyDamage(string entityId, DamageUnit damageAmt)
        {
            // Get a standard dictionary from the DamageUnit.
            var normalizedDamage = this.damageMapper.Normalize(damageAmt);
            
            // We need to track each damage type, and see if any modifiers need to occur based on current entity statistics.
            foreach (var damageType in normalizedDamage.Keys)
            {
                // We have a custom calculator for this damage type.
                if (this.damageTypeCalculators.ContainsKey(damageType))
                {
                    normalizedDamage = this.damageTypeCalculators[damageType].CalculateDamageTypeEffect(normalizedDamage);
                }
            }

            // Apply the damage to the entity.
            var remainingDamage = this.damageManager.ApplyDamage(entityId, normalizedDamage);
            return this.damageMapper.TranslateToModel(remainingDamage); // TODO : FINISH
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
