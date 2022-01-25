using Hans.Logging.LogExporters;
using System.Collections.Generic;

namespace Hans.Logging.Models.Configuration
{
    /// <summary>
    ///  Model responsible for containing configuration needed to manage the logging framework.  This will need to be mapped from the user's custom implementation (for real-time implementations), or based off 
    ///     a generic configuration file. (TODO: Provide generic configuration file for logging library)
    /// </summary>
    public class LoggingConfiguration
    {
        /// <summary>
        ///  List of Exporters we'd like to see added to the logging framework.  If empty, will create only a <see cref="ConsoleLogExporter" />
        /// </summary>
        public List<LogExporterConfiguration> logExporters { get; set; }
    }
}
