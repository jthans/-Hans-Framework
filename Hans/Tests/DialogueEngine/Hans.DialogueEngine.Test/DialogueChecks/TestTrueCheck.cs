using System.Collections.Generic;
using Hans.DialogueEngine.Interfaces;
using System.ComponentModel.Composition;

namespace Hans.DialogueEngine.Test.DialogueChecks
{
    /// <summary>
    ///  A dialogue check that is "true" making a node always eligible.
    /// </summary>
    [Export(typeof(IExecutionAllowance))]
    [ExportMetadata("Description", "AlwaysTrue")]
    public class TestTrueCheck : IExecutionAllowance
    {
        /// <summary>
        ///  Indicates if the node can execute - It can.
        /// </summary>
        /// <param name="actionParams">Any parameters to check execution.</param>
        /// <returns>true.</returns>
        public bool CanDialogueExecute(Dictionary<string, string> actionParams)
        {
            return true;
        }
    }
}
