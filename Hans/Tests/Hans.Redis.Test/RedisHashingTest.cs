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
        ///  Ensures that when a hash is requested to be deleted, deletes successfully.
        /// </summary>
        [TestMethod]
        [TestCategory("LocalDependent")]
        public void Redis_HashingDeletesHashSuccessfully()
        {
            // Test Values
            const string hashName = "DEL_HASH";
            const string testKey = "EmptyVal";

            // Create an accessor, then create a hash (make sure it exists), and delete it.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                redisDAO.Hashing.SetValues(hashName, new Dictionary<string, string>() { { testKey, "Empty" } });
                var readResult = redisDAO.Hashing.GetValues(hashName, testKey);

                Assert.IsTrue(readResult.Count > 0);

                // Delete the hash, and ensure the values no longer exist.
                redisDAO.Hashing.DeleteHash(hashName);
                readResult = redisDAO.Hashing.GetValues(hashName, testKey);

                Assert.IsTrue(readResult.Count == 0);
            }
        }

        /// <summary>
        ///  Ensures that we can get all keys for a hash in Redis successfully.
        /// </summary>
        [TestMethod]
        [TestCategory("LocalDependent")]
        public void Redis_HashingGetsKeysSuccessfully()
        {
            // Test Values
            const string hashName = "KEYS_HASH";
            const string keyOne = "KEY_ONE";
            const string keyTwo = "KEY_TWO";

            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                // Set the values in the cache.
                var testArgs = new Dictionary<string, string>()
                {
                    { keyOne, "TEST" },
                    { keyTwo, "HAI" }
                };

                redisDAO.Hashing.SetValues(hashName, testArgs);
                var readResult = redisDAO.Hashing.GetKeys(hashName);

                Assert.AreEqual(2, readResult.Length);
            }
        }

        /// <summary>
        ///  Ensures that hashes can set/get values successfully within Redis.  As the default
        ///     get/set commands work, these must be done together as they require one another
        ///     to validate success.
        ///     - This method validates single keys (HGET/HSET).
        /// </summary>
        [TestMethod]
        [TestCategory("LocalDependent")]
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
        [TestCategory("LocalDependent")]
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
        ///  Ensures that a hash can increment/decrement values within the hash successfully.
        /// </summary>
        [TestMethod]
        [TestCategory("LocalDependent")]
        public void Redis_HashingIncrementsValuesSuccessfully()
        {
            // Test Values
            const string hashName = "INCR_HASH";
            const string incrKey = "INCR_KEY";

            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                var testArgs = new Dictionary<string, string>()
                {
                    { incrKey, "45" }
                };

                // Load the 45 value into the cache.
                redisDAO.Hashing.SetValues(hashName, testArgs);

                // Add 20, and ensure that we have 65 saved in the value.
                redisDAO.Hashing.IncrementField(hashName, incrKey, 20);
                var readResult = redisDAO.Hashing.GetValues(hashName, incrKey);

                Assert.AreEqual("65", readResult[incrKey]);
            }
        }

        /// <summary>
        ///  Ensures that a missing hashValue parameter will throw the proper exception.
        /// </summary>
        [TestMethod]
        [TestCategory("LocalDependent")]
        public void Redis_HashingWithoutHashNameThrowsException()
        {
            // Create an accessor, then Get/Set a value.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.DeleteHash(null));
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.GetKeys(null));
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.GetValues(null));
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.IncrementField(null, null, 0));
                Assert.ThrowsException<ArgumentNullException>(() => redisDAO.Hashing.SetValues(null, null));
            }
        }
    }
}
