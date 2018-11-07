using Hans.Extensions;
using Hans.Logging.Interfaces;
using System;
using System.Collections.Generic;
using TeamDev.Redis;

namespace Hans.Redis.DAO
{
    /// <summary>
    ///  Accessor that manages operations pertaining to hashes in Redis.
    /// </summary>
    public class RedisHashingAccessor
    {
        #region Fields

        /// <summary>
        ///  Logger used to track events happening in this class.
        /// </summary>
        private ILogger log;

        /// <summary>
        ///  The DAO used to talk to the cache.
        /// </summary>
        private RedisDataAccessProvider redisDap;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="RedisHashingAccessor" /> class.
        /// </summary>
        /// <param name="_redisDap">The initialized data access object we'll use to communicate.</param>
        /// <param name="redisLogger">The logger that will be used in this class to create messages.</param>
        public RedisHashingAccessor(RedisDataAccessProvider _redisDap,
                                    ILogger redisLogger)
        {
            this.redisDap = _redisDap;
            this.log = redisLogger;
        }

        #endregion

        #region Redis Commands

        /// <summary>
        ///  Gets values for keys in a particular hash.
        /// </summary>
        /// <param name="hashName">Name of the hash we're accessing.</param>
        /// <param name="retrieveKeys">The keys to retrieve values for.</param>
        /// <exception cref="ArgumentNullException">Thrown if no hasn name was passed to the method.</exception>
        public Dictionary<string, string> GetValues(string hashName, params string[] retrieveKeys)
        {
            // Ensure a hash name was passed, and at least one record is being passed.
            if (string.IsNullOrEmpty(hashName))
            {
                throw new ArgumentNullException("hashName");
            }

            if (retrieveKeys.Length <= 0)
            {
                return new Dictionary<string, string>();
            }

            // Determine if we should get a single field, or multiple.
            RedisCommand getCmd = RedisCommand.HGET;
            if (retrieveKeys.Length > 1)
            {
                getCmd = RedisCommand.HMGET;
            }

            // Concat the keys requested, and grab the desired values.
            var paramArray = ArrayExtensions.Concatenate<string>(hashName, retrieveKeys);
            this.redisDap?.ExecuteRedisCommand(this.log, getCmd, paramArray);

            // For each key, get the value from the hash and add it to the results.
            Dictionary<string, string> resultsDic = new Dictionary<string, string>();
            foreach (var retrieveKey in retrieveKeys)
            {
                var keyValue = this.redisDap?.Hash[hashName].Get(retrieveKey);
                if (!string.IsNullOrEmpty(keyValue)) { resultsDic.Add(retrieveKey, keyValue); }
            }

            return resultsDic;
        }

        /// <summary>
        ///  Sets or creates a hash with the fields given - If the hash already exists, it should update the fields passed.
        /// </summary>
        /// <param name="hashName">Name of the hash to create.</param>
        /// <param name="hashFields">Fields to be saved/updated in the hash, with their values.</param>
        /// <exception cref="ArgumentNullException">Thrown if no hasn name was passed to the method.</exception>
        public void SetValues(string hashName, Dictionary<string, string> hashFields)
        {
            // Ensure a hash name was passed, and at least one record is being passed.
            if (string.IsNullOrEmpty(hashName))
            {
                throw new ArgumentNullException("hashName");
            }

            if (hashFields.Count <= 0)
            {
                return;
            }

            // Determine if we should set a single field, or multiple.
            RedisCommand setCmd = RedisCommand.HSET;
            if (hashFields.Count > 1)
            {
                setCmd = RedisCommand.HMSET;
            }

            var paramArray = ArrayExtensions.Concatenate<string>(hashName, Extensions.GetArgumentListAsStringArray(hashFields));
            this.redisDap?.ExecuteRedisCommand(this.log, setCmd, paramArray);
        }

        #endregion
    }
}
