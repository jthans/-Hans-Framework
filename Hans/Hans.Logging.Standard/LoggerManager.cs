using Hans.Logging.Interfaces;
using Hans.Logging.Loggers;
using Hans.Logging.Models;
using Hans.Logging.Models.Configuration;
using System;

namespace Hans.Logging
{
    /// <summary>
    ///  Factory that generates a logger in the class responsible, so we can output data.
    /// </summary>
    public static class LoggerManager
    {
        #region Fields

        /// <summary>
        ///  The logging thread that cycles as long as the application is active.
        /// </summary>
        private static LoggerThread logThread = null;

        #endregion

        #region Public Methods

        /// <summary>
        ///  Creates a logger off of the class's information that requested it.  Also leans on configuration to ensure it has the best recent settings.
        /// </summary>
        /// <param name="classType">The class that's requesting the logger's creation.</param>
        /// <returns>A logger that can be used to send information out.</returns>
        public static ILogger CreateLogger(Type classType)
        {
            try
            {
                return new BaseLogger(classType.AssemblyQualifiedName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///  Queues a log message in the consumption thread.
        /// </summary>
        /// <param name="logMsg">The message to queue in the thread.</param>
        public static void QueueMessage(Log logMsg)
        {
            if (logThread != null)
            {
                lock (logThread.LogMessageQueue)
                {
                    logThread.LogMessageQueue.Enqueue(logMsg);
                }
            }
        }
        
        /// <summary>
        ///  Allows us to configure the logging components - This is focused on a startup call within <see cref="StartLogging" />, however the library is set up to handle real-time updates if
        ///     this method is called again.
        /// </summary>
        /// <param name="config">Configuration defining which log exporters will be created - If null, will create only a Console logger (aiming for "out of the box" readiness)
        public static void ConfigureLogging(LoggingConfiguration config = null)
        {
            logThread = new LoggerThread(config);
        }

        /// <summary>
        ///  Starts the logging thread.
        /// </summary>
        public static void StartLogging()
        {
            // If the thread doesn't exist, we need to create it.  We're assuming if they called this without configuration, we just want to start bare-bones.
            if (logThread == null)
            {
                ConfigureLogging();
            }

            // We don't want to start another if it's already running.
            if (logThread.Running)
            {
                return;
            }

            logThread.RunThread();
        }

        /// <summary>
        ///  Stops the logging thread.
        /// </summary>
        public static void StopLogging()
        {
            // We can't stop something that isn't running.
            if (!logThread.Running)
            {
                return;
            }

            logThread.StopThread();
        }

        #endregion
    }
}
