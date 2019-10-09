using System;
using System.IO;
using System.Reflection;
using Cuemon.Integrity;
using Cuemon.IO;

namespace Cuemon.Extensions.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="Assembly"/> class.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="assembly" />.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="CacheValidator" /> from.</param>
        /// <param name="readByteForByteChecksum"><c>true</c> to read the <paramref name="assembly"/> byte-for-byte to promote a strong integrity checksum; <c>false</c> to read common properties of the <paramref name="assembly"/> for a weak (but reliable) integrity checksum.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions" /> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator" /> that fully represents the integrity of the specified <paramref name="assembly" />.</returns>
        public static CacheValidator GetCacheValidator(this Assembly assembly, bool readByteForByteChecksum = false, Action<CacheValidatorOptions> setup = null)
        {
            if (assembly == null || assembly.IsDynamic) { return CacheValidator.Default; }
            var assemblyHashCode64 = assembly.FullName.GetHashCode64();
            var assemblyLocation = assembly.Location;
            return assemblyLocation.IsNullOrEmpty() ? new CacheValidator(DateTime.MinValue, DateTime.MaxValue, assemblyHashCode64, setup) : new FileInfo(assemblyLocation).GetCacheValidator(Patterns.ConfigureExchange<CacheValidatorOptions, FileChecksumOptions>(setup, (cvo, fco) => 
            {
                fco.BytesToRead = readByteForByteChecksum ? int.MaxValue : 0;
                fco.Algorithm = cvo.Algorithm;
                fco.Method = cvo.Method;
            })).CombineWith(assemblyHashCode64);
        }
    }
}