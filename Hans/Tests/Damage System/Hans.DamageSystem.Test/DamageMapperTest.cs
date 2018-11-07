using Hans.DamageSystem.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual(5, normalizedResults["Ice"]);
            Assert.AreEqual(10, normalizedResults["Fire"]);
        }
    }
}
