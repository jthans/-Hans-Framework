namespace Hans.Logging.Attributes
{
    public class ExporterTypeAttribute: System.Attribute
    {
        /// <summary>
        ///  Type of Log Exporter the Class Refers To
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  Class that allows us to dynamically create log exporters as configured.
        /// </summary>
        /// <param name="type">Type of Exporter</param>
        public ExporterTypeAttribute(string type)
        {
            this.Type = type;
        }
    }
}
