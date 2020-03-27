namespace Hans.Math.Geometry.Models
{
    /// <summary>
    ///  Defines a Line object, with 2 points and a given distance between them.
    /// </summary>
    public sealed class Line
    {
        /// <summary>
        ///  Collection of points on this line.  For most and implemented purposes, this should be 2 points.
        /// </summary>
        public Vertex[] Endpoints { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Line" /> class.
        /// </summary>
        /// <param name="p1">Point #1 Coords</param>
        /// <param name="p2">Point #2 Coords</param>
        public Line(Vertex p1, Vertex p2)
        {
            this.Endpoints = new Vertex[] { p1, p2 };
        }
    }
}
