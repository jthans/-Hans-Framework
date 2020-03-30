using System;

namespace Hans.Extensions
{
    /// <summary>
    ///  Extensions related to enumerations used in C# projects.  These allow simpler manipulation
    ///     of enums, especially related to ones marked with the <see cref="System.FlagsAttribute" />, as 
    ///     they can cause more confusion in libraries occasionally.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///  Indicates if an enumeration has a particular flag, or not.
        ///     NOTE: Yes, HasFlag exists, but it can't be used currently in the Unity Engine so we'll write this for our use.
        /// </summary>
        /// <param name="inputEnum">The enumeration value we're checking.</param>
        /// <param name="flagVal">The value to check for, in the enumeration.</param>
        /// <returns>If the flag is set to true in the enum.</returns>
        public static bool IsFlagSet(this Enum inputEnum, Enum flagVal)
        {
            var inputValues = Convert.ToUInt16(inputEnum);
            var flagValues = Convert.ToUInt16(flagVal);

            // We don't want to have values of 0 as a flag value.  It makes it WAY too hard to determine bitwise operations.
            if (flagValues == 0)
            {
                throw new ArgumentOutOfRangeException("Enums with the [Flags] attribute cannot have values of 0 as a value!");
            }

            // See if narrowing the input to just the flag returns the same values as the flag.
            return (inputValues & flagValues) == flagValues;
        }
    }
}
