using System;
using System.Collections;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Represents the result of a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation. This class cannot be inherited.
    /// </summary>
    public sealed class HashResult
    {
        internal HashResult(byte[] value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of the <see cref="HashResult"/> that reflects a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation.
        /// </summary>
        /// <value>The value of the <see cref="HashResult"/> that reflects a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation.</value>
        public byte[] Value { get; }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the contents of <see cref="Value"/>.</returns>
        public string ToHexadecimal()
        {
            return StringConverter.ToHexadecimal(Value);
        }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64, of the contents of <see cref="Value"/>.</returns>
        public string ToBase64()
        {
            return Convert.ToBase64String(Value);
        }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent string representation that is encoded with base-64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64 which is usable for transmission on the URL, of the contents of <see cref="Value"/>.</returns>
        public string ToUrlEncodedBase64()
        {
            return StringConverter.ToUrlEncodedBase64(Value);
        }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent binary representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in binary, of the contents of <see cref="Value"/>.</returns>
        public string ToBinary()
        {
            return StringConverter.ToBinary(Value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToHexadecimal();
        }
    }
}