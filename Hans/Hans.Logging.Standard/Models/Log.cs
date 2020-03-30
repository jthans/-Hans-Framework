using Hans.Logging.Enums;
using System;

namespace Hans.Logging.Models
{
    /// <summary>
    ///  The log class model that will hold all information about a log to be recorded.
    /// </summary>
    public class Log
    {
        /// <summary>
        ///  The assembly that triggered the log.
        /// </summary>
        public string CallingAssembly { get; set; }

        /// <summary>
        ///  The level of the log at which the message should be logged.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        ///  The message of this log, the import part as it were.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  The timestamp at which this log was sent/received.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///  We'll format the log to be a standard statement for printing.
        ///     TODO: Maybe make this regex? So formatting can be changed.
        /// </summary>
        /// <returns>String representation of this log message.</returns>
        public override string ToString()
        {
            return $"{ this.TimeStamp.ToString() } / [{ this.CallingAssembly }] { this.Level.ToString() }: { this.Message }";
        }
    }
}
