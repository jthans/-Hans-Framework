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

        /// <summary>
        ///  Removes the next ear clipping from the given polygon, and returns if it's the last remaining triangle within the shape.
        /// </summary>
        /// <param name="currentPoly">The polygon this iteration will work on.</param>
        /// <param name="triangles">List of triangles/ear-clippings found in this polygon.</param>
        /// <returns>If this is the last ear-clipping (meaning we're left with a triangle) to terminate the loop.</returns>
        private bool RemoveNextEarClipping(Polygon currentPoly, ref List<Polygon> triangles)
        {
            // If we're left with a triangle, add it to our list and terminate the loop.
            if (currentPoly.Vertices.Count == 3)
            {
                triangles.Add(currentPoly);
                return true;
            }

            // todo
            // find list of all convex (sticking out) triangles in the polygon
            // can skip any that are concave
            // iterate over those to find first ear
            // remove ear, call this method again
            for (var i = 1; i <= currentPoly.Vertices.Count; i++)
            {

            }

            return true;
        }
    }
}
