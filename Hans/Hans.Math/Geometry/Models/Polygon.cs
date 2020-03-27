using System.Collections.Generic;

namespace Hans.Math.Geometry.Models
{
    /// <summary>
    ///  Represents a polygon in 2D space - Allows us, given a series of points or vertices, to describe a polygon in space.
    /// </summary>
    public class Polygon
    {
        /// <summary>
        ///  List of lines in the polygon, containing any length or vertex information needed.
        /// </summary>
        public IList<Line> Edges { get; set; }

        /// <summary>
        ///  List of vertices in the polygon, including any indexing information as well as point coordinates.
        /// </summary>
        public IList<Vertex> Vertices { get; private set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Polygon" /> class, including line definitions.
        /// </summary>
        /// <param name="vertices">Vertices that make up the polygon.</param>
        public Polygon(IList<Vertex> vertices)
        {
            this.Vertices = vertices;
        }

        /// <summary>
        /// Maps a collection of lines from given vertices, for usage in easier calculations later.
        ///     Note: Assumes vertices are provided in connection order.
        /// </summary>
        /// <param name="vertices">Vertices used in polygon calculation.</param>
        private void MapLinesFromVertices(IList<Vertex> vertices)
        {
            this.Edges = new List<Line>();
            for (var i = 0; i < vertices.Count; i++)
            {
                var thisPoint = vertices[i];
                var thatPoint = vertices[(i + 1) % vertices.Count];

                this.Edges.Add(new Line(thisPoint, thatPoint));
            }
        }
    }
}
