using Hans.Extensions;
using Hans.Logging.Interfaces;
using System;
using System.Linq;
using TeamDev.Redis;

namespace Hans.Redis.DAO
{
    /// <summary>
    ///  Accessor that manages operations pertaining to sets in Redis.
    /// </summary>
    public class RedisSetAccessor
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
        ///  Initializes a new instance of the <see cref="RedisSetAccessor" /> class.
        /// </summary>
        /// <param name="_redisDap">The initialized data access object we'll use to communicate.</param>
        /// <param name="redisLogger">The logger that will be used in this class to create messages.</param>
        public RedisSetAccessor(RedisDataAccessProvider _redisDap,
                                ILogger redisLogger)
        {
            this.redisDap = _redisDap;
            this.log = redisLogger;
        }

        #endregion

        #region Redis Commands

        /// <summary>
        ///  Adds members to a particular set, creating it if it doesn't already exist.
        /// </summary>
        /// <param name="setName">The name of the set to add/update.</param>
        /// <param name="membersToAdd">The member(s) to add to the given set.</param>
        public void AddMembers(string setName, params string[] membersToAdd)
        {
            // Ensure a set name was passed, and at least one record is being passed.
            if (string.IsNullOrEmpty(setName))
            {
                throw new ArgumentNullException("hashName");
            }

            if (membersToAdd.Length <= 0)
            {
                return;
            }

            // Pass the command to the cache.
            var paramArray = ArrayExtensions.Concatenate<string>(setName, membersToAdd);
            this.redisDap?.ExecuteRedisCommand(this.log, RedisCommand.SADD, paramArray);
        }

        /// <summary>
        ///  Gets all members existing in a set.
        /// </summary>
        /// <param name="setName">Name of the set to access members for.</param>
        /// <returns>All members in the requested set.</returns>
        public string[] GetAllMembers(string setName)
        {
            // Ensure a set name was passed, and at least one record is being passed.
            if (string.IsNullOrEmpty(setName))
            {
                throw new ArgumentNullException("hashName");
            }

            this.redisDap?.ExecuteRedisCommand(this.log, RedisCommand.SMEMBERS, setName);
            return this.redisDap?.Set[setName].Members;

        }

        #endregion
    }
}
