using Hans.DependencyInjection;
using Hans.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.Math.Test
{
    /// <summary>
    ///  Any Initialization/Cleanup Needed for Tests
    /// </summary>
    [TestClass]
    public class AssemblyManagement
    {
        #region Assembly Management

        /// <summary>
        ///  Initializes all logger tests by starting the logger service.
        /// </summary>
        [AssemblyInitialize]
        public static void InitializeLoggerTests(TestContext testContext)
        {
            MEFBootstrapper.Build();
            LoggerManager.StartLogging();
        }

        /// <summary>
        ///  Cleans up all logger test resources.
        /// </summary>
        [AssemblyCleanup]
        public static void CleanupLoggerTests()
        {
            LoggerManager.StopLogging();
        }

        #endregion
    }
}
