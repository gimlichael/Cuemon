using System;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="AssemblyUtility"/> class.
    /// </summary>
    public static class AssemblyUtilityExtensions
    {
        /// <summary>
        /// Returns a <see cref="Version"/> that represents the version number of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the version number of the specified <paramref name="assembly"/>.</returns>
        public static Version GetAssemblyVersion(this Assembly assembly)
        {
            return AssemblyUtility.GetAssemblyVersion(assembly);
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the file version number of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the file version number of the specified <paramref name="assembly"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="assembly"/> is null.
        /// </exception>
        public static Version GetFileVersion(this Assembly assembly)
        {
            return AssemblyUtility.GetFileVersion(assembly);
        }

        /// <summary>
        /// Returns a <see cref="Version"/> that represents the version of the product this <paramref name="assembly"/> is distributed with.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="Version"/> from.</param>
        /// <returns>A <see cref="Version"/> that represents the version of the product this <paramref name="assembly"/> is distributed with.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="assembly"/> is null.
        /// </exception>
        public static Version GetProductVersion(this Assembly assembly)
        {
            return AssemblyUtility.GetProductVersion(assembly);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a debug build.
        /// </summary>
        /// <param name="assembly">The assembly to parse and determine whether it is a debug build or not.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a debug build; otherwise, <c>false</c>.</returns>
        public static bool IsDebugBuild(this Assembly assembly)
        {
            return AssemblyUtility.IsDebugBuild(assembly);
        }
    }
}