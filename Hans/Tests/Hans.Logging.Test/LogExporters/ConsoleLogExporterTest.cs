using Hans.Logging.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace Hans.Logging.Test.LogExporters
{
    /// <summary>
    ///  Tests the console logger object.
    /// </summary>
    [TestClass]
    public class ConsoleLogExporterTest
    {
        #region Fields

        /// <summary>
        ///  The logger used in these console tests.
        /// </summary>
        private ILogger consoleLogger;

        /// <summary>
        ///  Dummy console that we'll use to ensure we're writing out correctly.
        /// </summary>
        private StringWriter dummyConsole;

        #endregion

        #region Test Management

        /// <summary>
        ///  Initializes these tests by adding resources needed.
        /// </summary>
        [TestInitialize]
        public void InitializeConsoleLogTests()
        {
            // Switch the Console to export to this stringwriter.
            this.dummyConsole = new StringWriter();
            Console.SetOut(dummyConsole);

            this.consoleLogger = LoggerManager.CreateLogger(typeof(ConsoleLogExporterTest));
        }

        /// <summary>
        ///  Cleanup these tests by setting the Console back to default.
        /// </summary>
        [TestCleanup]
        public void CleanupConsoleLogTests()
        {
            // Set the Console to output back to the standard/default way.
            var stdOut = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(stdOut);
        }

        #endregion

        /// <summary>
        ///  Ensures that this log exporter logs to the Console.
        /// </summary>
        [TestMethod]
        public void ExportLog_Successful()
        {
            // Log a test message.
            string testMessage = "This is a test log message.";
            this.consoleLogger.LogMessage(testMessage);

            // Sleep a little so the logger has time to pass it.s
            Thread.Sleep(250);

            // Ensure the console sees that value in the recorded values.
            bool consoleContainsMessage = this.dummyConsole.ToString().Contains(testMessage);
            Assert.IsTrue(consoleContainsMessage);
        }
    }
}
