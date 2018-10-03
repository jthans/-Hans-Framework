using Hans.DependencyInjection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hans.DependencyInjection.Test
{
    /// <summary>
    ///  Class used to test import capabilities in an MEFObject.
    /// </summary>
    [TestClass]
    public class MEFObjectTest
    {
        /// <summary>
        ///  Ensures MEF is loading dependencies successfully.
        /// </summary>
        [TestMethod]
        public void DependenciesLoad()
        {
            TestMEFObject testObj = new TestMEFObject();

            // We'll take the first implementation and run it, making sure we found one.
            foreach (var foundImplementation in testObj.implementations)
            {
                string strResult = foundImplementation.Value.GiveMeSomeString();
                Assert.IsNotNull(strResult);

                break;
            }
        }
    }
}
