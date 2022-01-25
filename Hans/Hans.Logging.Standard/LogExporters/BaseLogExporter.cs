using Hans.Logging.Enums;
using Hans.Logging.Interfaces;
using Hans.Logging.Models;

namespace Hans.Logging.LogExporters
{
    /// <summary>
    ///   Base Log Exporter, used to define base information about the exporters.  This class is needed to define these basic properties, so that logging can be
    ///     configurable and updatable at the user's whim.  Without this, all exporters have 1 instance and are enabled at all times, for all logs.  We added this
    ///     so we can be more dynamic.
    /// </summary>
    public class BaseLogExporter : ILogExporter
    {
        #region Fields

        /// <summary>
        ///  Internal Storage for <see cref="IsEnabled"/> - Default(true)
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        ///  Internal Storage for <see cref="MinLogLevel"/> - Default(Debug)
        /// </summary>
        private LogLevel _minLogLevel = LogLevel.Debug;

        #endregion

        #region Properties

        /// <summary>
        ///  Indicates whether or not this exporter is currently enabled.  If disabled, it scraps the log and exits.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this._isEnabled;
            }
            set
            {
                this._isEnabled = value;
            }
        }

        /// <summary>
        ///  The minimum log level available to this exporter.  Logs lower priority than the assigned level will be ignored.
        /// </summary>
        public LogLevel MinLogLevel
        {
            get
            {
                return this._minLogLevel;
            }
            set
            {
                this._minLogLevel = value;
            }
        }

        /// <summary>
        ///  Name, used to identify this exporter.  Will be used to update configurations.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///  Creates a shallow copy of this base object, so we can have multiple exporters of a given type.  Is likely rare, but wanted to give the option.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        public ILogExporter Copy()
        {
            return (ILogExporter)this.MemberwiseClone();
        }

        /// <summary>
        ///  Base Export Method - When inherited, will dispense logs as configured.
        /// </summary>
        /// <param name="logToExport">The log we want to store somewhere.</param>
        public virtual void ExportLog(Log logToExport)
        {
            throw new System.NotImplementedException("The BaseLogExporter type performs no actions, and should not be utilized or configured. Double check your code/configuration.");
        }

        /// <summary>
        ///  Processes the Log, accounting for the minimum LogLevel and other checks to speed up the logging process.
        /// </summary>
        /// <param name="logToExport">The log that is being exported.</param>
        public void ProcessLog(Log logToExport)
        {
            // Only export if the exporter is enabled, and the level is configured to do so.
            if (this._isEnabled &&
                logToExport.Level >= this._minLogLevel)
            {
                this.ExportLog(logToExport);
            }
        }

        #endregion
    }
}
