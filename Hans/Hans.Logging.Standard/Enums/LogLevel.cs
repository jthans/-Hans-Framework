namespace Hans.Logging.Enums
{
    /// <summary>
    ///  Enumeration that defines the level of logging a particular message or configuration is associated with.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///  Most detailed level of logging.  Will show everything.
        /// </summary>
        Debug = 0,

        /// <summary>
        ///  Slightly more narrowing, won't include any logs that are typically used for sequence testing.
        /// </summary>
        Information = 1,

        /// <summary>
        ///  Only logs warnings or above, showing problems in the system.
        /// </summary>
        Warning = 2,

        /// <summary>
        ///  Logs only the app-breaking messages.
        /// </summary>
        Error = 3,

        /// <summary>
        ///  The app has been shut down, we'll only show the things that caused it.
        /// </summary>
        Fatal = 4
    }
}
