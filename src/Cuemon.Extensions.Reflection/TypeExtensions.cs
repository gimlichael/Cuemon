using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> cannot be null -or-
        /// <paramref name="name"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="match"/> was not in the range of valid values.
        /// </exception>
        public static IDictionary<string, Stream> GetEmbeddedResources(this Type source, string name, ManifestResourceMatch match)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return Decorator.Enclose(source.Assembly).GetManifestResources(name, match);
        }

        /// <summary>
        /// Retrieves a collection that represents all the properties defined on a specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to exclude properties on <paramref name="type"/>.</typeparam>
        /// <param name="type">The type that contains the properties to include except those defined on <typeparamref name="T"/>.</param>
        /// <returns>A collection of properties for the specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> cannot be null.
        /// </exception>
        public static IEnumerable<PropertyInfo> GetRuntimePropertiesExceptOf<T>(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return Decorator.Enclose(type).GetRuntimePropertiesExceptOf<T>();
        }

        /// <summary>
        /// Converts the <see cref="Type"/> to its equivalent string representation.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns>A string that contains the fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly.</returns>
        public static string ToFullNameIncludingAssemblyName(this Type type)
        {
            return FormattableString.Invariant($"{type.FullName}, {type.GetTypeInfo().Assembly.GetName().Name}");
        }
    }
}