using Hans.DependencyInjection.Test.Interfaces;
using System.ComponentModel.Composition;

namespace Hans.DependencyInjection.Test.Models
{
    /// <summary>
    ///  Implementation that will need to be picked up by MEF.
    /// </summary>
    [Export(typeof(ITestInterface))]
    [ExportMetadata("Value", "TestValue")]
    public class InterfaceImplementation : ITestInterface
    {
        /// <summary>
        ///  Returns a string to indicate this class is working.
        /// </summary>
        /// <returns></returns>
        public string GiveMeSomeString()
        {
            return "SUCCESS";
        }
    }
}
