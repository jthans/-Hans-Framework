namespace Hans.JSON.Test.Models
{
    /// <summary>
    ///  Model used to test the loading of JSON Entities.
    ///     Testing string/int/bool to ensure we can load different types.
    /// </summary>
    public class TestJSONEntity : JSONEntity
    {
        #region Constructors 

        public TestJSONEntity()
            : base()
        {

        }

        public TestJSONEntity(string json)
            : base(json)
        {

        }

        #endregion

        #region Properties

        public string ValueOne { get; set; }

        public int ValueTwo { get; set; }

        public bool ValueThree { get; set; }

        public TestNestedEntity NestedEntity { get; set; }

        #endregion
    }
}
