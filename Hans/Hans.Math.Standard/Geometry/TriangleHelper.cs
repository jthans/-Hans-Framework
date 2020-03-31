namespace Hans.Math.Geometry
{
    /// <summary>
    ///  Helper class that focuses on triangle and angle-based calculations.  This involves a lot of
    ///     trigonometric functionality.
    /// </summary>
    public static class TriangleHelper
    {
        /// <summary>
        ///  Calculates a given angle in a triangle provided 3 sides, using the Cosine theorum.
        /// </summary>
        /// <param name="a">Side A Length</param>
        /// <param name="b">Side B Length</param>
        /// <param name="c">Side C Length</param>
        /// <returns>The angle across from side C, in degrees.</returns>
        public static double CalculateAngleFromTriangleSides(double a, double b, double c, int? numDigits = null)
        {
            var calcResult = System.Math.Acos((
                                System.Math.Pow(a, 2) +
                                System.Math.Pow(b, 2) -
                                System.Math.Pow(c, 2)) /
                                (2.0 * a * b)) *
                                (180.0 / System.Math.PI); // Conversion into degrees.

            if (numDigits.HasValue)
            {
                calcResult = System.Math.Round(calcResult, numDigits.Value);
            }

            return calcResult;
        }
    }
}
