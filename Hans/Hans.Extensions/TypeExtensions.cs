using System;

namespace Hans.Extensions
{
    /// <summary>
    ///  Extensions related to types and type variables.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///  Determines if a type is "primitive", meaning in most terms if the object is custom by the user or not.  We include even the more  
        ///     complex types, such as <see cref="string" />.
        /// </summary>
        /// <param name="type">The type to test for primitivity.</param>
        /// <returns>If the value is a primitive object or not.</returns>
        /// <remarks>This is based off an answer on StackOverflow: https://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive</remarks>
        public static bool IsPrimitive(Type type)
        {
            // Primitive: Covers Most Cases.
            // ValueType: Covers Decimal, and similar.
            // String: String is not technically a primitive, but we treat it as one in most cases.
            return type.IsPrimitive || type.IsValueType || type == typeof(string);
        }
    }
}
