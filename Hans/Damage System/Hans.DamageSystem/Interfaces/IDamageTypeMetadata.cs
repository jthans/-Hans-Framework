namespace Hans.DamageSystem.Interfaces
{
    /// <summary>
    ///  Metadata for determining which damage type a particular calculation needs to go to.
    /// </summary>
    public interface IDamageTypeMetadata
    {
        /// <summary>
        ///  The DamageType the calculation corresponds to.
        /// </summary>
        string DamageType { get; }
    }
}
