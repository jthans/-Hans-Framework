using System;

namespace Hans.DamageSystem.Models
{
    public class LayerMask
    {
        /// <summary>
        ///  The maximum amount of damage that will be mediated by this mask.
        /// </summary>
        public decimal DamageCap { get; set; }

        /// <summary>
        ///  The type of damage this mask is managing.
        /// </summary>
        public string DamageType { get; set; }

        /// <summary>
        ///  How long this mask lasts on the given entity.
        /// </summary>
        public decimal Duration { get; set; }

        /// <summary>
        ///  ID of this layer.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  How much the layer mask multiplies incoming damage.
        /// </summary>
        public decimal Multiplier { get; set; }

        /// <summary>
        ///  Instantiates a new instance of the <see cref="LayerMask" /> class.
        /// </summary>
        /// <param name="damageType">Type of damage that will be managed in this mask.</param>
        /// <param name="damageCap">Maximum damage that can be modified by this layer.</param>
        /// <param name="duration">How long this layer mask lasts.</param>
        /// <param name="multiplier">Multiplier on the damage coming through.</param>
        public LayerMask(string damageType,
                            decimal? damageCap,
                            decimal? duration,
                            decimal? multiplier)
        {
            this.DamageType = damageType;
            this.Id = Guid.NewGuid().ToString();

            this.DamageCap = damageCap ?? 0;
            this.Duration = duration ?? 0;
            this.Multiplier = multiplier ?? 1;
        }
    }
}
