using System;
using System.Linq;

namespace Hans.Extensions
{
    /// <summary>
    ///  Extensions related to the integer data types.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        ///  Converts a string representation of an integer, to a string value.  This has been proven using benchmarks to be much faster than the given standards.
        /// </summary>
        /// <param name="intVal">The int data type.</param>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static bool ParseFromString(ref int intVal, string strVal)
        {
            // Ensure this is fully digits, so we can convert.
            if (!strVal.All(x => Char.IsDigit(x)))
            {
                intVal = 0;
                return false;
            }

            int y = 0;
            for (int i = 0; i < strVal.Length; i++)
                y = y * 10 + (strVal[i] - '0');

            intVal = y;
            return true;
        }
    }
}
