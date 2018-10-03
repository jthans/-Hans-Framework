using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

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
            // Create a new catalog for the directory this library is currently running within.  This makes it easy to pull out any unwanted exports in DLLs.
            var objCatalog = new AggregateCatalog();
            objCatalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            // Build the content necessary for this class.
            CompositionContainer compCntnr = new CompositionContainer(objCatalog);
            compCntnr.ComposeParts(this);
        }
    }
}
