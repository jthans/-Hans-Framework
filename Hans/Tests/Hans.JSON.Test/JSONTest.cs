using Hans.DependencyInjection;
using Hans.JSON.Test.Constants;
using Hans.JSON.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Hans.JSON.Test
{
    /// <summary>
    ///  Test for the JSON static class that utilizes Newtonsoft to parse JSON strings and files.
    /// </summary>
    [TestClass]
    public class JSONTest
    {
        #region Assembly Management

        /// <summary>
        ///  Initializes the assembly by building the DI framework container.
        /// </summary>
        /// <param name="context">Context giving explanation about the tests.</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            MEFBootstrapper.Build();
        }

        #endregion

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

        #region Parse

        /// <summary>
        ///  Ensures that we can parse valid JSON.
        /// </summary>
        [TestMethod]
        public void Parse_ParsesValidString()
        {
            Assert.IsNotNull(JSON.Parse<TestJSONEntity>(JSONStrings.ValidJSONEntityString));
        }

        /// <summary>
        ///  If we pass an empty string, it's not really "invalid", so we'll return an empty object.
        /// </summary>
        [TestMethod]
        public void Parse_EmptyStringReturnsEmptyObject()
        {
            Assert.IsNull(JSON.Parse<TestJSONEntity>(string.Empty));
        }

        /// <summary>
        ///  Ensures that when we parse invalid JSON, we simply return null.
        /// </summary>
        [TestMethod]
        public void Parse_InvalidStringThrowsException()
        {
            Assert.ThrowsException<InvalidDataException>(() => 
            {
                JSON.Parse<TestJSONEntity>(JSONStrings.InvalidJSONEntityString);
            });
        }

        #endregion

        #region ParseFile

        /// <summary>
        ///  Ensures that ParseFile can parse valid JSON read from a file.
        /// </summary>
        [TestMethod]
        public void ParseFile_ParsesValidFile()
        {
            Assert.IsNotNull(JSON.ParseFile<TestJSONEntity>(JSONFiles.ValidFilePath));
        }

        /// <summary>
        ///  Ensures that when a file is not found, an exception is thrown since this is 
        ///     something the programmer needs to know.
        /// </summary>
        [TestMethod]
        public void ParseFile_ThrowsExceptionOnFileNotFound()
        {
            Assert.ThrowsException<FileNotFoundException>(() =>
            {
                JSON.ParseFile<TestJSONEntity>("FileDoesntExist.txt");
            });
        }

        #endregion
    }
}
