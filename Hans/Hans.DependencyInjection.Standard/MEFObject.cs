namespace Hans.DependencyInjection
{
    /// <summary>
    ///  Class used to inherit from when dependency injection is needed to import instances of a particular class.
    /// </summary>
    public class MEFObject
    {
        /// <summary>
        ///  Initializees a new instance of the <see cref="MEFObject" /> class.
        /// </summary>
        public MEFObject()
        {
            this.InitializeDependencies();
        }

        /// <summary>
        ///  Intiializes dependencies by calling the catalog for all objects requested in this class,
        ///     importing them from their various locations.
        /// </summary>
        private void InitializeDependencies()
        {
            MEFBootstrapper.ResolveDependencies(this);
        }
    }
}
