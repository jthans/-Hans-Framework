using Hans.Extensions;
using Hans.Logging;
using Hans.Logging.Interfaces;
using System;
using System.IO;

namespace Hans.JSON
{
    /// <summary>
    ///  Class that all JSON entities will inherit from.  THis containers any method that all entities have access to.
    /// </summary>
    public class JSONEntity
    {
        #region Fields

        /// <summary>
        ///  The logger for this entity.
        /// </summary>
        protected ILogger log;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initiantiates a new instance of the <see cref="JSONEntity" /> class, or the type inheriting it.
        /// </summary>
        public JSONEntity()
        {
            this.Initialize();
        }

        /// <summary>
        ///  Instantiates a new instance of the <see cref="JSONEntity" /> class, or inheriting type while loading the JSON string passed.
        /// </summary>
        /// <param name="json">The JSON string to parse into this object.</param>
        public JSONEntity(string json) 
        {
            this.Initialize();
            this.Load(json);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Loads a JSON string into this entity.
        /// </summary>
        /// <param name="jsonString">The string containing the JSON information to load.</param>
        public void Load(string jsonString)
        {
            // Generate a version of the generic method that will parse the JSON string to this class's type. Then get the calculated object.
            var parseMethod = this.MakeGenericMethodFromClass("ParseJSON", this.GetType());
            var methodResult = parseMethod.Invoke(this, new object[] { jsonString });

            // If nothing could be loaded, say so.  Otherwise, load the properties into this instance.
            if (methodResult == null)
            {
                log.LogMessage($"JSON object could not be constructed from string { jsonString }. Object will be empty.", Logging.Enums.LogLevel.Warning);
                return;
            }

            // Copy the properties from the translated instance to this one.
            this.CopyPropertiesFromInstance(methodResult);
        }

        /// <summary>
        ///  Loads a file in order to parse the JSON into this entity.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        public void LoadFile(string filePath)
        {
            try
            {
                this.Load(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                log.LogMessage($"Error reading file { filePath } when creating JSON entity. Ex: { ex.ToString() }", Logging.Enums.LogLevel.Error);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  Initializes the entity by generating any objects that need to exist already.
        /// </summary>
        private void Initialize()
        {
            this.log = LoggerManager.CreateLogger(this.GetType()); // Pull the logger.
        }

        /// <summary>
        ///  Parses a JSON string into this object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public T ParseJSON<T>(string jsonString)
        {
            return JSON.Parse<T>(jsonString);
        }

        #endregion
    }
}
