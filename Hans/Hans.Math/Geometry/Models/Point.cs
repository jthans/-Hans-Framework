using System;

namespace Hans.Math.Geometry.Models
{
    /// <summary>
    ///  Represents a "Point" in 2D space.
    /// </summary>
    [Serializable]
    public class Point
    {
        #region Properties

        /// <summary>
        ///  X Coordinate of the Point
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///  Y Coordinate of the Point
        /// </summary>
        public double Y { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="Point" /> class, with X/Y coords.
        /// </summary>
        /// <param name="x">X Coord</param>
        /// <param name="y">Y Coord</param>
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Operators

        /// <summary>
        ///  Indicates if two points are equal to one another, by comparing their coordinates.
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>If the points represent the same point in space.</returns>
        public static bool operator == (Point p1, Point p2)
        {
            return p1?.X == p2?.X &&
                    p1.Y == p2?.Y;
        }

        /// <summary>
        ///  Indicates if two points are NOT equal to one another, by comparing their coordinates.
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>If the points do not represent the same point in space.</returns>
        public static bool operator != (Point p1, Point p2)
        {
            return p1?.X != p2?.X ||
                    p1?.Y != p2?.Y;
        }

        #endregion

        #region Class Overrides

        /// <summary>
        ///  Compares the points and determines if they are equivalent based on value.
        /// </summary>
        /// <param name="obj">Point to compare it to.</param>
        /// <returns>If the points are equivalent.</returns>
        /// <throws><see cref="ArgumentException"/>An improper type comparison was passed.</throws>
        public override bool Equals(object obj)
        {
            var otherPoint = (Point)obj;
            if (otherPoint == null)
            {
                throw new ArgumentException("Object passed to comparison must match the type being compared to!");
            }

            return this == otherPoint;
        }

        /// <summary>
        ///  Returns a hash code representing this point.
        /// </summary>
        /// <returns>Hash Code for X/Y Coord</returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() & this.Y.GetHashCode();
        }

        #endregion
    }
}
