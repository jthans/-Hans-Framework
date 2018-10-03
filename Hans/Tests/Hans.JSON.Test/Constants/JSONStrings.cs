using Hans.JSON.Test.Models;

namespace Hans.JSON.Test.Constants
{
    /// <summary>
    ///  Collection of JSON strings to use in testing.  Kept here to share across tests.
    /// </summary>
    public class JSONStrings
    {
        /// <summary>
        ///  JSON string to be used in testing nested classes.
        /// </summary>
        public const string NestedJSONEntityString = "{ \"ValueOne\" : \"TestValueOne\", \"ValueTwo\" : \"7\", \"ValueThree\" : \"true\", \"NestedEntity\" : { \"ValueFour\" : \"TestValueFour\", \"ValueFive\" : 9 }}";

        /// <summary>
        ///  Valid JSON to be used testing <see cref="TestJSONEntity" />
        /// </summary>
        public const string ValidJSONEntityString = "{ \"ValueOne\" : \"TestValueOne\", \"ValueTwo\" : \"7\", \"ValueThree\" : \"true\" }";

        /// <summary>
        ///  These values are the valid values for the constant valid JSON above.
        /// </summary>
        public const string ValidValueOne = "TestValueOne";
        public const int ValidValueTwo = 7;
        public const bool ValidValueThree = true;
        public const string ValidValueFour = "TestValueFour";
        public const int ValidValueFive = 9;

        /// <summary>
        ///  Invalid JSON, won't even compile down structurally.
        /// </summary>
        public const string InvalidJSONEntityString = "{ { ValueOne\" : \"TestValueOne\", \"ValueTwo\" : \"7\", \"ValueThree\" : \"true\" }";
    }
}
