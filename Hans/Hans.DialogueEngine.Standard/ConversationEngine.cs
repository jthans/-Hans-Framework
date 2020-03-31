using Hans.DependencyInjection;
using Hans.DialogueEngine.Interfaces;
using Hans.DialogueEngine.Entities;
using System.Collections.Generic;
using System.Linq;
using Hans.Logging.Interfaces;
using Hans.Logging;

namespace Hans.DialogueEngine
{
    /// <summary>
    ///  Conversation engine that's used to provide various methods and functions to conversations
    ///     and their nodes in order to move the dialogue along.  
    /// </summary>
    public static class ConversationEngine
    {
        #region Fields

        /// <summary>
        ///  Collection of actions used when progressing through a conversation that can be used to do things 
        ///     in response to a node being executed.
        /// </summary>
        private static Dictionary<string, IDialogueAction> dialogueActions;

        /// <summary>
        ///  Collection of checks used when progressing through a conversation that can be used to determine
        ///     if a node can be activated or not.
        /// </summary>
        private static Dictionary<string, IExecutionAllowance> dialogueChecks;

        /// <summary>
        ///  Logger that will be used to display information about processing happening in this class.
        /// </summary>
        private static ILogger log = LoggerManager.CreateLogger(typeof(ConversationEngine));

        #endregion

        #region Constructors

        /// <summary>
        ///  Instantiates an instance of the <see cref="ConversationEngine" /> when this class is first access.  This
        ///     will load the custom object into the class when loaded.
        /// </summary>
        static ConversationEngine()
        {
            log.LogMessage("Starting Conversation Engine...", Logging.Enums.LogLevel.Information);
            LoadDialogueComponents();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Activates a node, first checking if it's been told that it can, and activating all actions associated with the event.  Typically, this is capped off
        ///     by the dialogue being presented.
        /// </summary>
        /// <param name="convoNode">The conversation node we're processing.</param>
        public static void ActivateNode(ConversationNode convoNode)
        {
            // Ensure the node has been enabled first!
            if (!convoNode.IsEnabled.HasValue)
            {
                ConversationEngine.AreConditionsMet(convoNode, true);
            }

            if (!convoNode.IsEnabled.Value)
            {
                log.LogMessage($"Conversation attempted to activate node { convoNode.Id }, but it hasn't been enabled yet.  Please run AreConditionsMet before attempting to execute.", Logging.Enums.LogLevel.Warning);
                return;
            }

            // If the node's been enabled, execute all actions and make it the current node.
            convoNode.DialogueActions?.ForEach(x =>
            {
                // If we have an action in the code, we'll execute this action.
                if (dialogueActions.ContainsKey(x.Key))
                {
                    dialogueActions[x.Key].PerformAction(x.Params);
                }
            });
        }

        /// <summary>
        ///  Checks to see if the conditions on this node are met, and will set the value on the node
        ///     to reflect our calculations.
        /// </summary>
        /// <param name="convoNode">The conversation node to check eligibility.</param>
        /// <param name="isAttempt">If this check is for an actual attempt - If so, we'll cache the value in the node.</param>
        /// <returns>If the node can execute.</returns>
        public static bool AreConditionsMet(ConversationNode convoNode, bool isAttempt = false)
        {
            // Run through each check, and ensure they all return true.
            //  We'll return as soon as one of the checks is false.
            bool checkSuccessful = true;
            convoNode.RequiredChecks?.ForEach(x =>
            {
                // Don't calculate if we've already failed one.
                if (!checkSuccessful)
                {
                    return;
                }

                // See if this check passed.
                if (dialogueChecks.ContainsKey(x.Key))
                {
                    checkSuccessful = dialogueChecks[x.Key].CanDialogueExecute(x.Params);
                    log.LogMessage($"Dialogue Check { x.Key } Result: [{ checkSuccessful }]");
                }
                else
                {
                    checkSuccessful = false;
                    log.LogMessage($"Dialogue Check { x.Key } Not Found. Checks FAIL.", Logging.Enums.LogLevel.Error);
                }
            });

            // If we're trying to execute this node, lock the condition result in the node.
            if (isAttempt)
            {
                convoNode.SetEnabled(checkSuccessful);
            }

            return checkSuccessful;
        }
        
        #endregion

        #region Internal Methods

        /// <summary>
        ///  Loads dialogue components into the global variables stored in this class.
        /// </summary>
        private static void LoadDialogueComponents()
        {
            dialogueActions = MEFBootstrapper.ResolveManyWithMetaData<IDialogueAction, IConversationComponentMetadata>()
                                                .ToDictionary(x => x.Metadata.Description, x => x.Value);
            log.LogMessage($"{ dialogueActions.Count } actions loaded for the conversation engine.");

            dialogueChecks = MEFBootstrapper.ResolveManyWithMetaData<IExecutionAllowance, IConversationComponentMetadata>()
                                                .ToDictionary(x => x.Metadata.Description, x => x.Value);
            log.LogMessage($"{ dialogueChecks.Count } checks loaded for the conversation engine.");
        }

        #endregion
    }
}
