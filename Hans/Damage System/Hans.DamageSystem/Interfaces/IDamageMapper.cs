using Hans.DamageSystem.Models;
using System.Collections.Generic;

namespace Hans.DamageSystem.Interfaces
{
    public interface IDamageMapper<T>
    {
        /// <summary>
        ///  Translates a damage unit class to a dictionary, so the damage controller can read any custom model.
        /// </summary>
        /// <param name="damageUnit">The damage unit to translate.</param>
        /// <returns>A dictionary representing key/damage connections.</returns>
        Dictionary<string, decimal> Normalize(DamageUnit damageUnit);

        /// <summary>
        ///  Translates the standard dictionary tracker to the custom model this mapper tracks.
        /// </summary>
        /// <param name="damageTracker">The damage tracker we're modifying.</param>
        /// <returns>The custom model containing information about the damage.</returns>
        T TranslateToModel(Dictionary<string, decimal> damageTracker);
    }
}
