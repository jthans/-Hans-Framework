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
        ///  Indicates if two given lists are equivalent in terms of a cycle - Takes into account order, though
        ///     perhaps rotated.
        /// </summary>
        /// <typeparam name="T">Type of list to compare.</typeparam>
        /// <param name="thisList">The list we're comparing.</param>
        /// <param name="otherList">The list we're comparing to.</param>
        /// <returns></returns>
        public static bool IsSameCycle<T>(this List<T> thisList, List<T> otherList)
        {
            // Different size lists can't be the exact same cycle.
            if (thisList.Count != otherList.Count)
            {
                return false;
            }

            bool cycleMatches = false;
            foreach (var matchingItem in otherList)
            {
                if (!matchingItem.Equals(thisList[0]))
                {
                    continue;
                }

                var itemIndex = otherList.IndexOf(matchingItem);
                for (var i = 0; i < thisList.Count; i++)
                {
                    // If the cycle doesn't match, break this loop.
                    if (!thisList[i].Equals(otherList[(itemIndex + i) % otherList.Count]))
                    {
                        break;
                    }

                    if (i == thisList.Count - 1)
                    {
                        cycleMatches = true;
                    }
                }

                if (cycleMatches)
                {
                    break;
                }
            }

            return cycleMatches;
        }

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
