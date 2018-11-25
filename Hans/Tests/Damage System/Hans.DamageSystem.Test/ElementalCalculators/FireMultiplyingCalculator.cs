using System.Collections.Generic;
using Hans.DamageSystem.Interfaces;
using System.ComponentModel.Composition;

namespace Hans.DamageSystem.Test.ElementalCalculators
{
    /// <summary>
    ///  Testing calculator that will multiply fire damage by 3.
    /// </summary>
    [Export(typeof(IDamageTypeCalculator))]
    [ExportMetadata("DamageType", "Fire")]
    public class FireMultiplyingCalculator : IDamageTypeCalculator
    {
        /// <summary>
        ///  Calculates the damage effect for fire, specifically.
        /// </summary>
        /// <param name="currDmg">The damage that is incoming to this calculator.</param>
        /// <returns>Dictionary of values for this damage type.</returns>
        public Dictionary<string, decimal> CalculateDamageTypeEffect(Dictionary<string, decimal> currDmg)
        {
            var damageModified = currDmg;
            damageModified["Fire"] *= 3;

            return damageModified;
        }
    }
}
