using Hans.DependencyInjection;
using Hans.Logging.Interfaces;
using Hans.Logging.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        public LoggerThread()
            : base()
        {
            this.Initialize();
        }

        #endregion

        #region Instance Methods

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
                    logExporters.ForEach(x => x.ExportLog(nextLog));
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
        private void Initialize()
        {
            this.LogMessageQueue = new ConcurrentQueue<Log>();
            this.LoadLogExporters();
        }

        /// <summary>
        ///  Loads the required log exports from the ones found into this logging service.  
        ///     TODO: Read from a config, or some form of determining which to run.  To start, we're doing all of them.
        /// </summary>
        private void LoadLogExporters()
        {
            foreach (var logExporter in this.allLogExporters)
            {
                this.logExporters.Add(logExporter.Value);
            }
        }

        #endregion
    }
}
