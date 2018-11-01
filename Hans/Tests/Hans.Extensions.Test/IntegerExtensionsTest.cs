using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.Extensions.Test
{
    /// <summary>
    ///  Tests the integer extensions contained in this class.
    /// </summary>
    [TestClass]
    public class IntegerExtensionsTest
    {
        #region ParseFromString 

        /// <summary>
        ///  Ensures that we can successfully parse a string into an integer, if the string is a number.
        /// </summary>
        [DataTestMethod]
        [DataRow("1")]
        [DataRow("43")]
        [DataRow("204")]
        public void ParseFromString_ParsesSuccessfully(string testString)
        {
            int testInt = 0;
            
            // Ensure it was parsed, and the number changed.
            Assert.IsTrue(IntegerExtensions.ParseFromString(ref testInt, testString));
            Assert.IsTrue(testInt > 0);
        }

        /// <summary>
        ///  Ensures that a parse fails when the string is not a number.
        /// </summary>
        [TestMethod]
        public void ParseFromString_ParseFailsOnCharacters()
        {
            int testInt = 32;
            string testString = "NotAnInteger";

            Assert.IsFalse(IntegerExtensions.ParseFromString(ref testInt, testString));
            Assert.AreEqual(0, testInt);
        }

        #endregion
    }
}
