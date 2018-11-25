using Hans.DamageSystem.Models;

namespace Hans.DamageSystem.Interfaces
{
    /// <summary>
    ///  Interface for the <see cref="DamageController{T}" /> - Used for testing purposes, so we can mock it out.
    /// </summary>
    public interface IDamageController<T>
    {
        DamageUnit ApplyDamage(string entityId, DamageUnit damageAmt);
    }
}