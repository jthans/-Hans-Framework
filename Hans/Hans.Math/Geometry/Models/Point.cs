namespace Hans.Math.Geometry.Models
{
    /// <summary>
    ///  Represents a "Point" in 2D space.
    /// </summary>
    public class Point
    {
        /// <summary>
        ///  X Coordinate of the Point
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///  Y Coordinate of the Point
        /// </summary>
        public double Y { get; set; }

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
    }
}
