using Hans.Logging.Interfaces;
using Hans.Logging.Loggers;
using Hans.Logging.Models;
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
        private static LoggerThread logThread = new LoggerThread();

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
            lock (logThread.LogMessageQueue)
            {
                logThread.LogMessageQueue.Enqueue(logMsg);
            }
        }

        /// <summary>
        ///  Starts the logging thread.
        /// </summary>
        public static void StartLogging()
        {
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
