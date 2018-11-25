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

        /// <summary>
        ///  Ensures that Redis adds multiple members to a set successfully.
        /// </summary>
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
        ///  Ensures that Redis removes items from Sets correctly.
        /// </summary>
        [TestMethod]
        public void Redis_SetRemovesMembersSuccessfully()
        {
            // Test Values
            const string setKey = "REM_SET";
            const string testKeyOne = "TEST_ONE";
            const string testKeyTwo = "TEST_TWO";

            string[] testMembers = new string[] { testKeyOne, testKeyTwo };

            // Create an accessor, then set multiple set values, removing one.
            using (var redisDAO = this.redisInstance.CreateAccessor())
            {
                // Set the values in the cache.
                redisDAO.Sets.AddMembers(setKey, testMembers);

                // Delete the members, and ensure that the second was deleted.
                redisDAO.Sets.DeleteMembers(setKey, testKeyOne);

                var readResult = redisDAO.Sets.GetAllMembers(setKey);
                Assert.IsTrue(readResult.Length == 1);
                Assert.IsTrue(readResult[0] == testKeyTwo);
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
