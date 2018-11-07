using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Hans.Redis.Test
{
    /// <summary>
    ///  Ensures the Hashing components of the Redis libraries are working.
    /// </summary>
    [TestClass]
    public class RedisHashingTest
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

        /// <summary>
        ///  Ensures that hashes can set/get values successfully within Redis.  As the default
        ///     get/set commands work, these must be done together as they require one another
        ///     to validate success.
        ///     - This method validates single keys (HGET/HSET).
        /// </summary>
        [TestMethod]
        public void Redis_HashingGetsOrSetsSingleValuesSuccessfully()
        {
            // Test Values
            const string hashName = "TEST_HASH";
            const string testKey = "KEY_TEST";
            const string testValue = "TEST_VAL";

            // Create an accessor, then Get/Set a value.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                // Set the values in the cache.
                var testArgs = new Dictionary<string, string>() { { testKey, testValue } };
                redisDAO.Hashing.SetValues(hashName, testArgs);

                // Load the values, and ensure they match.
                var readResult = redisDAO.Hashing.GetValues(hashName, new string[] { testKey });

                Assert.IsTrue(readResult.Count > 0);
                Assert.IsTrue(readResult.ContainsKey(testKey));
                Assert.AreEqual(testValue, readResult[testKey]);
            }
        }

        /// <summary>
        ///  Ensures that hashes can set/get values successfully within Redis.  As the default
        ///     get/set commands work, these must be done together as they require one another
        ///     to validate success.
        ///     - This method validates single keys (HMGET/HMSET).
        /// </summary>
        [TestMethod]
        public void Redis_HashingGetsOrSetsMultipleValuesSuccessfully()
        {
            // Test Values
            const string hashName = "TEST_HASH";

            const string testKeyOne = "KEY_TEST";
            const string testValueOne = "TEST_VAL";
            const string testKeyTwo = "KEY_TEST_TWO";
            const string testValueTwo = "TEST_VAL_TWO";

            // Create an accessor, then Get/Set a value.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                // Set the values in the cache.
                var testArgs = new Dictionary<string, string>()
                {
                    { testKeyOne, testValueOne },
                    { testKeyTwo, testValueTwo }
                };

                redisDAO.Hashing.SetValues(hashName, testArgs);

                // Load the values, and ensure they match.
                var readResult = redisDAO.Hashing.GetValues(hashName, new string[] { testKeyOne, testKeyTwo });

                Assert.IsTrue(readResult.Count > 0);

                Assert.IsTrue(readResult.ContainsKey(testKeyOne));
                Assert.AreEqual(testValueOne, readResult[testKeyOne]);

                Assert.IsTrue(readResult.ContainsKey(testKeyTwo));
                Assert.AreEqual(testValueTwo, readResult[testKeyTwo]);
            }
        }

        /// <summary>
        ///  Ensures that a missing hashValue parameter will throw the proper exception.
        /// </summary>
        [TestMethod]
        public void Redis_HashingWithoutHashNameThrowsException()
        {
            // Create an accessor, then Get/Set a value.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.GetValues(null));
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.SetValues(null, null));
            }
        }
    }
}
