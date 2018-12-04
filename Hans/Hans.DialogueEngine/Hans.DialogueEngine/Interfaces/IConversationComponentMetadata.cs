namespace Hans.DialogueEngine.Interfaces
{
    /// <summary>
    ///  Metadata class that holds information about a conversational method that needs to
    ///     be inherited in custom situations to move forward the conversation.
    /// </summary>
    public interface IConversationComponentMetadata
    {
        /// <summary>
        ///  Description of this component.  Is going to tie into any GUI that will be used in
        ///     creating these conversations, and is how we locate the component to run.
        /// </summary>
        string Description { get; }
    }
}
