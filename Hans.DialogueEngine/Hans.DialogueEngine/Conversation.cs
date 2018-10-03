using Hans.DialogueEngine.Models;
using System;
using System.Collections.Generic;

namespace Hans.DialogueEngine
{
    /// <summary>
    ///  The conversation object, that contains a list of nodes, within which has their connections
    ///     to future nodes, events that need to be thrown, etc.  This is the entire list of dialogue
    ///     in this particular conversation.
    /// </summary>
    public class Conversation 
    {
        /// <summary>
        ///  The node currently being iterated on in this conversation.33
        /// </summary>
        public ConversationNode CurrentNode { get; set; }

        /// <summary>
        ///  Dictionary of Conversational Nodes in this Conversation.
        ///     Key = NodeID, Value = Node
        /// </summary>
        protected Dictionary<Guid, ConversationNode> Nodes { get; set; }
    }
}
