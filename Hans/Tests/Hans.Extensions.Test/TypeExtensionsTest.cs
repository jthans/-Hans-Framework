using Hans.Extensions.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hans.Extensions.Test
{
    /// <summary>
    ///  Unit testing for any type-related calculations.
    /// </summary>
    [TestClass]
    public class TypeExtensionsTest
    {
        #region IsPrimitive

        /// <summary>
        ///  Ensures that IsPrimitive is correctly detecting types we'd expect to be primitive.
        /// </summary>
        /// <param name="testType">The type that's being tested.</param>
        [DataTestMethod]
        [DataRow(typeof(string))]
        [DataRow(typeof(int))]
        [DataRow(typeof(decimal))]
        public void IsPrimitive_DetectsPrimitivesSuccessfully(Type testType)
        {
            Assert.IsTrue(TypeExtensions.IsPrimitive(testType));
        }

        /// <summary>
        ///  Ensures that IsPrimitive correctly determines when a class is not a primitive.
        /// </summary>
        /// <param name="testType"></param>
        [DataTestMethod]
        [DataRow(typeof(TestClassAnimal))]
        public void IsPrimitive_FailsNonPrimitivesSuccessfully(Type testType)
        {
            Assert.IsFalse(TypeExtensions.IsPrimitive(testType));
        }

        #endregion
    }
}
