using Hans.Extensions;
using Hans.Extensions.Test.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hans.Extensions.Test
{
    /// <summary>
    ///  Tests any extensions residing in this library for enumerations.
    /// </summary>
    [TestClass]
    public class EnumExtensionsTest
    {
        #region IsFlagSet

        /// <summary>
        ///  Ensures that when checking for existing flags in an enum, it returns true successfully.
        /// </summary>
        [TestMethod]
        public void IsFlagSet_ContainsFlagSuccessfully()
        {
            FlagEnumTest testEnum = (FlagEnumTest.FlagThree | FlagEnumTest.FlagTwo);

            // Asserts
            Assert.IsTrue(testEnum.IsFlagSet(FlagEnumTest.FlagTwo));
            Assert.IsTrue(testEnum.IsFlagSet(FlagEnumTest.FlagThree));
            Assert.IsTrue(testEnum.IsFlagSet((FlagEnumTest.FlagTwo | FlagEnumTest.FlagThree)));
        }

        /// <summary>
        ///  Ensures that when the flag doesn't exist in an enum, it returns false successfully.
        /// </summary>
        [TestMethod]
        public void IsFlagSet_DoesntContainFlagSuccessfully()
        {
            FlagEnumTest testEnum = (FlagEnumTest.FlagThree | FlagEnumTest.FlagTwo);

            Assert.IsFalse(testEnum.IsFlagSet(FlagEnumTest.FlagOne));
            Assert.IsFalse(testEnum.IsFlagSet(FlagEnumTest.FlagFour));
        }

        /// <summary>
        ///  Ensures that when an enum is calculated with a value of 0, we throw the correct exception.
        /// </summary>
        [TestMethod]
        public void IsFlagSet_ThrowsExceptionOnFlagValueZero()
        {
            FlagEnumTest testEnum = (FlagEnumTest.FlagThree | FlagEnumTest.FlagTwo);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testEnum.IsFlagSet(FlagEnumTest.Invalid));
        }

        #endregion
    }
}
