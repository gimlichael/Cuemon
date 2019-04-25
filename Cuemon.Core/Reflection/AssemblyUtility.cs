using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This utility class is designed to make common <see cref="Assembly"/> related operations easier to work with.
    /// </summary>
    public static class AssemblyUtility
    {
        /// <summary>
        /// Loads the embedded resources from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to load the resources from.</param>
        /// <param name="name">The case-sensitive name of the resource being requested.</param>
        /// <param name="match">The ruleset to apply.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence that contains <see cref="Stream"/> objects representing the loaded resources; null if no resources were specified during compilation, or if the resources is not visible to the caller.</returns>
        public static IEnumerable<Stream> GetEmbeddedResources(Assembly assembly, string name, EmbeddedResourceMatch match = EmbeddedResourceMatch.Name)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            Validator.ThrowIfNullOrWhitespace(name, nameof(name));

            var resources = new List<Stream>();
            switch (match)
            {
                case EmbeddedResourceMatch.Name:
                    resources.Add(assembly.GetManifestResourceStream(name));
                    break;
                case EmbeddedResourceMatch.Extension:
                case EmbeddedResourceMatch.ContainsName:
                case EmbeddedResourceMatch.ContainsExtension:
                    var resourceNames = assembly.GetManifestResourceNames();
                    var matchExtension = Path.GetExtension(name).ToUpperInvariant();
                    switch (match)
                    {
                        case EmbeddedResourceMatch.ContainsName:
                            foreach (var resourceName in resourceNames)
                            {
                                if (resourceName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    resources.Add(assembly.GetManifestResourceStream(resourceName));
                                }
                            }
                            break;
                        case EmbeddedResourceMatch.Extension:
                            foreach (var resourceName in resourceNames)
                            {
                                var extension = Path.GetExtension(resourceName).ToUpperInvariant();
                                if (extension == matchExtension)
                                {
                                    resources.Add(assembly.GetManifestResourceStream(resourceName));
                                }
                            }
                            break;
                        case EmbeddedResourceMatch.ContainsExtension:
                            foreach (var resourceName in resourceNames)
                            {
                                var extension = Path.GetExtension(resourceName);
                                if (extension.IndexOf(matchExtension, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    resources.Add(assembly.GetManifestResourceStream(resourceName));
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(match));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(match));
            }
            return resources;
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the <see cref="AssemblyVersionAttribute"/> of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the assembly version.</returns>
        public static Version GetAssemblyVersion(Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            return assembly.GetName().Version;
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the <see cref="AssemblyFileVersionAttribute"/> of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the file version; null if no <see cref="AssemblyFileVersionAttribute"/> could be retrieved.</returns>
        public static Version GetFileVersion(Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            var version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return version == null ? null : new Version(version.Version);
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the <see cref="AssemblyInformationalVersionAttribute"/> of the  specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the product version; null if no <see cref="AssemblyInformationalVersionAttribute"/> could be retrieved.</returns>
        public static Version GetProductVersion(Assembly assembly)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return version == null ? null : new Version(version.InformationalVersion);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a debug build.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to parse and determine whether it is a debug build or not.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a debug build; otherwise, <c>false</c>.</returns>
        public static bool IsDebugBuild(Assembly assembly)
        {
            var debuggingFlags = GetDebuggingFlags(assembly);
            var isDebug = debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.Default) ||
                            debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.DisableOptimizations) ||
                            debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.EnableEditAndContinue);
            var isJitTrackingEnabled = (debuggingFlags & DebuggableAttribute.DebuggingModes.Default) != 0;
            return isDebug || isJitTrackingEnabled;
        }

        private static DebuggableAttribute.DebuggingModes GetDebuggingFlags(Assembly assembly)
        {
            if (assembly != null)
            {
                var attributes = assembly.CustomAttributes;
                foreach (var attribute in attributes)
                {
                    if (attribute.AttributeType == typeof(DebuggableAttribute))
                    {
                        var debugModesArgument = attribute.ConstructorArguments.First();
                        return (DebuggableAttribute.DebuggingModes) Enum.Parse(typeof(DebuggableAttribute.DebuggingModes), debugModesArgument.Value.ToString());
                    }
                }
            }
            return DebuggableAttribute.DebuggingModes.None;
        }
    }
}