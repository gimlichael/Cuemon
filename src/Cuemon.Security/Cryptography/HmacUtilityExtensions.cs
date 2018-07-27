using System;
using System.IO;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HmacUtility"/> class.
    /// </summary>
    public static class HmacUtilityExtensions
    {
        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="StreamKeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(this Stream value, byte[] sharedKey, Action<StreamKeyedHashOptions> setup = null)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, setup);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="KeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(this byte[] value, byte[] sharedKey, Action<KeyedHashOptions> setup = null)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, setup);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="StringKeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(this string value, byte[] sharedKey, Action<StringKeyedHashOptions> setup = null)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, setup);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="KeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(this object value, byte[] sharedKey, Action<KeyedHashOptions> setup = null)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, setup);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a keyed-hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="KeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(this object[] values, byte[] sharedKey, Action<KeyedHashOptions> setup = null)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey, setup);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="StringKeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(this string[] values, byte[] sharedKey, Action<StringKeyedHashOptions> setup = null)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey, setup);
        }
    }
}