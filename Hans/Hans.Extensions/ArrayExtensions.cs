using System;
using System.Collections.Generic;

namespace Hans.Extensions
{
    /// <summary>
    ///  Extensions related to arrays, and any operations that need to be done on them.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        ///  Concatenates a sequence of T and T[] objects, to generate an array from a full
        ///     set of values.
        /// </summary>
        /// <typeparam name="T">Type T being concatenated.</typeparam>
        /// <param name="concatObj">The parameters that we want concatenated into a single array.</param>
        /// <returns>An array containing all passed elements.</returns>
        /// <exception cref="TypeLoadException">Thrown when an object that is not the given type is passed.</exception>
        public static T[] Concatenate<T>(params object[] concatObj)
        {
            // Create a list, and add to it as necessary.
            List<T> concatList = new List<T>();
            foreach (var obj in concatObj)
            {
                // For arrays, we add ranges.
                if (obj.GetType().IsArray)
                {
                    concatList.AddRange(obj as T[]);
                }
                else if (obj.GetType() == typeof(T))
                {
                    concatList.Add((T) obj);
                }
                else if (typeof(T) == typeof(string))
                {
                    // We'll assume we can always convert to a string, and display in the array.
                    concatList.Add((T)(object)obj.ToString());
                }
                else
                {
                    throw new TypeLoadException();
                }
            }

            return concatList.ToArray();
        }
    }
}
