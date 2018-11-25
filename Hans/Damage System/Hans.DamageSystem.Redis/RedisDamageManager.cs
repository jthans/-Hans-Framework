using Hans.DamageSystem.Interfaces;
using Hans.Logging;
using Hans.Logging.Interfaces;
using Hans.Redis;
using Hans.Redis.DAO;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using Hans.DamageSystem.Models;
using Hans.Extensions;

namespace Hans.DamageSystem.Redis
{
    /// <summary>
    ///  Damanage Manager that uses a Redis cache to store/read values.  This is expected to be a very high-efficiency, very
    ///     thread-safe application for a damage system, especially if moving into multiplayer applications.
    /// </summary>
    [Export(typeof(IDamageDataManager))]
    public class RedisDamageManager : IDamageDataManager
    {
        #region Constants

        /// <summary>
        ///  Key to grab the damage cap from a layer.
        /// </summary>
        private const string DamageCapKey = "Cap";

        /// <summary>
        ///  Key to grab the DamageType from a layer.
        /// </summary>
        private const string DamageTypeKey = "DamageType";

        /// <summary>
        ///  Key to grab the multiplier from a layer.
        /// </summary>
        private const string MultiplierKey = "Multiplier";

        #endregion

        #region Fields

        /// <summary>
        ///  The entity layers set that will be tracked.
        /// </summary>
        /// <param name="entityId">The entity that the layer is being saved to.</param>
        /// <returns>Formatted entity layers set key.</returns>
        private string EntityLayers(string entityId) => $"entity.{ entityId }:layers";

        /// <summary>
        ///  Formatter for the layer ID needed to store/read from Redis.
        /// </summary>
        /// <param name="entityId">The entity ID that the layer is saved to.</param>
        /// <param name="layerId">The layer ID that is being saved/accessed.</param>
        /// <returns>Formatted entity layer mask key.</returns>
        private string LayerId(string entityId, string layerId) => $"entity.{ entityId }:layer.{ layerId }";

        /// <summary>
        ///  Logger to give us insight into what's happening.
        /// </summary>
        private readonly ILogger log = LoggerManager.CreateLogger(typeof(RedisDamageManager));

        /// <summary>
        ///  Accessor to interact with the Redis instance.
        /// </summary>
        private RedisDataAccessor redisCache;

        /// <summary>
        ///  Redis Host that will run the instance.
        /// </summary>
        private RedisHost redisHost;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="RedisDamageManager" /> class, which will allow us to interact with the cache.
        /// </summary>
        public RedisDamageManager()
        {
            // Create the Redis host, start it, and save the accessor.
            //  TODO: Connect to an existing one, if necessary.
            this.redisHost = new RedisHost();
            this.redisHost.Start();

            this.redisCache = this.redisHost.CreateAccessor();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Adds a layer to the given entity in the Redis cache.
        /// </summary>
        /// <param name="entityId">The entity ID that will be added to.</param>
        /// <param name="layerMask">The layer mask configuration that's being added.</param>
        public void AddLayer(string entityId, LayerMask layerMask)
        {
            // Add the layermask to an entity.
            this.redisCache.Sets.AddMembers(this.EntityLayers(entityId), layerMask.Id);

            // Add the layer as a hash to be tracked.
            var layerInfo = new Dictionary<string, string>()
            {
                { DamageCapKey, layerMask.DamageCap.ToString() },
                { DamageTypeKey, layerMask.DamageType },
                { MultiplierKey, layerMask.Multiplier.ToString() }
            };

            this.redisCache.Hashing.SetValues(this.LayerId(entityId, layerMask.Id), layerInfo);

            // If a duration is specified, add that to the expiry so Redis will forget the layer for us.
            if (layerMask.Duration > 0)
            {
                this.redisCache.ExpireAfterSeconds(this.LayerId(entityId, layerMask.Id), (int)layerMask.Duration);
            }
        }

        /// <summary>
        ///  Applies damage to the entity, and returns the damage remaining.
        /// </summary>
        /// <param name="entityId">The entity to apply damage to.</param>
        /// <param name="damageToApply">Translated damage to apply to the entity.</param>
        /// <returns></returns>
        public Dictionary<string, decimal> ApplyDamage(string entityId, Dictionary<string, decimal> damageToApply)
        {
            // For each damage type that has a value, apply it to the existing entity.
            foreach (var damageType in damageToApply.Keys)
            {
                if (damageToApply[damageType] == 0)
                {
                    continue;
                }

                this.redisCache.Hashing.IncrementField(entityId, damageType, damageToApply[damageType]);
            }

            return null;
        }

        /// <summary>
        ///  Adds an entity to Redis for caching.
        /// </summary>
        /// <param name="entityId">The entity's ID.</param>
        /// <param name="startHealth">The base health the entity starts with.</param>
        public void BeginTrackingDamage(string entityId, decimal startHealth)
        {
            this.redisCache.Hashing.SetValues(entityId, new Dictionary<string, string>() { { "BaseHealth", startHealth.ToString() } });
        }

        /// <summary>
        ///  Stops tracking an entity in Redis, removing all traces of the entity in the cache.
        /// </summary>
        /// <param name="entityId">Entity to stop tracking.</param>
        public void EndTrackingDamage(string entityId)
        {
            this.redisCache.Hashing.DeleteHash(entityId);
        }

        /// <summary>
        ///  Gets all layers existing in the cache for a particular entity.
        /// </summary>
        /// <param name="entityId">The entity to get all layers for.</param>
        /// <returns>All layers existing for a particular entity.</returns>
        public LayerMask[] GetLayersForEntity(string entityId)
        {
            // Get all IDs for this entity's layers.
            var layerIds = this.redisCache.Sets.GetAllMembers(this.EntityLayers(entityId));

            // Build the array to return to the caller.
            LayerMask[] layersList = new LayerMask[layerIds.Length];
            for (var i = 0; i < layerIds.Length; i++)
            { 
                // Get all keys for this layer, and build the object to add to the list.
                var layerResult = this.redisCache.Hashing.GetValues(this.LayerId(entityId, layerIds[i]), DamageCapKey, DamageTypeKey, MultiplierKey);
                if (layerResult.Count == 0)
                {
                    // If no results returned, the layer must have expired, or been removed elsewhere from the entity.
                    this.RemoveLayer(entityId, layerIds[i]);
                    continue;
                }

                // Parse the numerical values from the strings grabbed from the cache.
                int damageCap = 0;
                int multiplier = 0;

                NumericalExtensions.ParseIntFromString(ref damageCap, layerResult[DamageCapKey]);
                NumericalExtensions.ParseIntFromString(ref multiplier, layerResult[MultiplierKey]);

                layersList[i] = new LayerMask(layerResult[DamageTypeKey], (decimal)damageCap, null, (decimal)multiplier);
            }

            return layersList;
        }

        /// <summary>
        ///  Removes a layer from an entity in the cache.
        /// </summary>
        /// <param name="entityId">The entity that we're removing a layer from.</param>
        /// <param name="layerId">The layer that we're removing.</param>
        public void RemoveLayer(string entityId, string layerId)
        {
            this.redisCache.Sets.DeleteMembers(this.EntityLayers(entityId), layerId);
            this.redisCache.Hashing.DeleteHash(this.LayerId(entityId, layerId));
        }

        /// <summary>
        ///  Updates a layer for a given entity.
        /// </summary>
        /// <param name="entityId">The entity to update a layer for.</param>
        /// <param name="layerMask">The layer to update, and the values to update it with.</param>
        public void UpdateLayer(string entityId, LayerMask layerMask)
        {
            // Load the updated layer info into a dictionary, and send to Redis.
            var layerInfo = new Dictionary<string, string>()
            {
                { DamageCapKey, layerMask.DamageCap.ToString() },
                { DamageTypeKey, layerMask.DamageType },
                { MultiplierKey, layerMask.Multiplier.ToString() }
            };

            this.redisCache.Hashing.SetValues(this.LayerId(entityId, layerMask.Id), layerInfo);
        }

        #endregion
    }
}
