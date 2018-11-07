using Hans.Logging;
using Hans.Logging.Interfaces;
using System;
using System.Net.Sockets;
using TeamDev.Redis;

namespace Hans.Redis.DAO
{
    /// <summary>
    ///  Data accessor that's used to talk to a given Redis instance.
    /// </summary>
    public class RedisDataAccessor: IDisposable
    {
        #region Fields

        /// <summary>
        ///  Logger to output critical information about this object.
        /// </summary>
        private readonly ILogger log = LoggerManager.CreateLogger(typeof(RedisDataAccessor));

        /// <summary>
        ///  Data access provider that hosts the Redis instance, and allows access to its fields.
        /// </summary>
        private RedisDataAccessProvider redisDap;

        #endregion

        #region Properties

        /// <summary>
        ///  The hashing elements of the Redis accessor, used to not make this class so overwhelming.
        /// </summary>
        public RedisHashingAccessor Hashing { get; private set; }

        /// <summary>
        ///  The set elements of the Redis accessor, used to not make this class to overwhelming.
        /// </summary>
        public RedisSetAccessor Sets { get; private set; }

        #endregion
        
        #region Instance Methods

        /// <summary>
        ///  Disconnects from the current Redis instance.
        /// </summary>
        public void Disconnect()
        {
            if (redisDap == null)
            {
                return;
            }

            // Stop, dispose and delete the current redis interaction.
            redisDap.Close();
            redisDap.Dispose();

            redisDap = null;
        }

        /// <summary>
        ///  Disconnects from the Redis object, and cleans up the object,
        /// </summary>

        public void Dispose()
        {
            this.Disconnect();
        }

        /// <summary>
        ///  Initializes the accessor to communicate with a Redis instance.
        /// </summary>
        /// <param name="hostName">Hostname of the Redis service we're hitting.</param>
        /// <param name="portNum">Port number we're talking to for the Redis instance.</param>
        /// <returns>If the DAO was successfully initialized.</returns>
        public bool Initialize(string hostName, int portNum)
        {
            try
            {
                // Set up the accessor, and connect to the Redis instance passed.
                this.redisDap = new RedisDataAccessProvider();
                this.redisDap.Configuration.Host = hostName;
                this.redisDap.Configuration.Port = portNum;
                this.redisDap.Connect();

                // Assign all extender classes.
                this.Hashing = new RedisHashingAccessor(this.redisDap, this.log);
                this.Sets = new RedisSetAccessor(this.redisDap, this.log);
            }
            catch (SocketException ex)
            {
                log.LogMessage($"Error talking to the Redis instance at { hostName }:{ portNum }.  Can't access the Redis service. Ex: { ex.ToString() }");
                this.redisDap = null;
            }

            return this.redisDap != null;
        }

        #region Redis Commands

        /// <summary>
        ///  Gets the current stored value of the requested key.
        /// </summary>
        /// <param name="keyVal">Key value to retrieve.</param>
        /// <returns>The value stored in this key, or null if not found.</returns>
        public string GetValue(string keyVal)
        {
            // Refresh the DAO, and return the current value.
            this.redisDap?.ExecuteRedisCommand(this.log, RedisCommand.GET, keyVal);
            return this.redisDap?.Strings[keyVal].Get();
        }

        /// <summary>
        ///  Sets the value in this instance.
        /// </summary>
        /// <param name="keyVal">Key value we're setting a value for.</param>
        /// <param name="valStr">Value string we're setting a value for.</param>
        public void SetValue(string keyVal, string valStr)
        {
            this.redisDap?.ExecuteRedisCommand(this.log, RedisCommand.SET, keyVal, valStr);
        }

        #endregion

        #endregion
    }
}
