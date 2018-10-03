namespace Hans.DependencyInjection.Test.Interfaces
{
    /// <summary>
    ///  Test interface that will be imported by the unit tests.
    /// </summary>
    public interface ITestInterface
    {
        /// <summary>
        ///  Method that returns some string.
        /// </summary>
        /// <returns>Some string that will be evaulated.</returns>
        string GiveMeSomeString();
    }
}
