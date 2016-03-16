using System;
using System.Collections;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Represents the result of a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation. This class cannot be inherited.
    /// </summary>
    public sealed class HashValue
    {
        internal HashValue(byte[] result)
        {
            Result = result;
        }

        /// <summary>
        /// Gets the original result of a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation.
        /// </summary>
        /// <value>The original result of a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation.</value>
        public byte[] Result { get; }

        /// <summary>
        /// Converts the <see cref="Result"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the contents of <see cref="Result"/>.</returns>
        public string ToHexadecimal()
        {
            return StringConverter.ToHexadecimal(Result);
        }

        /// <summary>
        /// Converts the <see cref="Result"/> to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64, of the contents of <see cref="Result"/>.</returns>
        public string ToBase64()
        {
            return Convert.ToBase64String(Result);
        }

        /// <summary>
        /// Converts the <see cref="Result"/> to its equivalent string representation that is encoded with base-64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64 which is usable for transmission on the URL, of the contents of <see cref="Result"/>.</returns>
        public string ToUrlEncodedBase64()
        {
            return StringConverter.ToUrlEncodedBase64(Result);
        }

        /// <summary>
        /// Converts the <see cref="Result"/> to its equivalent binary representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in binary, of the contents of <see cref="Result"/>.</returns>
        public string ToBinary()
        {
            return StringConverter.ToBinary(Result);
        }
    }
}