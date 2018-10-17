using Hans.DialogueEngine.Test.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Hans.DialogueEngine.Test
{
    /// <summary>
    ///  Class that tests all standard conversation logic.
    /// </summary>
    [TestClass]
    public class ConversationEngineTest
    {
        #region ActivateNode

        /// <summary>
        ///  Activates the node's actions, when it's able to (eligible)
        /// </summary>
        [TestMethod]
        public void ActivateNode_ActivatesNodesWhenAble()
        {
            var openFile = File.Create(DialogueFiles.WriteValues);
            openFile.Close();

            // Load the conversation, and jump to a useful node for us to test.
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);
            testConvo.JumpToNode(DialogueFiles.SimpleNodeFileWriter);

            ConversationEngine.ActivateNode(testConvo.CurrentNode);

            // Test that the file was written.
            var fileContents = File.ReadAllText(DialogueFiles.WriteValues).Trim();
            Assert.AreEqual(DialogueFiles.WriteKeyword, fileContents);

            File.Delete(DialogueFiles.WriteValues);
        }

        #endregion

        #region AreConditionsMet

        /// <summary>
        ///  Ensures that when conditions should be met, they are.
        /// </summary>
        [TestMethod]
        public void AreConditionsMet_TrueConditionsSucceeed()
        {
            // Load the conversation, and jump to a useful node for us to test.
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);
            testConvo.JumpToNode(DialogueFiles.SimpleNodeEligibleNode);

            // Test that conditions are met for the node, should be true.
            bool conditionsMet = ConversationEngine.AreConditionsMet(testConvo.CurrentNode);
            Assert.IsTrue(conditionsMet);
        }

        /// <summary>
        ///  Ensures that when conditions shouldn't be met, they aren't.
        /// </summary>
        [TestMethod]
        public void AreConditionsMet_FalseConditionsFail()
        {
            // Load the conversation, and jump to a useful node for us to test.
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);
            testConvo.JumpToNode(DialogueFiles.SimpleNodeNonEligibleNode);

            // Test that conditions are met for the node, should be false.
            bool conditionsMet = ConversationEngine.AreConditionsMet(testConvo.CurrentNode);
            Assert.IsFalse(conditionsMet);
        }

        /// <summary>
        ///  Ensures that when we're attempting the node, we'll update it to reflect it's been calculated.
        /// </summary>
        [TestMethod]
        public void AreConditionsMet_UpdatesNodeWhenAttempting()
        {
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);
            testConvo.JumpToNode(DialogueFiles.SimpleNodeNonEligibleNode);

            // Test that conditions are met for the node, should be false.
            ConversationEngine.AreConditionsMet(testConvo.CurrentNode, true);

            Assert.IsNotNull(testConvo.CurrentNode.IsEnabled);
            Assert.IsFalse(testConvo.CurrentNode.IsEnabled.Value);
        }

        /// <summary>
        ///  Ensures that when we aren't attempting to execute a node, we don't update it.
        /// </summary>
        [TestMethod]
        public void AreConditionsMet_DoesntUpdateNodeWhenNotAttempting()
        {
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);
            testConvo.JumpToNode(DialogueFiles.SimpleNodeNonEligibleNode);

            // Test that conditions are met for the node, should be false.
            ConversationEngine.AreConditionsMet(testConvo.CurrentNode);

            Assert.IsNull(testConvo.CurrentNode.IsEnabled);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  Loads a conversation from file
        /// </summary>
        /// <param name="fileName">The file name to load into the conversation.</param>
        /// <param name="testConvo">The conversation we're updating.</param>
        /// <returns>If the conversation load was successful.</returns>
        private bool LoadConversationFromFile(string fileName, ref Conversation testConvo)
        {
            return testConvo.LoadConversation(File.ReadAllText($"../../Files/{ fileName }"));
        }

        #endregion
    }
}
