using System;
using System.Collections.Generic;
using System.Linq;

namespace Hans.Extensions
{
    /// <summary>
    ///  Collection of list extensions, to be run on the list object.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        ///  Returns a random entry from this list.
        /// </summary>
        /// <param name="thisList">The list to grab a random entry from.</param>
        /// <returns>A random entry.</returns>
        public static T GetRandomEntry<T>(this List<T> thisList)
        {
            // If there's no values, we really can't return an object.
            if (!thisList.Any())
            {
                return default(T);
            }

            System.Random randy = new Random();
            return thisList[randy.Next(thisList.Count)];
        }
    }
}
