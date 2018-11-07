using System;

namespace Hans.DamageSystem.Models
{
    /// <summary>
    ///  Properties of a model passed into the damage system, so we can map back to the user's model type.
    /// </summary>
    public class ModelPropertyConfig<T>
    {
        /// <summary>
        ///  Gets or sets the name of the property.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///  Gets or sets the getter for this property.
        /// </summary>
        public Func<T, decimal> GetterMethod { get; set; }

        /// <summary>
        ///  Gets or sets the setter for this property.
        /// </summary>
        public Action<T, decimal> SetterMethod { get; set; }
    }
}
