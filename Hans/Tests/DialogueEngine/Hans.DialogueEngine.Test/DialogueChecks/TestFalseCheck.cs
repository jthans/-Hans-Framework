using System.Collections.Generic;
using Hans.DialogueEngine.Interfaces;
using System.ComponentModel.Composition;

namespace Hans.DialogueEngine.Test.DialogueChecks
{
    /// <summary>
    ///  A dialogue check that is "false" making a node never eligible.
    /// </summary>
    [Export(typeof(IExecutionAllowance))]
    [ExportMetadata("Description", "AlwaysFalse")]
    public class TestFalseCheck : IExecutionAllowance
    {
        /// <summary>
        ///  Indicates if the node can execute - It can't.
        /// </summary>
        /// <param name="actionParams">Any parameters to check execution.</param>
        /// <returns>false.</returns>
        public bool CanDialogueExecute(Dictionary<string, string> actionParams)
        {
            return false;
        }
    }
}
