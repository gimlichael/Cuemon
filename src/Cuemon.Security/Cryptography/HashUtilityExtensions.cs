using System;
using System.IO;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HashUtility"/> class.
    /// </summary>
    public static class HashUtilityExtensions
    {
        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StreamHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(this Stream value, Action<StreamHashOptions> setup = null)
        {
            return HashUtility.ComputeHash(value, setup);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(this byte[] value, Action<HashOptions> setup = null)
        {
            return HashUtility.ComputeHash(value, setup);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(this string value, Action<StringHashOptions> setup = null)
        {
            return HashUtility.ComputeHash(value, setup);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(this object value, Action<HashOptions> setup = null)
        {
            return HashUtility.ComputeHash(value, setup);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a hash value of the specified sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified object sequence <paramref name="values"/>.</returns>
        public static HashResult ComputeHash(this object[] values, Action<HashOptions> setup = null)
        {
            return HashUtility.ComputeHash(values, setup);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="values"/>.</returns>
        public static HashResult ComputeHash(this string[] values, Action<StringHashOptions> setup = null)
        {
            return HashUtility.ComputeHash(values, setup);
        }
    }
}