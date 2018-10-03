using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hans.DependencyInjection
{
    public static class MEFBootstrapper
    {
        /// <summary>
        ///  Composition container to pull all exports for the registered interfaces, so we can grab objects that we need.
        /// </summary>
        private static CompositionContainer compositionContainer;

        /// <summary>
        ///  Instantiates a new instance of the <see cref="MEFBootstrapper" /> clas.  As this is a static constructor,
        ///     it will be run the first time the class is accessed.  It sets up the container to DI objects in.
        /// </summary>
        static MEFBootstrapper()
        {
            var objCatalog = new AggregateCatalog();
            objCatalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            // Initialize a new container looking at all exports in the given directory.
            compositionContainer = new CompositionContainer(objCatalog);
        }

        /// <summary>
        ///  Resolves all instances of the interface requested via the type parameter.
        /// </summary>
        /// <typeparam name="T">The type/interface that we're searching.</typeparam>
        /// <returns>A collection of all objects inheriting this interface.</returns>
        public static IEnumerable<T> ResolveMany<T>()
        {
            return compositionContainer.GetExports<T>().Select(x => x.Value);
        }

        /// <summary>
        ///  Resolves all instances of the interfaces requested, along with their metadata types.
        /// </summary>
        /// <typeparam name="T">The type/interface we're searching.</typeparam>
        /// <typeparam name="TMetaData">Metadata containing information relevant to the interface.</typeparam>
        /// <returns></returns>
        public static IEnumerable<Lazy<T, TMetaData>> ResolveManyWithMetaData<T, TMetaData>()
        {
            return compositionContainer.GetExports<T, TMetaData>();
        }
    }
}
