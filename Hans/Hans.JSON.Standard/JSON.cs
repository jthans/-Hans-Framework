using Newtonsoft.Json;
using System.IO;

namespace Hans.JSON
{
    /// <summary>
    ///  Base class for all entities derived from it.  Gives a universal ability to serialize/deserialize them into or to JSON.  Also
    ///     includes any methods to save to files, streams, etc.
    /// </summary>
    public static class JSON
    {
        /// <summary>
        ///  Opens a file, and parses it into the requested JSON object.
        /// </summary>
        /// <typeparam name="T">T derived from <see cref="JSONEntity" /></typeparam>
        /// <param name="fileName">Name of the file to open and parse into a JSON object.</param>
        /// <returns>The parsed object if successfully parsed, <code>null</code> otherwise.</returns>
        public static T ParseFile<T>(string fileName)
        {
            string jsonContents = File.ReadAllText(fileName);
            return JSON.Parse<T>(jsonContents);
        }

        /// <summary>
        ///  Loads a JSON string into this entity, using the values provided.
        /// </summary>
        /// <param name="json">JSON to parse into this object.</param>
        /// <returns>If the loading was successful.</returns>
        public static T Parse<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) { return default(T); }
            try
            {
                // Deserialize to this object type, and return it if it parsed succesfully.
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonReaderException)
            {
                throw new InvalidDataException("JSON not in proper format.");
            }
        }
    }
}
