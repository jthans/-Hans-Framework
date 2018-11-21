using Hans.DamageSystem.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Hans.DamageSystem.Test
{
    /// <summary>
    ///  Ensures the damage mapper handles the mapping correctly, and can perform as expected when models are registered.
    /// </summary>
    [TestClass]
    public class DamageMapperTest
    {
        /// <summary>
        ///  Ensures that a DamageUnit class can be mapped by the mapper correctly.
        /// </summary>
        [TestMethod]
        public void DamageMapper_MapsDamageUnitSuccessfully()
        {
            // Create the mapper, and tell it to map our model.
            DamageMapper<DamageUnitTest> testMapper = new DamageMapper<DamageUnitTest>();
            Assert.AreEqual(3, testMapper.NumDamageTypes);
        }

        /// <summary>
        ///  Ensures that when we're copying a model INTO the mapper it loads values correctly.
        /// </summary>
        [TestMethod]
        public void DamageMapper_MapsGettersSuccessfully()
        {
            // Build the test unit.
            DamageMapper<DamageUnitTest> testMapper = new DamageMapper<DamageUnitTest>();
            DamageUnitTest testUnit = new DamageUnitTest()
            {
                BaseHealth = 100,
                Ice = 5,
                Fire = 10
            };

            var normalizedResults = testMapper.Normalize(testUnit);

            Assert.AreEqual(100, normalizedResults["BaseHealth"]);
            Assert.AreEqual(  5, normalizedResults["Ice"]);
            Assert.AreEqual( 10, normalizedResults["Fire"]);
        }

        /// <summary>
        ///  Ensures that when we're copying a model OUT of the mapper, it loads the values correctly.
        /// </summary>
        [TestMethod]
        public void DamageMapper_MapsSettersSuccessfully()
        {
            // Build the test unit.
            DamageMapper<DamageUnitTest> testMapper = new DamageMapper<DamageUnitTest>();
            Dictionary<string, decimal> testResults = new Dictionary<string, decimal>()
            {
                { "BaseHealth", 63 },
                { "Ice", 1 },
                { "Fire", 4 }
            };

            var translatedResults = testMapper.TranslateToModel(testResults);

            Assert.AreEqual(63, translatedResults.BaseHealth);
            Assert.AreEqual( 1, translatedResults.Ice);
            Assert.AreEqual( 4, translatedResults.Fire);
        }
    }
}
