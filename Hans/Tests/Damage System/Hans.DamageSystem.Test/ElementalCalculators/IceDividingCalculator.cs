using System.Collections.Generic;
using Hans.DamageSystem.Interfaces;
using System.ComponentModel.Composition;

namespace Hans.DamageSystem.Test.ElementalCalculators
{
    /// <summary>
    ///  Ice Dividing Calculator to run division on the ice number.
    /// </summary>
    [Export(typeof(IDamageTypeCalculator))]
    [ExportMetadata("DamageType", "Ice")]
    public class IceDividingCalculator : IDamageTypeCalculator
    {
        /// <summary>
        ///  Divides the ice effect by 4.
        /// </summary>
        /// <param name="currDmg">The damage coming into this dataset.</param>
        /// <returns>Resulting data.</returns>
        public Dictionary<string, decimal> CalculateDamageTypeEffect(Dictionary<string, decimal> currDmg)
        {
            var modifiedDmg = currDmg;
            modifiedDmg["Ice"] /= 4;

            return modifiedDmg;
        }
    }
}
