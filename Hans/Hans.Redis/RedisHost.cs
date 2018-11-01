using Hans.Logging;
using Hans.Logging.Interfaces;
using Hans.Redis.Constants;
using Hans.Redis.DAO;
using System;
using System.Diagnostics;

namespace Hans.Redis
{
    /// <summary>
    ///  Redis class that will start a Redis host on the local machine, and giving the data access capabilities pre-mapped.
    /// </summary>
    public class RedisHost : IDisposable
    {
        #region Fields

        /// <summary>
        ///  Log object that will be used to output useful information about the processes in this class.
        /// </summary>
        private readonly ILogger log = LoggerManager.CreateLogger(typeof(RedisHost));

        /// <summary>
        ///  Stores the current instnace of Redis, so we can stop it if necessary.
        /// </summary>
        private Process currInstance;

        #endregion

        #region Properties

        /// <summary>
        ///  The singleton instance of the RedisHost.
        /// </summary>
        public static RedisHost Instance { get; private set; }
        static RedisHost()
        {
            Instance = new RedisHost();
        }

        /// <summary>
        ///  If the Redis instance is currently running.
        /// </summary>
        public bool Running
        {
            get
            {
                return this.currInstance != null && 
                        !this.currInstance.HasExited &&
                        this.currInstance.Responding;
            }
        }

        #endregion

        #region Disposal

        /// <summary>
        ///  Disposes the instance by killing the existing process.
        /// </summary>
        public void Dispose()
        {
            if (this.currInstance != null)
            {
                log.LogMessage("Killing Redis Instance...");

                this.currInstance.Kill();
                this.currInstance = null;
            }
        }

        /// <summary>
        ///  On disposal, kills the process that was running so Redis doesn't run forever.
        /// </summary>
        public static void DisposeInstance()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Creates a data accessor for the current local Redis instance.
        /// </summary>
        /// <returns>A data access, configured to the current Redis instance.</returns>
        public RedisDataAccessor CreateAccessor()
        {
            // No instance, no DAO.
            if (this.currInstance == null)
            {
                return null;
            }

            // Builds the accessor for the Redis instance.
            RedisDataAccessor newAccessor = new RedisDataAccessor();
            if (newAccessor.Initialize("localhost", InstanceParams.DefaultPortNumber))
            {
                return newAccessor;
            }

            return null;
        }

        /// <summary>
        ///  Starts a local Redis instance.
        /// </summary>
        /// <returns>If the Redis process started successfully or not.</returns>
        public bool Start()
        {
            try
            {
                log.LogMessage($"Starting Redis Host...", Logging.Enums.LogLevel.Information);

                // Set up the process information for this instance.
                Process redisHostProcess = new Process();
                ProcessStartInfo processInfo = new ProcessStartInfo();

                processInfo.FileName = InstanceParams.ServerExe;
                processInfo.WorkingDirectory = InstanceParams.FilePath + InstanceParams.VerNumber;
                processInfo.Arguments = InstanceParams.ConfigFileName;

                // Hide the window, and start the Redis instance.
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;

                log.LogMessage($"Running Command { processInfo.ToString() }...");

                // Assign the command, and run the process.
                redisHostProcess.StartInfo = processInfo;
                redisHostProcess.Start();

                this.currInstance = redisHostProcess;

                return true;
            }
            catch (Exception ex)
            {
                log.LogMessage($"Error starting Redis Host. Ex: { ex.ToString() }");
                return false;
            }
        }

        #endregion
    }
}
