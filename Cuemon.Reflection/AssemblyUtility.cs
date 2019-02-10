using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This utility class is designed to make common <see cref="Assembly"/> related operations easier to work with.
    /// </summary>
    public static class AssemblyUtility
    {
        /// <summary>
        /// Returns a <see cref="Version"/> that represents the version number of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the version number of the specified <paramref name="assembly"/>.</returns>
        public static Version GetAssemblyVersion(Assembly assembly)
        {
            if ((assembly == null) || (assembly.ManifestModule is ModuleBuilder)) { return VersionUtility.MinValue; }
            return assembly.GetName().Version;
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the file version number of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the file version number of the specified <paramref name="assembly"/>.</returns>
        public static Version GetFileVersion(Assembly assembly)
        {
            if ((assembly == null) || (assembly.ManifestModule is ModuleBuilder)) { return VersionUtility.MinValue; }
            var fileVersion = GetFileVersionCore(assembly);
            if (fileVersion != VersionUtility.MaxValue) { return fileVersion; }
            return GetAssemblyVersion(assembly);
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the version of the product this <paramref name="assembly"/> is distributed with.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the version of the product this <paramref name="assembly"/> is distributed with.</returns>
        public static Version GetProductVersion(Assembly assembly)
        {
            if ((assembly == null) || (assembly.ManifestModule is ModuleBuilder)) { return VersionUtility.MinValue; }
            var productVersion = GetProductVersionCore(assembly);
            if (productVersion != VersionUtility.MaxValue) { return productVersion; }
            return GetFileVersion(assembly);
        }

        private static Version GetProductVersionCore(Assembly assembly)
        {
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return version == null ? VersionUtility.MaxValue : new Version(version.InformationalVersion);
        }

        private static Version GetFileVersionCore(Assembly assembly)
        {
            var version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return version == null ? VersionUtility.MaxValue : new Version(version.Version);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a debug build.
        /// </summary>
        /// <param name="assembly">The assembly to parse and determine whether it is a debug build or not.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a debug build; otherwise, <c>false</c>.</returns>
        public static bool IsDebugBuild(Assembly assembly)
        {
            var debuggingFlags = GetDebuggingFlags(assembly);
            bool isDebug = debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.Default) ||
                            debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.DisableOptimizations) ||
                            debuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.EnableEditAndContinue);
            bool isJitTrackingEnabled = ((debuggingFlags & DebuggableAttribute.DebuggingModes.Default) != 0);
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