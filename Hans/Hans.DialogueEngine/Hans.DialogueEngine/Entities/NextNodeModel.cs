using System;

namespace Hans.DialogueEngine.Entities
{
    /// <summary>
    ///  A model representing the next node in the series, from a given node.
    /// </summary>
    public class NextNodeModel
    {
        /// <summary>
        ///  The ID of this node we're representing.
        /// </summary>
        public Guid NextID { get; set; }

        /// <summary>
        ///  The priority of this node, used during decisions when more than one node is eligible.
        /// </summary>
        public int? Priority { get; set; }
    }
}
