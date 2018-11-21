using System.Collections.Generic;

namespace Hans.DamageSystem.Interfaces
{
    /// <summary>
    ///  Interface that allows us to calculate certain damage effects based on an elemental "defense" stat, or modifiers.
    /// </summary>
    public interface IDamageTypeCalculator
    {
        /// <summary>
        ///  Calculates the damage changes, in relation to a particular damage type.  This gives a lot of freedom planning critical hits,
        ///     fire weaknesses in certain weather, etc.
        /// </summary>
        /// <param name="currDmg">The damage currently being tracked that will be applied to an entity.</param>
        Dictionary<string, decimal> CalculateDamageTypeEffect(Dictionary<string, decimal> currDmg);
    }
}
