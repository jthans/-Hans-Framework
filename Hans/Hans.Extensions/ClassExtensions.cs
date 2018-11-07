using Hans.Logging;
using Hans.Logging.Interfaces;
using System;
using System.Reflection;

namespace Hans.Extensions
{
    /// <summary>
    ///  Any extensions related to dealing with classes and their properties.
    /// </summary>
    public static class ClassExtensions
    {
        /// <summary>
        ///  The logger for this class that will send out important information.
        /// </summary>
        private static readonly ILogger log = LoggerManager.CreateLogger(typeof(ClassExtensions));

        /// <summary>
        ///  Clears all properties in an instance of an object. This can be done from within the class, rather than overwriting with a new object.
        /// </summary>
        /// <param name="instance">The instance to clear values from.</param>
        public static void ClearProperties(this object instance)
        {
            var propList = instance.GetType().GetProperties();
            foreach (var propToClear in propList)
            {
                // Determine what default value to set for this property - If it's a value, we need to create a new instance. Otherwise, we'll do null.
                object valToSet = null;
                if (propToClear.PropertyType.IsValueType)
                {
                    valToSet = Activator.CreateInstance(propToClear.PropertyType);
                }

                // Set the calculated value.
                propToClear.SetValue(instance, valToSet);
            }
        }

        /// <summary>
        ///  Copies the values of all visible properties from one class to another, if they're the same type.
        /// </summary>
        /// <param name="instance">The instance that we want the properties copied into.</param>
        /// <param name="otherInstance">The instance that we want the properties copied from.</param>
        /// <param name="propertiesToCopy">The descriptor of which properties to copy.  Public/Nonpublic/Etc.</param>
        public static void CopyPropertiesFromInstance(this object instance, object otherInstance)
        {
            var instanceType = instance.GetType();
            log.LogMessage($"Attempting to copy properties into an instance of type { instanceType }.");

            // If the instances don't match types, we can't copy properties across.
            var otherType = otherInstance.GetType();
            if (instanceType != otherType)
            {
                log.LogMessage($"Cannot copy properties from type { otherType } into type { instanceType }.", Logging.Enums.LogLevel.Fatal);
                throw new ReflectionTypeLoadException(new Type[] { instanceType, otherType }, null);
            }

            // If they do match, we need to copy the properties across.
            PropertyInfo[] myObjectProperties = instanceType.GetProperties();
            foreach (var propToCopy in myObjectProperties)
            {
                propToCopy.SetValue(instance, propToCopy.GetValue(otherInstance));
            }
        }

        /// <summary>
        ///  Creates a generic method from the class type this is called on.  We use this in events a bracketed method exists, e.g. MethodName<T>,
        ///     so that we can call a generic method easily.
        ///  Note - Considered invoking the method here as well, but figured then we couldn't manipulate the results of the method, since the type would change.
        /// </summary>
        /// <param name="class">The class that's being managed here.</param>
        /// <param name="methodName">The name of the method to create a generic method for.</param>
        /// <param name="callTypes">The types that we're calling the method with - This will be the T.</param>
        /// <returns>The generic method that's built, ready to invoke.</returns>
        public static MethodInfo MakeGenericMethodFromClass(this object @class, string methodName, params Type[] callTypes)
        {
            // Get the type of the class calling this.
            var classType = @class.GetType();
            log.LogMessage($"Creating generic method { methodName } using type { classType.AssemblyQualifiedName }.");

            // Generate the generic method, and return it to be used.
            MethodInfo callMethod = classType.GetMethod(methodName);
            if (callMethod == null)
            {
                // Method wasn't found on the class.
                throw new MethodAccessException($"The method { methodName } could not be found in the classtype { classType.ToString() }. Ensure the method exists, and is public.");
            }

            return callMethod.MakeGenericMethod(callTypes);
        }
    }
}
