using Hans.Math.Geometry.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Hans.Math.Test.Geometry
{
    /// <summary>
    ///  All tests pertaining to the <see cref="Polygon" /> class.
    /// </summary>
    [TestClass]
    public class PolygonTest
    {
        #region CalculateAngles

        /// <summary>
        ///  Measures if all angles were correctly identified in a triangle.
        /// </summary>
        [TestMethod]
        public void CalculateAngles_CalculatedSuccessfully()
        {
            // Create the vertices, and the polygon.
            var aVertex = new Vertex(0, 0);
            var bVertex = new Vertex(11, 0);
            var cVertex = new Vertex(3.727, 3.333);

            var testPoly = new Polygon(new List<Vertex> { aVertex, bVertex, cVertex });
            var polyAngles = testPoly.CalculateAngles(2);

            Assert.AreEqual(3, polyAngles.Count);
            Assert.AreEqual(41.81, polyAngles[0]);
            Assert.AreEqual(24.62, polyAngles[1]);
            Assert.AreEqual(113.57, polyAngles[2]);
        }

        #endregion

        #region IsCongruentWith
        
        /// <summary>
        ///  Ensures that when line lengths are different, congruency fails.
        /// </summary>
        [TestMethod]
        public void IsCongruentWith_LineLengthDifferences()
        {
            var polyOne = new Polygon(new List<Vertex> { new Vertex(0, 0), new Vertex(1, 1), new Vertex(2, 1) });
            var polyTwo = new Polygon(new List<Vertex> { new Vertex(0, 0), new Vertex(1, 2), new Vertex(2, 1) });

            Assert.IsFalse(polyOne.IsCongruentWith(polyTwo));
        }

        /// <summary>
        ///  Ensures that polygons with differing number vertices fail congruency.
        /// </summary>
        [TestMethod]
        public void IsCongruentWith_NonmatchingVertices()
        {
            var polyOne = new Polygon(new List<Vertex> { new Vertex(0, 0), new Vertex(1, 1) });
            var polyTwo = new Polygon(new List<Vertex> { new Vertex(0, 0) });

            Assert.IsFalse(polyOne.IsCongruentWith(polyTwo));
        }

        /// <summary>
        ///  Ensures that congruent polygons are actually congruent.
        /// </summary>
        [TestMethod]
        public void IsCongruentWith_Success()
        {
            var polyOne = new Polygon(new List<Vertex> { new Vertex(0, 0), new Vertex(1, 1), new Vertex(3, 3) });
            var polyTwo = new Polygon(new List<Vertex> { new Vertex(0, 0), new Vertex(1, 1), new Vertex(3, 3) });

            Assert.IsTrue(polyOne.IsCongruentWith(polyTwo));
        }

        #endregion
    }
}
