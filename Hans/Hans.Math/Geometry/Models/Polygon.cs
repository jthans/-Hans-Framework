using Hans.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hans.Math.Geometry.Models
{
    /// <summary>
    ///  Represents a polygon in 2D space - Allows us, given a series of points or vertices, to describe a polygon in space.
    /// </summary>
    [Serializable]
    public class Polygon
    {
        /// <summary>
        ///  The number of digits to use when calculating congruency.
        /// </summary>
        private const int CongruencyAngleResolution = 2;

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
            this.MapLinesFromVertices(this.Vertices);
        }

        /// <summary>
        ///  Calculates all internal angles for the polygon, moving from the first vertex to the next.
        /// </summary>
        /// <param name="numDigits">If you'd like a cap on the number of digits calculated, you can pass it here.</param>
        /// <returns>List of all calculated angles in the polygon.</returns>
        public List<double> CalculateAngles(int? numDigits = null)
        {
            List<double> calcAngles = new List<double>();
            for (var i = 0; i < this.Vertices.Count; i++)
            {
                var leftVertex = i == 0 ? this.Vertices.Last() : this.Vertices[i - 1];
                var rightVertex = this.Vertices[(i + 1) % this.Vertices.Count];
                var thisVertex = this.Vertices[i];

                calcAngles.Add(TriangleHelper.CalculateAngleFromTriangleSides(CoordinateHelper.CalculateLengthOfLine(leftVertex, thisVertex),
                                                                              CoordinateHelper.CalculateLengthOfLine(rightVertex, thisVertex),
                                                                              CoordinateHelper.CalculateLengthOfLine(leftVertex, rightVertex),
                                                                              numDigits));
            }

            return calcAngles;
        }

        /// <summary>
        ///  Indicates whether this polygon is congruent (equal, but perhaps rotated) with the given other polygon.
        /// </summary>
        /// <param name="otherPoly">The polygon to compare against.</param>
        /// <returns>If this polygon is congruent with another.</returns>
        public bool IsCongruentWith(Polygon otherPoly)
        {
            if (this.Vertices.Count != otherPoly.Vertices.Count)
            {
                return false;
            }

            var theseLineLengths = this.Edges.Select(x => x.Length).ToList();
            var thoseLineLengths = otherPoly.Edges.Select(x => x.Length).ToList();

            // Ensure it has the same line lengths, in the same order, as the other polygon.
            if (!theseLineLengths.IsSameCycle(thoseLineLengths))
            {
                return false;
            }

            var thesePolyAngles = this.CalculateAngles(CongruencyAngleResolution);
            var thosePolyAngles = otherPoly.CalculateAngles(CongruencyAngleResolution);

            if (!thesePolyAngles.IsSameCycle(thosePolyAngles))
            {
                return false;
            }

            // If it has the same number of sides, same lengths of sides, and same angle measurements, it's congruent.
            return true;
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
