using Hans.Logging.Models;

namespace Hans.Logging.Interfaces
{
    /// <summary>
    ///  Interface that allows implementations of a log exporter, which takes logs from the applications and libraries, and brings them outside
    ///     of the system so they can be easily viewed.
    /// </summary>
    public interface ILogExporter
    {
        /// <summary>
        ///  Exports the log to an external source.
        /// </summary>
        /// <param name="logToExport">The log that we'll be exporting.</param>
        void ExportLog(Log logToExport);
    }
}
