using Hans.JSON.Test.Constants;
using Hans.JSON.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Hans.JSON.Test
{
    /// <summary>
    ///  Test that JSON Entities successfully load, or don't load in the circumstances that they shouldn't.
    /// </summary>
    [TestClass]
    public class JSONEntityTest
    {
        #region Test Management

        /// <summary>
        ///  Intializes the tests by adding the JSON files necessary.
        /// </summary>
        [TestInitialize]
        public void Intialize()
        {
            // Create Valid/Invalid files to read from.
            File.WriteAllText(JSONFiles.ValidFilePath, JSONStrings.ValidJSONEntityString);
            File.WriteAllText(JSONFiles.InvalidFilePath, JSONStrings.InvalidJSONEntityString);
        }

        /// <summary>
        ///  Cleans up the tests by deleting the files we were using.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Delete the JSON files created for the tests.
            File.Delete(JSONFiles.ValidFilePath);
            File.Delete(JSONFiles.InvalidFilePath);
        }

        #endregion

        #region Load

        /// <summary>
        ///  Ensures that we can load a JSON string into this entity, if the entities were to match.
        ///     This test uses the constructor to load JSON in.
        /// </summary>
        [TestMethod]
        public void Load_LoadsValidJSONIntoEntityUsingConstructor()
        {
            TestJSONEntity testEntity = new TestJSONEntity(JSONStrings.ValidJSONEntityString);

            Assert.AreEqual(testEntity.ValueOne, JSONStrings.ValidValueOne);
            Assert.AreEqual(testEntity.ValueTwo, JSONStrings.ValidValueTwo);
            Assert.AreEqual(testEntity.ValueThree, JSONStrings.ValidValueThree);
        }

        /// <summary>
        ///  Ensures that we can load a JSON string into this entity, if the entities were to match.
        ///     This test uses the Load method to load JSON in.
        /// </summary>
        [TestMethod]
        public void Load_LoadsValidJSONIntoEntityUsingMethod()
        {
            TestJSONEntity testEntity = new TestJSONEntity();
            testEntity.Load(JSONStrings.ValidJSONEntityString);

            Assert.AreEqual(testEntity.ValueOne, JSONStrings.ValidValueOne);
            Assert.AreEqual(testEntity.ValueTwo, JSONStrings.ValidValueTwo);
            Assert.AreEqual(testEntity.ValueThree, JSONStrings.ValidValueThree);
        }

        /// <summary>
        ///  Ensures that we can load a nested JSON entity successfully with a custom class.
        /// </summary>
        [TestMethod]
        public void Load_LoadsNestedJSONEntitySuccessfully()
        {
            TestJSONEntity testEntity = new TestJSONEntity();
            testEntity.Load(JSONStrings.NestedJSONEntityString);

            Assert.IsNotNull(testEntity.NestedEntity);
            Assert.AreEqual(testEntity.NestedEntity.ValueFour, JSONStrings.ValidValueFour);
            Assert.AreEqual(testEntity.NestedEntity.ValueFive, JSONStrings.ValidValueFive);
        }

        #endregion
    }
}
