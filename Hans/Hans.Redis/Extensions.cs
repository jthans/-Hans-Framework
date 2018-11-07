using Hans.Logging.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TeamDev.Redis;

namespace Hans.Redis
{
    /// <summary>
    ///  Extensions necessary for the Redis library, to alow expansion on existing libraries for
    ///     repeated use.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///  Executes a Redis command, when requested.
        /// </summary>
        /// <param name="redisDap">DAO that will communicate with the cache.</param>
        /// <param name="log">Logger to output information about what's occurring.</param>
        /// <param name="cmdToExecute">The command we're executing.</param>
        /// <param name="paramArgs">The parameters to send with the command.</param>
        public static void ExecuteRedisCommand(this RedisDataAccessProvider redisDap, ILogger log, RedisCommand cmdToExecute, params string[] paramArgs)
        {
            log.LogRedisMessage(cmdToExecute, paramArgs);
            redisDap.SendCommand(cmdToExecute, paramArgs);
        }

        /// <summary>
        ///  Returns an argument list as a string, separated by the space character.
        /// </summary>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static string[] GetArgumentListAsStringArray(Dictionary<string, string> argumentList)
        {
            List<string> paramList = new List<string>();
            foreach (var argItem in argumentList)
            {
                paramList.Add(argItem.Key);
                paramList.Add(argItem.Value);
            }

            return paramList.ToArray();
        }

        /// <summary>
        ///  Logs a Redis message, wrapping it in a standard result to make filtering easy, keeping the same format.
        ///     TODO: Eventually disable Redis logging with config/code control - Will get overwhelming if a system is working.
        /// </summary>
        /// <param name="logger">Logger that will handle the log.</param>
        /// <param name="cmdType">The type of command being logged.</param>
        /// <param name="argumentList">List of arguments/values being passed in the command.</param>
        public static void LogRedisMessage(this ILogger logger, RedisCommand cmdType, params string[] arguments)
        {
            logger.LogMessage($"Redis: { cmdType.ToString() } Command w/ Params: { string.Join(" ", arguments) }");
        }
    }
}
