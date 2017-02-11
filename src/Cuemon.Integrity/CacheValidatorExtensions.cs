using System;
using System.Reflection;
using System.Reflection.Emit;
using Cuemon.IO;
using Cuemon.Reflection;

namespace Cuemon.Integrity
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="CacheValidator"/> class.
    /// </summary>
    public static class CacheValidatorExtensions
    {
        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Double"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, double checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }


        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int16"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, short checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="String"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, string checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int32"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, int checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int64"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, long checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Single"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, float checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt16"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ushort checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt32"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, uint checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt64"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ulong checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">An array of bytes containing a checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, byte[] checksum, Action<CacheValidatorOptions> setup = null)
        {
            return new CacheValidator(created, modified, checksum, setup);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the file.</param>
        /// <param name="bytesToRead">The maximum size of a byte-for-byte that promotes a medium/strong integrity check of the specified <paramref name="fileName"/>. A value of 0 (or less) leaves the integrity check at weak.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents either a weak, medium or strong integrity check of the specified <paramref name="fileName"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="fileName"/> is null.
        /// </exception>
        /// <remarks>Should the specified <paramref name="fileName"/> trigger any sort of exception, a <see cref="CacheValidator.Default"/> is returned.</remarks>
        public static CacheValidator GetCacheValidator(this string fileName, int bytesToRead = 0, Action<CacheValidatorOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(fileName, nameof(fileName));
            var options = setup.ConfigureOptions();
            try
            {
                return FileInfoConverter.Convert(fileName, bytesToRead, (fi, checksumBytes) =>
                {
                    if (checksumBytes.Length > 0)
                    {
                        return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, checksumBytes.GetHashCode64(), setup);
                    }
                    return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, o =>
                    {
                        o.AlgorithmType = options.AlgorithmType;
                        o.Method = bytesToRead == 0 ? ChecksumMethod.Timestamp : ChecksumMethod.Default;
                    });
                });
            }
            catch (Exception)
            {
                return CacheValidator.Default;
            }
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="assembly" />.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="CacheValidator" /> from.</param>
        /// <param name="readByteForByteChecksum"><c>true</c> to read the <paramref name="assembly"/> byte-for-byte to promote a strong integrity checksum; <c>false</c> to read common properties of the <paramref name="assembly"/> for a weak (but reliable) integrity checksum.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions" /> which need to be configured.</param>
        /// <returns>A <see cref="CacheValidator" /> that fully represents the integrity of the specified <paramref name="assembly" />.</returns>
        public static CacheValidator GetCacheValidator(this Assembly assembly, bool readByteForByteChecksum = false, Action<CacheValidatorOptions> setup = null)
        {
            if ((assembly == null) || (assembly.ManifestModule is ModuleBuilder)) { return CacheValidator.Default; }
            var assemblyHashCode64 = assembly.FullName.GetHashCode64();
            var assemblyLocation = "";
            try
            {
                assemblyLocation = assembly.GetLocation();
            }
            catch (Exception)
            {
            }
            return assemblyLocation.IsNullOrEmpty() ? new CacheValidator(DateTime.MinValue, DateTime.MaxValue, setup).CombineWith(assemblyHashCode64) : GetCacheValidator(assemblyLocation, readByteForByteChecksum ? int.MaxValue : 0, setup).CombineWith(assemblyHashCode64);
        }
    }
}