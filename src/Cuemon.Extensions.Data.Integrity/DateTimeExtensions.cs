using System;
using Cuemon.Data.Integrity;

namespace Cuemon.Extensions.Data.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class DateTimeExtensions
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
    }
}