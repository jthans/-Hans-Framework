using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Hans.Redis.Test
{
    /// <summary>
    ///  Ensures the Set components of the Redis libraries are working.
    /// </summary>
    [TestClass]
    public class RedisSetsTest
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

        [TestMethod]
        public void Redis_SetAddsMultipleMembersSuccessfully()
        {
            // Test Values
            string[] testKeys = new string[] { "TEST_ONE", "TEST_TWO" };
            string testSetName = "TEST_SET";

            // Create an accessor, then set multiple set values.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                // Set the values in the cache.
                redisDAO.Sets.AddMembers(testSetName, testKeys);

                // Load the values, and ensure they match.
                var readResult = redisDAO.Sets.GetAllMembers(testSetName);

                // We only have to make sure they exist in the set, because Redis ensures they are unique.
                Assert.IsTrue(readResult.Length == 2);
                Assert.IsTrue(readResult.Any(x => x == testKeys[0]));
                Assert.IsTrue(readResult.Any(x => x == testKeys[1]));
            }
        }

        /// <summary>
        ///  Ensures that a missing setName parameter will throw the proper exception.
        /// </summary>
        [TestMethod]
        public void Redis_HashingWithoutHashNameThrowsException()
        {
            // Create an accessor, then Get/Set a value.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Sets.GetAllMembers(null));
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Sets.AddMembers(null, null));
            }
        }
    }
}
