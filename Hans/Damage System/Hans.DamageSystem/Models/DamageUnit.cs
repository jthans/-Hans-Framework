using Hans.DamageSystem.Interfaces;

namespace Hans.DamageSystem.Models
{
    /// <summary>
    ///  DamageUnit class that represents a set of damage that can befall an entity.
    /// </summary>
    public class DamageUnit
    {
        /// <summary>
        ///  Gets or sets the base, no specializations health.
        /// </summary>
        public decimal BaseHealth { get; set; }
    }
}
