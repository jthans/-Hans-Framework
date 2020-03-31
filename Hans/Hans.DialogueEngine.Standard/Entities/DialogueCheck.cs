using System.Collections.Generic;

namespace Hans.DialogueEngine.Entities
{
    /// <summary>
    ///  Model representing a dialogue check that needs to be performed.
    /// </summary>
    public class DialogueCheck
    {
        /// <summary>
        ///  THe key for the dialogue check, which we'll use to determine which calculation to perform.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///  Parameters to the check, that need to be passed in.
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
    }
}
