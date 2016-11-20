using System;

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
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created)
        {
            return new CacheValidator(created);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified)
        {
            return new CacheValidator(created, modified);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Double"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, double checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Double"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, double checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int16"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, short checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int16"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, short checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="String"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, string checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="String"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, string checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int32"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, int checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int32"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, int checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int64"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, long checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int64"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, long checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Single"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, float checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="Single"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, float checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt16"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ushort checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt16"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ushort checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt32"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, uint checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt32"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, uint checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt64"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ulong checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt64"/> value containing a byte-for-byte checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, ulong checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">An array of bytes containing a checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, byte[] checksum)
        {
            return new CacheValidator(created, modified, checksum);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified parameters.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
        /// <param name="checksum">An array of bytes containing a checksum of the data this <see cref="CacheValidator"/> represents.</param>
        /// <param name="method">One of the enumeration values of <see cref="ChecksumMethod"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified parameters.</returns>
        public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, byte[] checksum, ChecksumMethod method)
        {
            return new CacheValidator(created, modified, checksum, method);
        }
    }
}