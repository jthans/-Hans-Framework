using System.Collections.Generic;
using Hans.DialogueEngine.Interfaces;
using System.ComponentModel.Composition;
using System.IO;
using Hans.DialogueEngine.Test.Constants;

namespace Hans.DialogueEngine.Test.DialogueActions
{
    /// <summary>
    ///  Action that will update a file with a keyword, to show that we can fire actions on a node.
    /// </summary>
    [Export(typeof(IDialogueAction))]
    [ExportMetadata("Description", "FileWriter")]
    public class UpdateFileAction : IDialogueAction
    {
        /// <summary>
        ///  Performs an action, when fired.
        /// </summary>
        /// <param name="actionParams">Parameters useful for the action.</param>
        public void PerformAction(Dictionary<string, string> actionParams)
        {
            File.WriteAllText(DialogueFiles.WriteValues, DialogueFiles.WriteKeyword);
        }
    }
}
