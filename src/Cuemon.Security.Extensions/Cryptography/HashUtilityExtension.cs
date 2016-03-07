using System.IO;
using System.Text;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HashUtility"/> class.
    /// </summary>
    public static class HashUtilityExtension
    {
        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <returns>A <see cref="string"/> containing the computed MD5 hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static string ComputeHash(this Stream value)
        {
            return HashUtility.ComputeHash(value);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static string ComputeHash(this Stream value, HashAlgorithmType algorithmType)
        {
            return HashUtility.ComputeHash(value, algorithmType);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static string ComputeHash(this Stream value, HashAlgorithmType algorithmType, bool leaveStreamOpen)
        {
            return HashUtility.ComputeHash(value, algorithmType, leaveStreamOpen);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="byte"/> sequence, <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <returns>A <see cref="string"/> containing the computed MD5 hash value of the specified <see cref="byte"/> sequence <paramref name="value"/>.</returns>
        public static string ComputeHash(this byte[] value)
        {
            return HashUtility.ComputeHash(value);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="byte"/> sequence, <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="byte"/> sequence <paramref name="value"/>.</returns>
        public static string ComputeHash(this byte[] value, HashAlgorithmType algorithmType)
        {
            return HashUtility.ComputeHash(value, algorithmType);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <returns>A <see cref="string"/> containing the computed MD5 hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static string ComputeHash(this string value)
        {
            return HashUtility.ComputeHash(value);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static string ComputeHash(this string value, HashAlgorithmType algorithmType)
        {
            return HashUtility.ComputeHash(value, algorithmType);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="encoding">The encoding to use when computing the <paramref name="value"/>.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static string ComputeHash(this string value, HashAlgorithmType algorithmType, Encoding encoding)
        {
            return HashUtility.ComputeHash(value, algorithmType, encoding);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <returns>A <see cref="string"/> containing the computed MD5 hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static string ComputeHash(this string[] values)
        {
            return HashUtility.ComputeHash(values);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static string ComputeHash(this string[] values, HashAlgorithmType algorithmType)
        {
            return HashUtility.ComputeHash(values, algorithmType);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HashAlgorithmType"/> hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="encoding">The encoding to use when computing the <paramref name="values"/> sequence.</param>
        /// <returns>A <see cref="string"/> containing the computed hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static string ComputeHash(string[] values, HashAlgorithmType algorithmType, Encoding encoding)
        {
            return HashUtility.ComputeHash(values, algorithmType, encoding);
        }
    }
}