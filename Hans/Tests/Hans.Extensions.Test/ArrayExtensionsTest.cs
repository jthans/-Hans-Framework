using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.Extensions.Test
{
    /// <summary>
    ///  Tests for the <see cref="ArrayExtensions" /> class, that supplies extensions for
    ///     array objects.
    /// </summary>
    [TestClass]
    public class ArrayExtensionsTest
    {
        /// <summary>
        ///  Ensures that with same-type concatenations, the array building is always successful.
        /// </summary>
        [TestMethod]
        public void Concatenate_ConcatenatesSuccessfully()
        {
            // Setting up the values we'll concatenate.
            int[] arrayOne = new int[] { 3, 4, 5, 6 };
            int valueOne = 7;
            int[] arrayTwo = new int[] { -1, 0, 1, 2 };

            // Expected
            var expectedArray = new int[] { 3, 4, 5, 6, 7, -1, 0, 1, 2 };

            // Do the concatenation.
            var arrayResult = ArrayExtensions.Concatenate<int>(arrayOne, valueOne, arrayTwo);

            Assert.IsTrue(expectedArray.Length == arrayResult.Length);
            for (var i = 0; i < expectedArray.Length; i++)
            {
                Assert.AreEqual(expectedArray[i], arrayResult[i]);
            }
        }
    }
}
