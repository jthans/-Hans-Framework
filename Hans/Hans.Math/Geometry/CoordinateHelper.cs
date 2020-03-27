using Hans.Math.Geometry.Models;

namespace Hans.Math.Geometry
{
    /// <summary>
    ///  Helper class containing useful calculations/algorithms for managing coordinate systems.
    /// </summary>
    public static class CoordinateHelper
    {
        #region Line Calculations

        /// <summary>
        ///  Calculates the midpoint of a line, based on the two existing points.
        /// </summary>
        /// <param name="line">The line to calculate the midpoint of.</param>
        /// <returns>The midpoint of the line given.</returns>
        /// <returns>NULL, if line is invalid.</returns>
        public static Point CalculateMidpointOfLine(Line line)
        {
            return line?.Endpoints?.Length == 2 ?
                    CoordinateHelper.CalculateMidpointOfLine(line.Endpoints[0], line.Endpoints[1]) :
                    null;
        }

        /// <summary>
        ///  Calculates the midpoint of a line, based on two existing points.
        /// </summary>
        /// <param name="p1">Point 1 in X/Y Space</param>
        /// <param name="p2">Point 2 in X/Y Space</param>
        /// <returns>The midpoint of the points given.</returns>
        /// <returns>NULL, if either point provided is invalid.</returns>
        public static Point CalculateMidpointOfLine(Point p1, Point p2)
        {
            if (p1 == null ||
                p2 == null)
            {
                // Can't work with empty points.
                return null;
            }
            else if (p1 == p2)
            {
                // The midpoint will be the same as the points that were passed.
                return p1;
            }

            return new Point(
                            (p1.X + p2.X) / 2.0,
                            (p1.Y + p2.Y) / 2.0
                        );
        }

        #endregion
    }
}
