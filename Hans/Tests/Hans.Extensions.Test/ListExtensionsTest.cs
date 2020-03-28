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
        #region IsSameCycle

        /// <summary>
        ///  Ensures the IsSameCycle method returns true when a cycle is present.
        /// </summary>
        [TestMethod]
        public void IsSameCycle_ReturnsTrueWhenSuccess()
        {
            var listOne = new List<int> { 3, 4, 6, 7, 9, 2 };
            var listTwo = new List<int> { 7, 9, 2, 3, 4, 6 };

            Assert.IsTrue(listOne.IsSameCycle(listTwo));
        }

        /// <summary>
        ///  Ensures the IsSameCycle method returns false when a cycle is not present.
        /// </summary>
        [TestMethod]
        public void IsSameCycle_ReturnsFalseWhenUntrue()
        {
            var listOne = new List<int> { 3, 4, 6, 7, 9, 2 };
            var listTwo = new List<int> { 7, 9, 2, 8, 4, 6 };

            Assert.IsFalse(listOne.IsSameCycle(listTwo));
        }

        #endregion

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
