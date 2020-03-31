using Hans.Math.Geometry.Models;
using System;

namespace Hans.Math.Geometry
{
    /// <summary>
    ///  Helper class containing useful calculations/algorithms for managing coordinate systems.
    /// </summary>
    public static class CoordinateHelper
    {
        #region Line Calculations

        /// <summary>
        ///  Calculates the length of the line, based on two existing points.
        /// </summary>
        /// <param name="line">The line to be evaluated.</param>
        /// <returns>The distance from one point to another.</returns>
        /// <throws><see cref="ArgumentException"/>,  if the line given is invalid.</throws>
        public static double CalculateLengthOfLine(Line line)
        {
            return line?.Endpoints?.Length == 2 ?
                    CoordinateHelper.CalculateLengthOfLine(line.Endpoints[0], line.Endpoints[1]) :
                    throw new ArgumentException("Line is invalid when calculating length.");
        }

        /// <summary>
        ///  Calculates the length of the line, based on two existing points.
        /// </summary>
        /// <param name="line">The line to be evaluated.</param>
        /// <returns>The distance from one point to another.</returns>
        public static double CalculateLengthOfLine(Point p1, Point p2)
        {
            if (p1 == null ||
                p2 == null)
            {
                // Can't work with empty points.
                throw new ArgumentException("Line is invalid when calculating midpoint.");
            }
            else if (p1 == p2)
            {
                // The midpoint will be the same as the points that were passed.
                return 0;
            }

            return System.Math.Sqrt(System.Math.Pow(p2.X - p1.X, 2)+ 
                                    System.Math.Pow(p2.Y - p1.Y, 2));
        }

        /// <summary>
        ///  Calculates the midpoint of a line, based on the two existing points.
        /// </summary>
        /// <param name="line">The line to calculate the midpoint of.</param>
        /// <returns>The midpoint of the line given.</returns>
        /// <throws><see cref="ArgumentException"/>, if line is invalid.</throws>
        public static Point CalculateMidpointOfLine(Line line)
        {
            return line?.Endpoints?.Length == 2 ?
                    CoordinateHelper.CalculateMidpointOfLine(line.Endpoints[0], line.Endpoints[1]) :
                    throw new ArgumentException("Line is invalid when calculating midpoint.");
        }

        /// <summary>
        ///  Calculates the midpoint of a line, based on two existing points.
        /// </summary>
        /// <param name="p1">Point 1 in X/Y Space</param>
        /// <param name="p2">Point 2 in X/Y Space</param>
        /// <returns>The midpoint of the points given.</returns>
        /// <throws><see cref="ArgumentException"/>One of the points was invalid.</throws>
        public static Point CalculateMidpointOfLine(Point p1, Point p2)
        {
            if (p1 == null ||
                p2 == null)
            {
                // Can't work with empty points.
                throw new ArgumentException("Line is invalid when calculating midpoint.");
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
