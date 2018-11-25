namespace Hans.DamageSystem.Models
{
    public class DamageOverTime
    {
        /// <summary>
        ///  How often this DOT executes on an entity.
        /// </summary>
        public decimal CycleTime { get; set; }

        /// <summary>
        ///  Entity that this DOT affects.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        ///  Time left on this DOT before it expires.
        /// </summary>
        public decimal RemainingTime { get; set; }
    }
}
