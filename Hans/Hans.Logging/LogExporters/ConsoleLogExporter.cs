using Hans.Logging.Interfaces;
using System.ComponentModel.Composition;
using Hans.Logging.Models;
using System;

namespace Hans.Logging.LogExporters
{
    /// <summary>
    ///  Logging exporter that will display all logs in the console/command prompt running this application.
    /// </summary>
    [Export(typeof(ILogExporter))]
    public class ConsoleLogExporter : ILogExporter
    {
        /// <summary>
        ///  Exports the log placed here in the Console.
        /// </summary>
        /// <param name="logToExport">Log Message.</param>
        public void ExportLog(Log logToExport)
        {
            Console.WriteLine(logToExport.ToString());
        }
    }
}
