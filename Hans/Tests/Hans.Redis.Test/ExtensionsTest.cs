using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Hans.Redis.Test
{
    /// <summary>
    ///  Ensures the Redis extensions are working properly, so our caching can happen correctly.
    /// </summary>
    [TestClass]
    public class ExtensionsTest
    {
        /// <summary>
        ///  Ensures that a given list of Key/Value pairs are parsed to a string succesfully, so the Redis engine
        ///     can understand them correctly.
        /// </summary>
        [TestMethod]
        public void ArgumentsParseToStringArraySuccessfully()
        {
            // Set up the variables that will be used.
            var testArgs = new Dictionary<string, string>()
            {
                { "KEY_ONE", "VALUE_ONE" },
                { "KEY_TWO", "VALUE_TWO" }
            };

            // Results we want to see.
            string[] expectedResult = new string[] { "KEY_ONE", "VALUE_ONE", "KEY_TWO", "VALUE_TWO" };

            // Execute the method, and compare to the expected.
            var argsToStringResult = Extensions.GetArgumentListAsStringArray(testArgs);

            Assert.IsTrue(expectedResult.Length == argsToStringResult.Length);
            for (var i = 0; i < expectedResult.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], argsToStringResult[i]);
            }
        }
    }
}
