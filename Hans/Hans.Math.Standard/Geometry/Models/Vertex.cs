using System;

namespace Hans.Math.Geometry.Models
{
    /// <summary>
    ///  Represents a Vertex in 2D Space - X/Y Coords, along with other supporting information.
    /// </summary>
    [Serializable]
    public sealed class Vertex : Point
    {
        /// <summary>
        ///  The index of this vertex, to identify it within a gridspace or polygon.
        /// </summary>
        public int Index { get; set; }

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="Vertex" /> class.
        /// </summary>
        public Vertex()
        {

        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Vertex" /> class, with coordinate information only.
        /// </summary>
        /// <param name="x">X Coord</param>
        /// <param name="y">Y Coord</param>
        public Vertex (double x, double y)
            : base (x, y)
        {

        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Vertex" /> class.  
        /// </summary>
        /// <param name="index">Index of a given vertex, in context of the other vertices.</param>
        /// <param name="x">X Coord</param>
        /// <param name="y">Y Coord</param>
        public Vertex(int index, double x, double y)
            : base(x, y)
        {
            this.Index = index;
        }

        #endregion
    }
}