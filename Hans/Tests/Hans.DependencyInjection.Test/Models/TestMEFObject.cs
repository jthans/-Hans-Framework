using Hans.DependencyInjection.Test.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Hans.DependencyInjection.Test.Models
{
    /// <summary>
    ///  Test MEFObject that will be used to ensure we can import DI'd interfaces.
    /// </summary>
    public class TestMEFObject : MEFObject
    {
        /// <summary>
        ///  Implementations of the <see cref="ITestInterface" /> interface, to ensure MEF is working.
        /// </summary>
        [ImportMany(typeof(ITestInterface))]
        public IEnumerable<Lazy<ITestInterface>> implementations;

        /// <summary>
        ///  Instantiates a new object, importing the implementations given.
        /// </summary>
        public TestMEFObject()
            : base()
        {

        }
    }
}
