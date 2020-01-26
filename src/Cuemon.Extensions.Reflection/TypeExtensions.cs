using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
       /// <summary>
        /// Loads the embedded resources from the associated <see cref="Assembly"/> of the specified <see cref="Type"/> following the <see cref="ManifestResourceMatch"/> ruleset of <paramref name="match"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The name of the resource being requested.</param>
        /// <param name="match">The match ruleset to apply.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resources; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static IDictionary<string, Stream> GetEmbeddedResources(this Type source, string name, ManifestResourceMatch match)
        {
            return AssemblyInsight.FromType(source).GetManifestResources(name, match);
        }
    }
}