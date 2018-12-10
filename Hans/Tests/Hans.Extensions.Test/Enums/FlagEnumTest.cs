using System;

namespace Hans.Extensions.Test.Enums
{
    /// <summary>
    ///  Enum Test that Contains Flags.
    /// </summary>
    [Flags]
    public enum FlagEnumTest
    {
        Invalid = 0,

        FlagOne = 1,

        FlagTwo = 2,

        FlagThree = 4,

        FlagFour = 8
    }
}
