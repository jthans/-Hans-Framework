using Hans.Logging.Enums;

namespace Hans.Logging.Models.Configuration
{
    /// <summary>
    ///  Configuration for a single log exporter, to control how and where the logs go.  Defaults are assigned for all possible properties, so logging can be "out of the box" for simple implementations.
    /// </summary>
    public class LogExporterConfiguration
    {
        #region Default Values

        /// <summary>
        ///   Internal Storage of <see cref="Enabled" />, kept here so we can set a default.
        /// </summary>
        private bool _enabled = true;

        /// <summary>
        ///   Internal Storage of <see cref="ExporterLevel" />, kept here so we can set a default.
        /// </summary>
        private LogLevel _exporterLevel = LogLevel.Debug;

        #endregion

        #region Properties

        /// <summary>
        ///  Whether this exporter is enabled or not. 
        ///     Note: If Disabled, all logs received while Disabled will be lost for this exporter.
        /// </summary>
        public bool Enabled { get { return this._enabled; } set { this._enabled = value; } }

        /// <summary>
        ///   Minimum log level to be exported.  Anything at or above this setting will be received.
        /// </summary>
        public LogLevel ExporterLevel { get { return this._exporterLevel; } set { this._exporterLevel = value; } }

        /// <summary>
        ///  Name/ID of this log exporter.  Only required if multiple exporters of the same type are being run.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Type of this log exporter.  Can be custom, but recommended standard would be "Console" for type "ConsoleLogExporter", for example.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}
