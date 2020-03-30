using Hans.Logging.Enums;
using Hans.Logging.Interfaces;
using Hans.Logging.Models;
using System;

namespace Hans.Logging.Loggers
{
    /// <summary>
    ///  Base logger class that does the simple task of adding the logging messages to the thread queue.
    /// </summary>
    public class BaseLogger : ILogger
    {
        /// <summary>
        ///  The external assembly that this logger was setup for.
        /// </summary>
        public string externalAssembly { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="BaseLogger" /> class, saving the assembly for log messages.
        /// </summary>
        /// <param name="assemblyInfo">The name of the assembly this logger was tied to.</param>
        public BaseLogger(string assemblyInfo = null)
        {
            this.externalAssembly = assemblyInfo;
        }

        /// <summary>
        ///  Logs a message to the <see cref="LoggerManager" />, where it will be queued and exported.
        ///     TODO: Implement "Params" Option that Allows Tags
        /// </summary>
        /// <param name="logMsg">Message to be logged.</param>
        /// <param name="logLevel">At what level to log the message at.</param>
        public void LogMessage(string logMsg, LogLevel logLevel = LogLevel.Debug)
        {
            // Create the new log, and queue it up for consumption!
            Log newLog = new Log()
            {
                CallingAssembly = this.externalAssembly,
                Level = logLevel,
                Message = logMsg,
                TimeStamp = DateTime.Now
            };

            LoggerManager.QueueMessage(newLog);
        }
    }
}
