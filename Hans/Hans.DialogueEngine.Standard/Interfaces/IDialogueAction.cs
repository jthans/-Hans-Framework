using System.Collections.Generic;

namespace Hans.DialogueEngine.Interfaces
{
    /// <summary>
    ///  Interface that can be used to perform an action (or set of) on execution of dialogue.
    /// </summary>
    public interface IDialogueAction
    {
        /// <summary>
        ///  Method that performs an action with given parameters.
        /// </summary>
        /// <param name="actionParams">The action parameters that will be used to execute any action.</param>
        void PerformAction(Dictionary<string, string> actionParams);
    }
}
