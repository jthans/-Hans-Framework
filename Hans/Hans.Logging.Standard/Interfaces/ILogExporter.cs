using Hans.Logging.Enums;
using Hans.Logging.Models;

namespace Hans.Logging.Interfaces
{
    /// <summary>
    ///  Interface that allows implementations of a log exporter, which takes logs from the applications and libraries, and brings them outside
    ///     of the system so they can be easily viewed.
    /// </summary>
    public interface ILogExporter
    {
        #region Properties

        /// <summary>
        ///  Indicates whether or not this exporter is currently enabled.  If disabled, it scraps the log and exits.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        ///  Exporters should have a "base" log level that will define which logs will even be processed.
        /// </summary>
        LogLevel MinLogLevel { get; set; }

        /// <summary>
        ///  Name, used to identify this exporter.  Will be used to update configurations.
        /// </summary>
        string Name { get; set; }

        #endregion

        /// <summary>
        ///  Creates a shallow copy of this base object, so we can have multiple exporters of a given type.  Is likely rare, but wanted to give the option.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        ILogExporter Copy();

        /// <summary>
        ///  Exports the log to an external source.
        /// </summary>
        /// <param name="logToExport">The log that we'll be exporting.</param>
        void ExportLog(Log logToExport);

        /// <summary>
        ///  Processes the Log, accounting for the minimum LogLevel and other checks to speed up the logging process.
        /// </summary>
        /// <param name="logToExport">The log that is being exported.</param>
        void ProcessLog(Log logToExport);
    }
}
