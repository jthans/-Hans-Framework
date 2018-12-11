using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.Redis.Test
{
    /// <summary>
    ///  Tests that ensure the data accessor class for the Redis server works correctly all-round.
    /// </summary>
    [TestClass]
    public class RedisDataAccessorTest
    {
        /// <summary>
        ///  Redis instance that we'll use for all tests, so we're not starting/stopping processes.
        /// </summary>
        private RedisHost redisInstance;

        #region Test Management

        /// <summary>
        ///  Initializes a Redis instance so we can use the same process across tests.
        /// </summary>
        [TestInitialize]
        public void InitializeDataAccessorTests()
        {
            this.redisInstance = new RedisHost();
            this.redisInstance.Start();
        }

        /// <summary>
        ///  Kills the Redis process, as we're done running tests on the instance.
        /// </summary>
        [TestCleanup]
        public void CleanupDataAccessorTests()
        {
            this.redisInstance.Dispose();
            this.redisInstance = null;
        }

        #endregion

        #region Commands

        /// <summary>
        ///  Ensures that we can properly get/set a value in Redis.  This test needed to be done in tandem, as
        ///     in order to check that one is working, the other must be as well.
        /// </summary>
        [TestMethod]
        [TestCategory("LocalDependent")]
        public void Redis_GetsOrSetsValueSuccessfully()
        {
            // Test Values
            string testKey = "KEY_TEST";
            string testValue = "TEST_VAL";

            // Create an accessor, then Get/Set a value.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                redisDAO.SetValue(testKey, testValue);
                var readResult = redisDAO.GetValue(testKey);

                Assert.AreEqual(testValue, readResult);
            }
        }

        #endregion
    }
}
