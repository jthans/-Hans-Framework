using System;

namespace Hans.DamageSystem.Events
{
    /// <summary>
    ///  Event args that give information about an entity's death, for anything that would like to consume it.
    /// </summary>
    public class EntityDeathEventArgs : EventArgs
    {
        /// <summary>
        ///  ID of the entity that has perished.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        ///  Instantiates a new instance of the <see cref="EntityDeathEventArgs" /> class, which gives
        ///     useful information about an entity's death.
        /// </summary>
        /// <param name="entityId">The entity that has passed.</param>
        public EntityDeathEventArgs(string entityId)
        {
            this.EntityId = entityId;
        }
    }
}
