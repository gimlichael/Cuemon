using System;
using System.IO;
using Cuemon.Integrity;
using Cuemon.IO;

namespace Cuemon.Extensions.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="FileInfo"/> class.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/> to extend.</param>
        /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents either a weak, medium or strong integrity check of the specified <paramref name="file"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> is null.
        /// </exception>
        /// <remarks>Should the specified <paramref name="file"/> trigger any sort of exception, a <see cref="CacheValidator.Default"/> is returned.</remarks>
        public static CacheValidator GetCacheValidator(this FileInfo file, Action<FileChecksumOptions> setup = null)
        {
            Validator.ThrowIfNull(file, nameof(file));
            try
            {
                return CacheValidator.FromFileInfo(file, setup);
            }
            catch (Exception)
            {
                return CacheValidator.Default;
            }
        }
    }
}