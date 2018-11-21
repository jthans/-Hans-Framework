using Hans.DamageSystem.Interfaces;
using Hans.DamageSystem.Models;
using Hans.Logging;
using Hans.Logging.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Hans.DamageSystem
{
    /// <summary>
    ///  Class responsible for calculating/holding a mapping from a model class, to stored methods/variables the damage system can understand.  This will allow us to accept any
    ///     DamageUnit model necessary for implementation.
    /// </summary>
    public class DamageMapper<T> : IDamageMapper<T>
        where T : DamageUnit
    {
        /// <summary>
        ///  Logger that is used to output any useful information happening here.
        /// </summary>
        private readonly ILogger log = LoggerManager.CreateLogger(typeof(DamageMapper<T>));

        #region Fields

        /// <summary>
        ///  Maps the model type to the object type, so that we don't need to use reflection each time, and store the property options in the class.
        /// </summary>
        private List<ModelPropertyConfig<T>> modelMapping;

        #endregion

        /// <summary>
        ///  Initializes a new instance of the <see cref="DamageMapper{T}" /> class, and maps the model to this instance.
        /// </summary>
        public DamageMapper()
        {
            this.MapDamageUnit();
        }

        #region Properties

        /// <summary>
        ///  Number of damage types being tracked by the controller, and the model registered.  Mainly for testing.
        /// </summary>
        public int NumDamageTypes { get { return modelMapping.Count; } }

        #endregion

        /// <summary>
        ///  Translates a damage unit class to a dictionary, so the damage controller can read any custom model.
        /// </summary>
        /// <param name="damageUnit">The damage unit to translate.</param>
        /// <returns>A dictionary representing key/damage connections.</returns>
        public Dictionary<string, decimal> Normalize(DamageUnit damageUnit)
        {
            // Return a dictionary of the property and its value.
            return modelMapping.ToDictionary(x => x.PropertyName, x => x.GetterMethod((T)damageUnit));
        }

        /// <summary>
        ///  Translates the standard dictionary tracker to the custom model this mapper tracks.
        /// </summary>
        /// <param name="damageTracker">The damage tracker we're modifying.</param>
        /// <returns>The custom model containing information about the damage.</returns>
        public T TranslateToModel(Dictionary<string, decimal> damageTracker)
        {
            // Create a damage unit, and store it as our custom type.
            T emptyDamageUnit = (T) Activator.CreateInstance(typeof(T));
            this.modelMapping.ForEach(x =>
            {
                // We don't need to modify a key here, the model doesn't have it.
                if (!damageTracker.ContainsKey(x.PropertyName))
                {
                    return;
                }

                x.SetterMethod(emptyDamageUnit, damageTracker[x.PropertyName]);
            });

            return emptyDamageUnit;
        }

        #region Internal Methods

        /// <summary>
        ///  Creates the getter for a given property, the "GET" method being found beforehand.  We'll cache these for easy use later.
        /// </summary>
        /// <typeparam name="T">The model type we're getting properties from.</typeparam>
        /// <param name="getMethod">The GET method to access a property's value.</param>
        /// <returns>Getter method to access a property in the model.</returns>
        private Func<T, decimal> CreateGetter(MethodInfo getMethod)
        {
            ParameterExpression unitInstance = Expression.Parameter(typeof(T), "instance");

            var methodBody = Expression.Call(unitInstance, getMethod);
            var methodParams = new ParameterExpression[] { unitInstance };

            return Expression.Lambda<Func<T, decimal>>(methodBody, methodParams).Compile();
        }

        /// <summary>
        ///  Creates the setter for a given property, the "SET" method being found beforehand.  We'll cache these for easy use later.
        /// </summary>
        /// <param name="setMethod">The SET method used to modify a model's property value.</param>
        /// <returns>Setter method used to modify a property in the model.</returns>
        private Action<T, decimal> CreateSetter(MethodInfo setMethod)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(decimal), "param");

            var body = Expression.Call(instance, setMethod, parameter);
            var parameters = new ParameterExpression[] { instance, parameter };

            return Expression.Lambda<Action<T, decimal>>(body, parameters).Compile();
        }

        /// <summary>
        ///  Maps the damage unit passed to this class to the controller, so the damage type can be used.
        ///     Note: Mapping performance enchancements (caching the get/set delegates) learned here: https://blogs.msmvps.com/jonskeet/2008/08/09/making-reflection-fly-and-exploring-delegates/
        /// </summary>
        /// <param name="objType">The type this controller was initialized with.</param>
        /// <returns>The number of properties mapped in the unit.</returns>
        private int MapDamageUnit()
        {
            var objType = typeof(T);

            this.log.LogMessage($"DamageMapper: Mapping Model { objType.Name } to DamageUnit.");
            this.modelMapping = new List<ModelPropertyConfig<T>>();

            // Get the properties, and add their information to our mapping.
            var modelProperties = objType.GetProperties();
            foreach (var prop in modelProperties)
            {
                // Get the get/set methods on this property.
                MethodInfo propGetter = objType.GetMethod($"get_{ prop.Name }");
                MethodInfo propSetter = objType.GetMethod($"set_{ prop.Name }");

                // Build the property profile, and add it to the global mapping.
                var propertyProfile = new ModelPropertyConfig<T>()
                {
                    PropertyName = prop.Name,
                    GetterMethod = this.CreateGetter(propGetter),
                    SetterMethod = this.CreateSetter(propSetter)
                };

                this.log.LogMessage($"DamageMapper: Mapped { prop.Name }.");
                this.modelMapping.Add(propertyProfile);
            }
            
            return this.modelMapping.Count;
        }

        #endregion
    }
}
