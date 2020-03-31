using Hans.Math.Geometry.Models;
using System.Collections.Generic;

namespace Hans.Math.Geometry.Calculators
{
    /// <summary>
    ///  Interface representing the basic implementation of a calculator focused on providing a list of triangles (polygons) that make up a larger simple polygon.  Used
    ///     as an interface, as multiple implementations are possible.
    /// </summary>
    public interface ITriangulationCalculator
    {
        /// <summary>
        ///  Triangulates a simple (non-overlapping, closed) polygon into triangles for rendering/calculation/etc.
        /// </summary>
        /// <param name="poly">Polygon to be separated into triangles.</param>
        /// <returns>A list of triangles that make up the simple polygon.</returns>
        List<Polygon> TriangulatePolygon(Polygon poly);
    }
}
