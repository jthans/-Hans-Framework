using Hans.DependencyInjection;
using Hans.DialogueEngine.Test.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Hans.DialogueEngine.Test
{
    /// <summary>
    ///  Tests that revolve around the "conversation" object, and moving the conversation along.
    /// </summary>
    [TestClass]
    public class ConversationTest
    {
        #region Assembly Management

        /// <summary>
        ///  Initializes the assembly by building the DI framework container.
        /// </summary>
        /// <param name="context">Context giving explanation about the tests.</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            MEFBootstrapper.Build();
        }

        #endregion

        #region Events

        /// <summary>
        ///  Ensures the conversation complete event throws successfully.
        /// </summary>
        [TestMethod]
        public void ConversationComplete_EventFires()
        {
            Conversation testConvo = new Conversation();

            // Set up the event.
            bool eventFired = false;
            testConvo.ConversationComplete += (sender, e) => 
                                                {
                                                    eventFired = true;
                                                };

            // Load the conversation, and jump to the second to last node.
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);
            testConvo.JumpToNode(DialogueFiles.SimpleNodeMoveID);

            // Move to the final node, then confirm the event fired.
            bool moveSuccess = testConvo.MoveToNextNode(DialogueFiles.SimpleNodeExitConversation);
            Assert.IsTrue(eventFired);
        }

        #endregion

        #region MoveToNextNode

        /// <summary>
        ///  Tests that given a conversation, and a selected node, the conversation will choose the requested one. 
        /// </summary>
        [TestMethod]
        public void MoveToNextNode_ChoosesGivenNode()
        {
            // Loads the simple conversation from file.
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);

            // Move to the node, and ensure we did successfully.
            bool moveSuccess = testConvo.MoveToNextNode(DialogueFiles.SimpleNodeMoveID);

            Assert.IsTrue(moveSuccess);
            Assert.AreEqual(DialogueFiles.SimpleNodeMoveID, testConvo.CurrentNode.Id);
        }

        /// <summary>
        ///  Ensures that when given a mix of eligible and non-eligible nodes, the conversation will choose the eligible one.
        /// </summary>
        [TestMethod]
        public void MoveToNextNode_ChoosesEligibleNode()
        {
            // Loads the simple conversation from file.
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);

            // Jump to the important piece of the convo.
            testConvo.JumpToNode(DialogueFiles.SimpleNodeHigherPriorityID);

            // Move to the node, and ensure we did successfully.
            bool moveSuccess = testConvo.MoveToNextNode();

            Assert.IsTrue(moveSuccess);
            Assert.AreEqual(DialogueFiles.SimpleNodeEligibleNode, testConvo.CurrentNode.Id);
        }

        /// <summary>
        ///  Ensures that when multiple nodes are eligible, will choose the highest priority node.
        /// </summary>
        [TestMethod]
        public void MoveToNextNode_ChoosesHighestPriorityNode()
        {
            // Loads the simple conversation from file.
            Conversation testConvo = new Conversation();
            this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);

            // Jump to the important piece of the convo.
            testConvo.JumpToNode(DialogueFiles.SimpleNodeMoveID);

            // Move to the highest eligible node. For the node we're on, both should be available.
            bool moveSuccess = testConvo.MoveToNextNode();

            Assert.IsTrue(moveSuccess);
            Assert.AreEqual(testConvo.CurrentNode.Id, DialogueFiles.SimpleNodeHigherPriorityID);
        }

        #endregion

        #region LoadConversation

        /// <summary>
        ///  Ensures a valid conversation is loaded correctly into the conversation object.
        /// </summary>
        [TestMethod]
        public void LoadConversation_ConversationLoadsSuccessfully()
        {
            // Create the conversation, and load the conversation into the object.
            Conversation testConvo = new Conversation();
            bool convoLoaded = this.LoadConversationFromFile(DialogueFiles.SimpleConversation, ref testConvo);

            // Make sure it loaded the proper conversation, and that it did load nodes in, as they should exist.
            Assert.IsTrue(convoLoaded);
            Assert.AreEqual(testConvo.Name, DialogueFiles.SimpleConversationName);
            Assert.IsTrue(testConvo.Nodes.Count > 0);
        }

        /// <summary>
        ///  Ensures a conversation will not load if the validation conditions aren't met.
        /// </summary>
        [DataTestMethod]
        [DataRow(DialogueFiles.EmptyConversation)]
        [DataRow(DialogueFiles.MultipleInitialNodes)]
        [DataRow(DialogueFiles.DuplicateNodes)]
        [DataRow(DialogueFiles.NextNodeNotPresent)]
        public void LoadConversation_ConversationDoesNotLoadfailureConditions(string fileName)
        {
            // Create the conversation, and try to load the conversation into the object.
            Conversation testConvo = new Conversation();
            bool convoLoaded = this.LoadConversationFromFile(fileName, ref testConvo);

            Assert.IsFalse(convoLoaded);
            Assert.IsNull(testConvo.Name);
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
