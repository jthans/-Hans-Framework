using Hans.JSON;
using System;
using System.Collections.Generic;

namespace Hans.DialogueEngine.Entities
{
    /// <summary>
    ///  Node in a conversation, that represents a single thing said by a character, or
    ///     particpant in the conversation.
    /// </summary>
    public class ConversationNode : JSONEntity
    {
        #region Properties

        /// <summary>
        ///  The ID of the node, so that they can be accessed form other nodes.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///  The dialogue of this node, either said as a prompt, answer, etc.
        /// </summary>
        public string Dialogue { get; set; }

        /// <summary>
        ///  The list of actions that will be executed upon activation of this node.s
        /// </summary>
        public List<DialogueCheck> DialogueActions { get; set; }

        /// <summary>
        ///  Indicates if this node is enabled or not, null indicates a calculation has not been run.
        /// </summary>
        public bool? IsEnabled { get; private set; }

        /// <summary>
        ///  The checks required for this dialogue to be allowed to execute.
        /// </summary>
        public List<DialogueCheck> RequiredChecks { get; set; }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Sets the isEnabled flag to indicate whether or not this conversation node has been enabled.  This is 
        ///     set when a calculation has already been run, so we're not running it every time.
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetEnabled(bool isEnabled = true)
        {
            this.IsEnabled = isEnabled;
        }

        #endregion
    }
}
