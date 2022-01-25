using Hans.DependencyInjection;
using Hans.Logging.Attributes;
using Hans.Logging.Interfaces;
using Hans.Logging.LogExporters;
using Hans.Logging.Models;
using Hans.Logging.Models.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hans.Logging
{
    /// <summary>
    ///  Thrad that runs for the logging system, so that logs can be one-shot sent to this class to be recorded.  This keeps all
    ///     applications and interactions moving quickly.
    /// </summary>
    public class LoggerThread : MEFObject
    {
        #region Fields

#pragma warning disable 0649

        /// <summary>
        ///  All log exporters found using DI, we'll filter these out based on any configuration included.
        /// </summary>
        [ImportMany(typeof(ILogExporter))]
        private IEnumerable<Lazy<ILogExporter>> allLogExporters;

#pragma warning restore 0649

        /// <summary>
        ///  The cancellation token that will allow us to safely stop the thread in this logger.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        ///  List of loggers that will be used to export to multiple locations at once.
        /// </summary>
        private List<ILogExporter> logExporters = new List<ILogExporter>();

        /// <summary>
        ///  How long to wait once the queue is emptied before we start processing again.
        /// </summary>
        private int millisecondsToWaitOnEmptyQueue = 250;

        #endregion

        #region Properties

        /// <summary>
        ///  The queue of messages that the thread processes continuously.
        /// </summary>
        public ConcurrentQueue<Log> LogMessageQueue;

        /// <summary>
        ///  If the service is running or not.
        /// </summary>
        public bool Running { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="LoggerThread" /> class, which continuously runs in order to log information over
        ///     the course of an application.
        /// </summary>
        /// <param name="config">Configuration information used for this log thread.</param>
        public LoggerThread(LoggingConfiguration config = null)
            : base()
        {
            this.Initialize(config);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Reconfigures the log exporters in this thread. Used only to modify properties, not to create/remove exporters.
        ///     NOTE: Do not use this function unless you have a consistent way to map configuration.  Missing exporters in Config will be disabled, and new ones will not go live.
        /// </summary>
        /// <param name="config">Configuration information for the exporters.</param>
        public void Reconfigure(LoggingConfiguration config)
        {
            this.LogReconfigure();
            foreach (var exporterConfig in config?.logExporters ?? new List<LogExporterConfiguration>())
            {
                var updateExporter = this.logExporters.FirstOrDefault(x => x.Name == exporterConfig.Name);
                if (updateExporter != null)
                {
                    updateExporter.IsEnabled = exporterConfig.Enabled;
                    updateExporter.MinLogLevel = exporterConfig.ExporterLevel;
                }
            }
        }

        /// <summary>
        ///  Runs the dispatch thread, which continuously checks the queue/prccesses the messages when they're received.
        /// </summary>
        public void RunThread()
        {
            Task.Factory.StartNew(() =>
            {
                // Initialize the cancellation token for this run.
                this.cancellationTokenSource = new CancellationTokenSource();
                Running = true;

                while (true)
                {
                    // Cancellation was requested, so we'll break out of this dispatch loop.
                    if (this.cancellationTokenSource.IsCancellationRequested)
                    {
                        this.Initialize();
                        break;
                    }

                    // If there's no logs in the queue, continue.
                    Log nextLog;
                    if (!this.LogMessageQueue.TryDequeue(out nextLog))
                    {
                        Thread.Sleep(millisecondsToWaitOnEmptyQueue);
                        continue;
                    }

                    // Export the log for all exporters that we have configured.
                    logExporters.ForEach(x => x.ProcessLog(nextLog));
                } 

                this.cancellationTokenSource = null;
                Running = false;
            });
        }

        /// <summary>
        ///  Cancels execution of the dispatch thread, while it's running continuously.
        /// </summary>
        public void StopThread()
        {
            // If we don't have a cancellation token, this thread hasn't even started.
            if (this.cancellationTokenSource == null)
            {
                return;
            }

            // If the thread can't be cancelled, it's probably in a stopping state. If we can cancel, do so.
            if (this.cancellationTokenSource.Token.CanBeCanceled)
            {
                this.cancellationTokenSource.Cancel();
                return;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  Handles any logic that's needed to initialize this thread to a working state.
        /// </summary>
        /// <param name="config">Configuration details, if available.</param>
        private void Initialize(LoggingConfiguration config = null)
        {
            this.LogMessageQueue = new ConcurrentQueue<Log>();
            this.LoadLogExporters(config);
        }

        /// <summary>
        ///  Loads the required log exports from the ones found into this logging service.  
        /// </summary>
        /// <param name="config">Configuration information, if available.</param>
        private void LoadLogExporters(LoggingConfiguration config = null)
        {
            // Default Behavior, Start all Exporters Found (Easy Debugging)
            if (config == null)
            {
                // Initialize all log exporters except the base type, all other configurations will be handled in the log additions.
                foreach (var logExporter in this.allLogExporters.Where(x => !x.GetType().IsSubclassOf(typeof(BaseLogExporter))))
                {
                    this.logExporters.Add(logExporter.Value);
                }
            }
            else
            {
                // One-Time Calculation of Exporter Types
                Dictionary<string, ILogExporter> mappedExporter = new Dictionary<string, ILogExporter>();
                foreach (var logExporter in this.allLogExporters.Where(x => !x.GetType().IsSubclassOf(typeof(BaseLogExporter))))
                {
                    string exporterType = ((ExporterTypeAttribute)(logExporter.GetType().GetCustomAttributes(typeof(ExporterTypeAttribute), false)?[0]))?.Type ?? "NULL"; // Grabs the [ExporterType("VALUE"] Attribute, and Maps to the Class Types
                    if (exporterType != "NULL" &&
                        !mappedExporter.ContainsKey(exporterType))
                    {
                        mappedExporter.Add(exporterType, logExporter.Value);
                    }
                    else
                    {
                        // We should break here if logging is seriously misconfigured, this can cause issues and we need them to correct it.
                        throw new Exception($"Log Initialization Issue: Exporter Type { exporterType } is mapped multiple times, or is NULL.  Please correct the ExporterType Attributes of your custom exporters.");
                    }
                }

                // Initialize only exporters located in the configuration file, enabled or disabled. (Them being present means they may want to enable later.)
                foreach (var exporterConfig in config.logExporters ?? new List<LogExporterConfiguration>())
                {
                    if (mappedExporter.ContainsKey(exporterConfig.Type))
                    {
                        var newExporter = mappedExporter[exporterConfig.Type].Copy();
                        newExporter.Name = string.IsNullOrEmpty(exporterConfig.Name) ? exporterConfig.Type : exporterConfig.Name;

                        this.logExporters.Add(newExporter);
                    }
                    else
                    {
                        // If they're trying to create a logger that doesn't exist, we should throw an exception and tell them - Thought about suppressing this, but it may not give them another logging method to see any issues otherwise.
                        throw new Exception($"Log Initialization Issue: Exporter Type { exporterConfig.Type } wasn't found in the code.  Check the type and try again.");
                    }
                }
            }
        }

        /// <summary>
        ///  Log the Reconfigure, for Debugging Purposes
        /// </summary>
        private void LogReconfigure()
        {
            Log reconfigureLog = new Log()
            {
                CallingAssembly = "Hans.Logging.Standard",
                Level = Enums.LogLevel.Debug,
                Message = "Log Reconfigure BEGIN",
                TimeStamp = DateTime.Now
            };

            this.LogMessageQueue.Enqueue(reconfigureLog);
        }

        #endregion
    }
}
