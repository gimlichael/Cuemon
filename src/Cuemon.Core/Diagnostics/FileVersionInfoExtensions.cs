using System;
using System.Diagnostics;

namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="FileVersionInfo"/> class.
    /// </summary>
    public static class FileVersionInfoExtensions
    {
        /// <summary>
        /// Returns a <see cref="Version"/> from the specified <paramref name="fvi"/>.
        /// </summary>
        /// <param name="fvi">An instance of <see cref="FileVersionInfo"/>.</param>
        /// <returns>A <see cref="Version"/> that represents the product version that the <see cref="FileVersionInfo"/> is distributed with.</returns>
        /// <remarks>Should the specified <paramref name="fvi"/> not contain any product version, a <see cref="Version"/> initialized to 0.0.0.0 is returned.</remarks>
        public static Version ToProductVersion(this FileVersionInfo fvi)
        {
            return ToVersion(fvi, info => info.ProductVersion);
        }

        /// <summary>
        /// Returns a <see cref="Version"/> from the specified <paramref name="fvi"/>.
        /// </summary>
        /// <param name="fvi">An instance of <see cref="FileVersionInfo"/>.</param>
        /// <returns>A <see cref="Version"/> that represents the file version that the <see cref="FileVersionInfo"/> is distributed with.</returns>
        /// <remarks>Should the specified <paramref name="fvi"/> not contain any file version, a <see cref="Version"/> initialized to 0.0.0.0 is returned.</remarks>
        public static Version ToFileVersion(this FileVersionInfo fvi)
        {
            return ToVersion(fvi, info => info.FileVersion);
        }

        private static Version ToVersion(this FileVersionInfo fvi, Func<FileVersionInfo, string> propertySelector)
        {
            Validator.ThrowIfNull(fvi, nameof(fvi));
            var version = propertySelector(fvi);
            return new Version(version.IsNullOrEmpty() ? "0.0.0.0" : version);
        }
    }
}