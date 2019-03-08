using Hans.DependencyInjection.Test.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Hans.DependencyInjection.Test
{
    /// <summary>
    ///  Test for the MEFBoostrapper class that's used to resolve particular
    ///     interface types. In most cases, this class is used.
    /// </summary>
    [TestClass]
    public class MEFBootstrapperTest
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

        /// <summary>
        ///  Ensures that the bootstrapper can resolve an interface.
        /// </summary>
        [TestMethod]
        public void ResolveMany_ResolvesInterface()
        {
            // Get all instances of the ITestInterface class. Ensure there's more than one found.
            var allTestInterfaces = MEFBootstrapper.ResolveMany<ITestInterface>();
            Assert.AreNotEqual<int>(allTestInterfaces.Count(), 0);
        }

        /// <summary>
        ///  Ensures that the bootsrapper can resolve an interface, with its metadata.
        /// </summary>
        [TestMethod]
        public void ResolveManyWithMetaData_ResolvesInterfaceAndMetadata()
        {
            // Get all instances, with the metadata, and ensure the metadata has values.
            var allTestInterfaces = MEFBootstrapper.ResolveManyWithMetaData<ITestInterface, ITestMetadata>();
            Assert.AreNotEqual<int>(allTestInterfaces.Count(), 0);
            Assert.AreEqual(allTestInterfaces.FirstOrDefault().Metadata.Value, "TestValue");
        }
    }
}
