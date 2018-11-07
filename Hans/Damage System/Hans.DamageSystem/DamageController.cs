using Hans.DamageSystem.Interfaces;
using Hans.DamageSystem.Models;
using Hans.DependencyInjection;
using Hans.Logging;
using Hans.Logging.Interfaces;

using System.ComponentModel.Composition;

namespace Hans.DamageSystem
{
    /// <summary>
    ///  Class that acts as the "Control Center" for anything damage related.  This will act as a mediator to various backing services, as
    ///     others are written to keep the data stored elsewhere for various reasons.
    /// </summary>
    /// <typeparam name="T">Damage model, used to encourage multiple types to be represented.</typeparam>
    public class DamageController : MEFObject
    {
        #region Fields
        
        /// <summary>
        ///  The damage manager, that will allow us to communicate with the damage cache/storage.
        /// </summary>
        [Import(typeof(IDamageDataManager))]
        private IDamageDataManager damageManager;

        /// <summary>
        ///  The damage mapper, that will convert a generic model to arrays, and vice versa for us.
        /// </summary>
        private IDamageMapper damageMapper;

        /// <summary>
        ///  Logger that is used to output any useful information happening here.
        /// </summary>
        private readonly ILogger log = LoggerManager.CreateLogger(typeof(DamageController));

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Registers a given model as the DamageUnit to be used in this class.
        /// </summary>
        /// <param name="modelType">Model representing the damage types available to the player.</param>
        /// <typeparam name="T">T model, that inherits from DamageUnit.</typeparam>
        public void RegisterModelAsDamageUnit<T>() where T : DamageUnit
        {
            // Recreate the mapper, and map the model this instance was called with.
            this.damageMapper = new DamageMapper<T>();
        }

        #endregion

        #region Internal Methods


        #endregion
    }
}
