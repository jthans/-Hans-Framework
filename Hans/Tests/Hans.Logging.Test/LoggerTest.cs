using Hans.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.Logging.Test
{
    /// <summary>
    ///  Class that tests all basic logger functions.
    /// </summary>
    [TestClass]
    public class LoggerTest
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

        #region LoggerManager Tests

        /// <summary>
        ///  Ensures we can successfully create a logger of our class type.
        /// </summary>
        [TestMethod]
        public void CreateLogger_Successful()
        {
            var classLogger = LoggerManager.CreateLogger(this.GetType());
            Assert.IsNotNull(classLogger);
        }

        #endregion
    }
}
