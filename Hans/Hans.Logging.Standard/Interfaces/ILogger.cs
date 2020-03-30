using Hans.Logging.Enums;

namespace Hans.Logging.Interfaces
{
    /// <summary>
    ///  The <see cref="ILogger" /> class, this represents the backbone of any logger created.  This allows us to have many different kinds, depending on the 
    ///     configuration found for this logging class.  Contains basic definitions for logging activities.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///  The external assembly this logger refers to.  This will be output with the log messages, if the configuration requires it.
        /// </summary>
        string externalAssembly { get; set; }

        /// <summary>
        ///  Logs a message to the logging thread, which will export the information as desired.
        /// </summary>
        /// <param name="logMsg">The message to log.</param>
        /// <param name="logLevel">At what level to log the message at.</param>
        void LogMessage(string logMsg, LogLevel logLevel = LogLevel.Debug);
    }
}
