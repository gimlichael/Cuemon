using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provides a set of methods for working with reflection related operations on a <see cref="Assembly"/> object in a robust way.
    /// </summary>
    public class AssemblyInsight
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Assembly"/> to <see cref="AssemblyInsight"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to convert.</param>
        /// <returns>A <see cref="AssemblyInsight"/> that is equivalent to <paramref name="assembly"/>.</returns>
        public static implicit operator AssemblyInsight(Assembly assembly)
        {
            return new AssemblyInsight(assembly);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="AssemblyInsight"/> to <see cref="Assembly"/>.
        /// </summary>
        /// <param name="insight">The <see cref="AssemblyInsight"/> to convert.</param>
        /// <returns>A <see cref="Assembly"/> that is equivalent to <paramref name="insight"/>.</returns>
        public static implicit operator Assembly(AssemblyInsight insight)
        {
            return insight._assembly;
        }

        /// <summary>
        /// Creates a new instance of <see cref="AssemblyInsight"/> from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <returns>An instance of <see cref="AssemblyInsight"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembly"/> cannot be null.
        /// </exception>
        public static AssemblyInsight FromAssembly(Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            return assembly;
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="AssemblyInsight"/> from the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Type"/>.</typeparam>
        /// <returns>An instance of <see cref="AssemblyInsight"/>.</returns>
        public static AssemblyInsight FromType<T>()
        {
            return FromType(typeof(T));
        }

        /// <summary>
        /// Creates a new instance of <see cref="AssemblyInsight"/> from the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The underlying <see cref="Type"/>.</param>
        /// <returns>An instance of <see cref="AssemblyInsight"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> cannot be null.
        /// </exception>
        public static AssemblyInsight FromType(Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return type.Assembly;
        }

        AssemblyInsight(Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            _assembly = assembly;
        }

        /// <summary>
        /// Loads the embedded resources from the underlying <see cref="Assembly"/> of this instance.
        /// </summary>
        /// <param name="name">The case-sensitive name of the resource being requested.</param>
        /// <param name="match">The ruleset that defines the match to apply.</param>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> that contains the result of <paramref name="match"/>.</returns>
        /// <remarks>The result returned can have null values if no resources were specified during compilation or if the resource is not visible to the caller.</remarks>
        /// <seealso cref="Assembly.GetManifestResourceStream(string)"/>
        /// <seealso cref="Assembly.GetManifestResourceNames()"/>
        public IDictionary<string, Stream> GetManifestResources(string name, ManifestResourceMatch match = default)
        {
            Validator.ThrowIfNullOrWhitespace(name, nameof(name));
            var resources = new Dictionary<string, Stream>();
            switch (match)
            {
                case ManifestResourceMatch.Name:
                    resources.Add(name, _assembly.GetManifestResourceStream(name));
                    break;
                default:
                    var resourceNames = _assembly.GetManifestResourceNames();
                    var matchExtension = Path.GetExtension(name)?.ToUpperInvariant();
                    switch (match)
                    {
                        case ManifestResourceMatch.ContainsName:
                            foreach (var resourceName in resourceNames)
                            {
                                if (resourceName.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    resources.Add(resourceName, _assembly.GetManifestResourceStream(resourceName));
                                }
                            }
                            break;
                        case ManifestResourceMatch.Extension:
                            foreach (var resourceName in resourceNames)
                            {
                                var extension = Path.GetExtension(resourceName).ToUpperInvariant();
                                if (extension == matchExtension)
                                {
                                    resources.Add(resourceName, _assembly.GetManifestResourceStream(resourceName));
                                }
                            }
                            break;
                        case ManifestResourceMatch.ContainsExtension:
                            if (matchExtension == null) { throw new ArgumentException($"Unable to match by {ManifestResourceMatch.ContainsExtension} since no extension was specified.", nameof(name)); }
                            foreach (var resourceName in resourceNames)
                            {
                                var extension = Path.GetExtension(resourceName);
                                if (extension.IndexOf(matchExtension, StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    resources.Add(resourceName, _assembly.GetManifestResourceStream(resourceName));
                                }
                            }
                            break;
                        default:
                            throw new InvalidEnumArgumentException(nameof(match), (int)match, typeof(ManifestResourceMatch));
                    }
                    break;
            }
            return resources;
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the <see cref="AssemblyVersionAttribute"/> of the underlying <see cref="Assembly"/> of this instance.
        /// </summary>
        /// <returns>A <see cref="Version"/> that represents the underlying <see cref="Assembly"/> of this instance.</returns>
        public Version GetAssemblyVersion()
        {
            return _assembly.GetName().Version;
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the <see cref="AssemblyFileVersionAttribute"/> of the underlying <see cref="Assembly"/> of this instance.
        /// </summary>
        /// <returns>A <see cref="Version"/> that represents the file version of the underlying <see cref="Assembly"/> of this instance; null if no <see cref="AssemblyFileVersionAttribute"/> could be retrieved.</returns>
        public Version GetFileVersion()
        {
            var version = _assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return version == null ? null : new Version(version.Version);
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the <see cref="AssemblyInformationalVersionAttribute"/> of the underlying <see cref="Assembly"/> of this instance.
        /// </summary>
        /// <returns>A <see cref="Version"/> that represents the product version of the underlying <see cref="Assembly"/> of this instance; null if no <see cref="AssemblyInformationalVersionAttribute"/> could be retrieved.</returns>
        public Version GetProductVersion()
        {
            var version = _assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return version == null ? null : new Version(version.InformationalVersion);
        }

        /// <summary>
        /// Gets the types contained within the underlying <see cref="Assembly"/> of this instance.
        /// </summary>
        /// <param name="namespaceFilter">The filter to limit the types by namespace.</param>
        /// <param name="typeFilter">The filter to limit the types by a specific type.</param>
        /// <returns>A sequence of <see cref="Type"/> elements, matching the applied filters, from the underlying <see cref="Assembly"/> of this instance.</returns>
        public IEnumerable<Type> GetTypes(string namespaceFilter = null, Type typeFilter = null)
        {
            var hasNamespaceFilter = !string.IsNullOrEmpty(namespaceFilter);
            var hasTypeFilter = (typeFilter != null);
            IEnumerable<Type> types = _assembly.GetTypes();
            if (hasNamespaceFilter || hasTypeFilter)
            {
                if (hasNamespaceFilter) { types = GetAssemblyTypesByNamespace(types, namespaceFilter); }
                if (hasTypeFilter)
                {
                    types = typeFilter.IsInterface ? GetAssemblyTypesByInterfaceType(types, typeFilter) : GetAssemblyTypesByBaseType(types, typeFilter);
                }
            }
            return types;
        }

        private static IEnumerable<Type> GetAssemblyTypesByInterfaceType(IEnumerable<Type> types, Type typeFilter)
        {
            foreach (var type in types)
            {
                if (TypeInsight.FromType(type).HasInterface(typeFilter))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetAssemblyTypesByBaseType(IEnumerable<Type> types, Type typeFilter)
        {
            foreach (var type in types)
            {
                if (TypeInsight.FromType(type).HasType(typeFilter))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetAssemblyTypesByNamespace(IEnumerable<Type> types, string namespaceFilter)
        {
            foreach (var type in types)
            {
                if (type.Namespace == null) { continue; }
                if (type.Namespace.Equals(namespaceFilter, StringComparison.OrdinalIgnoreCase))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Assembly"/> of this instance is a debug build.
        /// </summary>
        /// <returns><c>true</c> if the the underlying <see cref="Assembly"/> of this instance is a debug build; otherwise, <c>false</c>.</returns>
        public bool IsDebugBuild()
        {
            var debuggingFlags = GetDebuggingFlags();
            var isDebug = debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.Default) ||
                          debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.DisableOptimizations) ||
                          debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.EnableEditAndContinue);
            var isJitTrackingEnabled = (debuggingFlags & DebuggableAttribute.DebuggingModes.Default) != 0;
            return isDebug || isJitTrackingEnabled;
        }

        private DebuggableAttribute.DebuggingModes GetDebuggingFlags()
        {
            var attributes = _assembly.CustomAttributes;
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeType == typeof(DebuggableAttribute))
                {
                    var debugModesArgument = attribute.ConstructorArguments.First();
                    return (DebuggableAttribute.DebuggingModes)Enum.Parse(typeof(DebuggableAttribute.DebuggingModes), debugModesArgument.Value.ToString());
                }
            }
            return DebuggableAttribute.DebuggingModes.None;
        }
    }
}