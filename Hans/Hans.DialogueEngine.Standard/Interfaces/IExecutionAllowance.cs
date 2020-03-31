using System.Collections.Generic;

namespace Hans.DialogueEngine.Interfaces
{
    /// <summary>
    ///  Interface that can be used to determine if a dialogue is allowed to execute.
    /// </summary>
    public interface IExecutionAllowance
    {
        /// <summary>
        ///  Method that checks to see if a dialogue item is allowed to execute.
        /// </summary>
        /// <param name="actionParams">The parameters to this calculation.</param>
        /// <returns></returns>
        bool CanDialogueExecute(Dictionary<string, string> actionParams);
    }
}
