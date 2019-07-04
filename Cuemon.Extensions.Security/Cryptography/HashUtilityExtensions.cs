using System;
using System.IO;
using Cuemon.Integrity;
using Cuemon.Security.Cryptography;
using Cuemon.Text;

namespace Cuemon.Extensions.Security.Cryptography
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HashUtility"/> class.
    /// </summary>
    public static class HashUtilityExtensions
    {
        /// <summary>
        /// Computes a hash value of the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StreamHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="stream"/>.</returns>
        //public static HashResult ComputeHash(this Stream stream, Action<StreamHashOptions> setup = null)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Computes a hash value of the specified <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="bytes"/>.</returns>
        //public static HashResult ComputeHash(this byte[] bytes, Action<HashOptions> setup = null)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(this string value, CryptoAlgorithm algorithm, Action<EncodingOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="values"/>.</returns>
        //public static HashResult ComputeHash(this string[] values, Action<StringHashOptions> setup = null)
        //{
        //    throw new NotImplementedException();
        //}
    }
}