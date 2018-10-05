using Hans.DialogueEngine.Entities;
using Hans.Extensions;
using Hans.JSON;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hans.DialogueEngine
{
    /// <summary>
    ///  The conversation object, that contains a list of nodes, within which has their connections
    ///     to future nodes, events that need to be thrown, etc.  This is the entire list of dialogue
    ///     in this particular conversation.
    /// </summary>
    public class Conversation : JSONEntity
    {
        #region Fields
        
        /// <summary>
        ///  The current node for the conversation - Holds the value here.
        /// </summary>
        private ConversationNode currentNode { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///  The node currently being iterated on in this conversation.33
        /// </summary>
        public ConversationNode CurrentNode
        {
            get { return this.currentNode; }
            private set
            {
                if (value != null)
                {
                    this.SetNodeAsCurrent(value);
                }
            }
        }

        /// <summary>
        ///  The name of the conversation this entity represents.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The nodes on-deck to be activated based on the current node in the covnersation.  These are loaded, as responses will need their
        ///     dialogue to make a decision.
        /// </summary>
        public List<ConversationNode> NextNodes { get; set; }

        /// <summary>
        ///  The list of nodes currently in the system.
        /// </summary>
        public List<ConversationNode> Nodes { get; set; }

        /// <summary>
        ///  Dictionary of Conversational Nodes in this Conversation.
        ///     Key = NodeID, Value = Node
        /// </summary>
        protected Dictionary<Guid, ConversationNode> NodeLookup { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///  Instantiates a new instance of the <see cref="Conversation" /> class.
        /// </summary>
        public Conversation()
        {
            this.Nodes = new List<ConversationNode>();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Jumps to a particular node - Used if you want to skip pieces of conversation.
        ///     This will NOT trigger any actions on the node, nor check to see if it can be accessed.
        /// </summary>
        /// <param name="nodeID">The ID of the node.</param>
        public void JumpToNode(Guid nodeID)
        {
            var newNode = this.Nodes.FirstOrDefault(x => x.Id == nodeID);
            if (newNode != null)
            {
                this.CurrentNode = newNode;
            }
        }

        /// <summary>
        ///  Loads a JSON conversation into this object. Also does some setup based on the initial node and things.
        /// </summary>
        /// <param name="convoJson">The conversation, in JSON form.</param>
        /// <returns>If the conversation load was successful.</returns>
        public bool LoadConversation(string convoJson)
        {
            // First, load the conversation into this object.
            this.Load(convoJson);

            // If the object couldn't be validated, clear all properties.
            bool validateResult = this.ValidateConversation();
            if (!validateResult)
            {
                log.LogMessage("Conversation failed validation.  Object will be cleared.");

                this.ClearProperties();
                return validateResult;
            }

            // Build the lookup table, to make for easier node loading.
            this.NodeLookup = this.Nodes.ToDictionary(x => x.Id, x => x);

            // Assign the current node to the initial node found.
            this.CurrentNode = this.Nodes.FirstOrDefault(x => x.Initial);
            return true;
        }

        /// <summary>
        ///  Moves the conversation to the next node, if possible obviously.  Checks the execution permission of this node, and if it can execute, will do so.
        /// </summary>
        /// <param name="nextNodeId">Desired next node, used in decisions or conversations with two sides.  If null, choose the first available.</param>
        /// <returns>The next node in the series.</returns>
        public bool MoveToNextNode(Guid? nextNodeId = null)
        {
            // Grab the next best node for us to move into.
            var bestNode = this.GetNextBestNode(nextNodeId);

            if (bestNode != null)
            {
                // Fire all actions for activating this new node, before we set it as current.
                ConversationEngine.ActivateNode(bestNode);
                this.SetNodeAsCurrent(bestNode);

                return true;
            }

            // Moving to the next node was not possible, due to execution ability or a conversation error.
            return false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  Chooses the next best node out of the ones cached in the conversation.  If an ID is given, we choose that one.  If not, we look at all eligible nodes (those
        ///     that have their conditions met).  If more than one node is eligible, we'll look at the priority in relation to the current node to decide.
        /// </summary>
        /// <param name="nextNodeId">If not null, the ID they would like to choose.</param>
        /// <returns>The next node we'd like to execute, or null if none was found.</returns>
        private ConversationNode GetNextBestNode(Guid? nextNodeId = null)
        {
            // Determine which nodes are eligible to execute. Save the result in the nodes, so future calculations aren't necessary.
            var eligibleNodes = this.NextNodes.Where(x => ConversationEngine.AreConditionsMet(x, true)).ToList();

            // If NO nodes were found, the dialogue likely wasn't set up right, as there needs to be an exit point in the dialogue. Return null.
            if (!eligibleNodes.Any())
            {
                log.LogMessage($"No nodes are eligible to execute. This is likely a design fault in the conversation. Returning NULL.", Logging.Enums.LogLevel.Warning);
                return null;
            }

            // Initialize the conversation node as the requested node, if it was requested - Otherwise, we'll choose the best fit.
            ConversationNode selectedNode = nextNodeId.HasValue ? this.NextNodes.FirstOrDefault(x => x.Id == nextNodeId.Value) : null;
            if (selectedNode != null)
            {
                // Return the requested, if it can execute. If not, return null;
                return selectedNode.IsEnabled.HasValue && selectedNode.IsEnabled.Value ? selectedNode : null; ;
            }
            
            // If there's only 1 eligible node, return that.
            if (eligibleNodes.Count == 1)
            {
                return eligibleNodes.FirstOrDefault();
            }

            // Now, since we have more than 1 node eligible, we'll decide based on the conversation order chosen.
            // I realize this is a scary query, but I wanted it in one line. Basically, this orders the nodes by priority, with null priority being last. 
            // Then, we pick the first one that was calculated as eligible, and grab the node(s).  If there's more than one, we randomly choose one.
            var nextNodeSet = this.currentNode.NextNodes.Where(x => eligibleNodes.Select(e => e.Id).ToList().Contains(x.NextID))
                                                                    .OrderByDescending(x => x.Priority.HasValue)
                                                                    .ThenBy(x => x.Priority)
                                                                    .GroupBy(x => x.Priority)
                                                                    .FirstOrDefault();

            // There's only one - Perfect!
            if (nextNodeSet.Count() == 1)
            {
                return this.NodeLookup[nextNodeSet.FirstOrDefault().NextID];
            }

            // There's more than one, return a random one for us.
            return this.NodeLookup[nextNodeSet.ToList().GetRandomEntry().NextID];
        }

        /// <summary>
        ///  Sets the passed node as the current node, and updates all other values that rely on the current node.
        /// </summary>
        /// <param name="convoNode">The node to set as the current one.</param>
        private void SetNodeAsCurrent(ConversationNode convoNode)
        {
            this.currentNode = convoNode;

            // If any next nodes exist, we'll need to grab them and store them.
            this.NextNodes = new List<ConversationNode>();
            if (convoNode.NextNodes != null)
            {
                foreach (var nodeNext in convoNode.NextNodes)
                {
                    this.NextNodes.Add(this.NodeLookup[nodeNext.NextID]); // We can go ahead and do the lookup - Because we've already ensured all next nodes are available.
                }
            }
        }

        /// <summary>
        ///  Validates the conversation loaded in this object by ensuring the following:
        ///   Nodes exist in the conversation.
        ///   Only one initial node exists, no more no less.
        ///   Only one instance per node ID in the conversation, no duplicates.
        ///   All next IDs listed as connections must exist as nodes in the conversation.
        /// </summary>
        /// <returns></returns>
        private bool ValidateConversation()
        {
            // Check to ensure we actually have nodes first.
            if (this.Nodes == null || this.Nodes.Count == 0)
            {
                log.LogMessage($"No nodes were loaded for conversation { this.Name }! Aborting.", Logging.Enums.LogLevel.Error);
                return false;
            }

            // Check to ensure we have exactly ONE initial node.
            if (this.Nodes.Where(x => x.Initial).Count() != 1)
            {
                log.LogMessage($"Conversation { this.Name } does not have EXACTLY 1 initial node. Aborting.", Logging.Enums.LogLevel.Error);
                return false;
            }

            // Ensure that no node GUIDs were added twice - This could mess up our lookup table.
            var nodeIds = this.Nodes.Select(x => x.Id);
            if (nodeIds.Count() != nodeIds.Distinct().Count())
            {
                log.LogMessage($"Conversation { this.Name } contains duplicate node IDs.  Each ID may only be used once in a ocnversation. Aborting.", Logging.Enums.LogLevel.Error);
                return false;
            }

            // Ensure that all "next" nodes have a corresponding node in the converstaion.
            var nextNodeIds = this.Nodes.Where(x => x.NextNodes != null).SelectMany(x => x.NextNodes).Select(x => x.NextID).Distinct().ToList();
            if (nextNodeIds.Except(nodeIds).Count() != 0)
            {
                log.LogMessage($"Conversation { this.Name } contains nodes that look at future nodes not existing in the conversation. Aborting.", Logging.Enums.LogLevel.Error);
                return false;
            }

            return true;
        }

        #endregion
    }
}
