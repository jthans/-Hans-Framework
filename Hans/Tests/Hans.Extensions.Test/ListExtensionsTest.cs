using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Hans.Extensions.Test
{
    /// <summary>
    ///  Contians test to ensure the ListExtensions class successfully executes its logic.
    /// </summary>
    [TestClass]
    public class ListExtensionsTest
    {
        #region GetRandomEntry

        /// <summary>
        ///  Ensures the GetRandomEntry method returns something.
        /// </summary>
        [TestMethod]
        public void GetRandomEntry_ReturnsEntry()
        {
            List<string> testList = new List<string>() {  "test", "values" };
            Assert.IsNotNull(testList.GetRandomEntry());
        }

        /// <summary>
        ///  Ensures we return the default value (null in most cases) for an empty list.
        /// </summary>
        public void GetRandomEntry_ReturnsDefaultForEmptyList()
        {
            List<string> testList = new List<string>();
            Assert.IsNull(testList.GetRandomEntry());
        }

        #endregion
    }
}
