using System;

namespace Hans.DialogueEngine.Test.Constants
{
    /// <summary>
    ///  List of filenames that will be used in testing the dialogue engine.
    /// </summary>
    public class DialogueFiles
    {
        #region Failure Conversations

        /// <summary>
        ///  Filename for the conversation containing 2 nodes with the same ID.
        /// </summary>
        public const string DuplicateNodes = "DuplicateNodes.json";

        /// <summary>
        ///  Filename for the conversation containing no nodes, which is not allowed.
        /// </summary>
        public const string EmptyConversation = "EmptyConversation.json";

        /// <summary>
        ///  Filename for the conversation containing multiple initial nodes, which is not allowed.
        /// </summary>
        public const string MultipleInitialNodes = "MultipleInitialNodes.json";

        /// <summary>
        ///  Filename for the conversation where a next node is listed, but doesn't exist.
        /// </summary>
        public const string NextNodeNotPresent = "NextNodeNotPresent.json";

        #endregion

        #region SimpleConversation.json

        /// <summary>
        ///  Filename for the simple conversation file, used in most tests to determine node flow, etc.
        /// </summary>
        public const string SimpleConversation = "SimpleConversation.json";

        /// <summary>
        ///  The name of the conversation stored in SimpleConversation.json.
        /// </summary>
        public const string SimpleConversationName = "SimpleConversation";

        /// <summary>
        ///  ID of a node that is eligible when deciding between 2 nodes that are eligible.
        /// </summary>
        public static Guid SimpleNodeEligibleNode = new Guid("62a6e888-ac80-4550-a62b-f4616587251e");

        /// <summary>
        ///  ID of a node that is supposed to end the conversation.
        /// </summary>
        public static Guid SimpleNodeExitConversation = new Guid("10ec3284-090e-4237-97fa-fca3fb3c0c51");

        /// <summary>
        ///  ID of a node that'll trigger a file writer.
        /// </summary>
        public static Guid SimpleNodeFileWriter = new Guid("365e81f0-01b5-4954-aaf0-c8888dc21abf");

        /// <summary>
        ///  ID of a node that's higher priority, we'll test that with multiple eligible, higher priority will reign.
        /// </summary>
        public static Guid SimpleNodeHigherPriorityID = new Guid("2f208d30-d645-4819-acc5-eea71c7a8873");

        /// <summary>
        ///  The ID of the node that is considered a simple move - Only node, no checks.
        /// </summary>
        public static Guid SimpleNodeMoveID = new Guid("b816f10b-126c-4e13-984d-aa1aeb0c11d8");

        /// <summary>
        ///  ID of a node that's never eligible.
        /// </summary>
        public static Guid SimpleNodeNonEligibleNode = new Guid("c973ab24-b4a8-4e56-953f-c04a1e505ca3");

        /// <summary>
        ///  ID of a non-eligible node, that has an action attached.
        /// </summary>
        public static Guid SimpleNodeNonEligibleNodeFileWriter = new Guid("2cf56171-2de0-4cbe-8b25-366f30c86802");

        #endregion

        #region WriteValues.json

        /// <summary>
        ///  The keyword we'll write to the file to make sure this is working.
        /// </summary>
        public const string WriteKeyword = "BABABOOEY";

        /// <summary>
        ///  File we'll write to so that actions prove they're firing.
        /// </summary>
        public const string WriteValues = "WriteValues.json";

        #endregion
    }
}
