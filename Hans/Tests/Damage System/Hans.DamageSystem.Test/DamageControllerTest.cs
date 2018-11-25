using Hans.DamageSystem.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Hans.DamageSystem.Test
{
    /// <summary>
    ///  Tests that ensure the proper working order of the <see cref="DamageController{T}" /> class.
    /// </summary>
    [TestClass]
    public class DamageControllerTest
    {
        /// <summary>
        ///  Ensures that the elemental calculators are successfully picked up, and parse the damages correctly.
        /// </summary>
        [TestMethod]
        public void DamageController_ElementalCalculatorsParseSuccessfully()
        {
            // Test Data
            Dictionary<string, decimal> testDamageData = new Dictionary<string, decimal>()
            {
                { "BaseHealth", 100 },
                { "Fire", 50 },
                { "Ice", 200 }
            };

            // Run the damage effects, and compare the results to those expected.
            DamageController<DamageUnitTest> testController = new DamageController<DamageUnitTest>();
            var numCalc = testController.CalculateDamageTypeEffects(ref testDamageData);

            // Assert our expectations.
            Assert.AreEqual(2, numCalc);
            Assert.AreEqual(100, testDamageData["BaseHealth"]);
            Assert.AreEqual(150, testDamageData["Fire"]);
            Assert.AreEqual(50,  testDamageData["Ice"]);
        }

        /// <summary>
        ///  Ensures layer calculations are happening correctly, and the controller is interacting with the manager successfully.
        ///     While the manager for testing is very simplistic, we'll ensure the controller is calling properly, meaning any problems
        ///     are occurring in the manager.
        /// </summary>
        [TestMethod]
        public void DamageController_LayersParseSuccessfully()
        {
            // Test Values
            const string entityId = "TestEntity";
            Dictionary<string, decimal> testDamageData = new Dictionary<string, decimal>()
            {
                { "BaseHealth", 100 },
                { "Fire", 100 },
                { "Ice", 100 }
            };

            // Setup the test controller.
            DamageController<DamageUnitTest> testController = new DamageController<DamageUnitTest>();
            testController.AddLayer(entityId, "Fire", 300, (decimal) 0.5, null);
            testController.AddLayer(entityId, "Ice", 100, 2, null);

            // Execute the calcluations, and compare to expected.
            var dmgResult = testController.CalculateLayerEffects(entityId, ref testDamageData);

            Assert.AreEqual(2, dmgResult);
            Assert.AreEqual(50, testDamageData["Fire"]);
            Assert.AreEqual(200, testDamageData["Ice"]);

            // Make sure the manager was updated correctly, as the manager controls this.
            var entityLayers = testController.DamageManager.GetLayersForEntity(entityId);

            Assert.AreEqual(1, entityLayers.Length);
            Assert.AreEqual(200, entityLayers[0].DamageCap);
            Assert.AreEqual("Fire", entityLayers[0].DamageType);
        }
    }
}
