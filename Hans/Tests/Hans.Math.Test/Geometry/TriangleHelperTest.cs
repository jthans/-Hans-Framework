using Hans.Math.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.Math.Test
{
    /// <summary>
    ///  Class that tests and validates the <see cref="TriangleHelper" /> class, and its funtionalities.
    /// </summary>
    [TestClass]
    public class TriangleHelperTest
    {
        /// <summary>
        ///  Ensures that our calculation of an inner angle of a triangle is correct, and hasn't changed.
        /// </summary>
        [TestMethod]
        public void CalculateAngleFromTriangleSides_CalculatesSuccessfully()
        {
            double aLength = 5;
            double bLength = 9;
            double cLength = 8;

            var calcResult = TriangleHelper.CalculateAngleFromTriangleSides(aLength, bLength, cLength, 1);
            Assert.AreEqual(62.2, calcResult);
        }
    }
}
