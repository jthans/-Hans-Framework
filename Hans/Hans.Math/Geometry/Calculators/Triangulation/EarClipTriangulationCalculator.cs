using System.Collections.Generic;
using Hans.Math.Geometry.Models;

namespace Hans.Math.Geometry.Calculators
{
    /// <summary>
    ///  Triangulation Calculator using the "Ear Clipping" Methodology.
    /// </summary>
    public class EarClipTriangulationCalculator : ITriangulationCalculator
    {
        /// <summary>
        ///  Triangulates a polygon using the ear-clip method.  Basic algorithm is as such:
        ///     - Find a triangle that exists entirely in the polygon with no overlapping vertices
        ///     - If this triangle has a single side interacting with the polygon, and no other points contained within, remove it.
        ///     - Add removed triangle to results list, and continue until larger polygon contains only a single triangle.  
        ///     - Return accumulated resultset.
        /// </summary>
        /// <param name="poly">Polygon to be triangulated.</param>
        /// <returns>A list of triangles that make up the simple polygon passed.</returns>
        public List<Polygon> TriangulatePolygon(Polygon poly)
        {
            throw new System.NotImplementedException();
        }
    }
}
