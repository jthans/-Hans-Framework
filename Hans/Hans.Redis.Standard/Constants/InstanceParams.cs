namespace Hans.Redis.Constants
{
    /// <summary>
    ///  Constants related to starting instances of Redis locally within an application.
    /// </summary>
    public class InstanceParams
    {
        /// <summary>
        ///  File name of the Redis Windows config.
        /// </summary>
        public const string ConfigFileName = "redis.windows.conf";

        /// <summary>
        ///  Default port number for Redis, if none was specified.
        /// </summary>
        public const int DefaultPortNumber = 6379;

        /// <summary>
        ///  Filepath to the Redis version folders.
        /// </summary>
        public const string FilePath = "Redis/";

        /// <summary>
        ///  Name of the server executable.
        /// </summary>
        public const string ServerExe = "redis-server.exe";

        /// <summary>
        ///  Default version number to use, if none is specified.
        /// </summary>
        public const string VerNumber = "3.0";
    }
}
