using System;
using System.Security.Cryptography;
using Cuemon.Integrity;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Represents the result of a computed <see cref="HashAlgorithm"/> or <see cref="KeyedHashAlgorithm"/> operation. This class cannot be inherited.
    /// </summary>
    public sealed class HashResult : ChecksumResult
    {
        internal HashResult(byte[] value) : base(value)
        {
        }

        /// <summary>
        /// Converts the <see cref="ChecksumResult.Value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the contents of <see cref="ChecksumResult.Value"/>.</returns>
        public override string ToHexadecimal()
        {
            return StringConverter.ToHexadecimal(Value);
        }

        /// <summary>
        /// Converts the <see cref="ChecksumResult.Value"/> to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64, of the contents of <see cref="ChecksumResult.Value"/>.</returns>
        public override string ToBase64()
        {
            return Convert.ToBase64String(Value);
        }

        /// <summary>
        /// Converts the <see cref="ChecksumResult.Value"/> to its equivalent string representation that is encoded with base-64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64 which is usable for transmission on the URL, of the contents of <see cref="ChecksumResult.Value"/>.</returns>
        public override string ToUrlEncodedBase64()
        {
            return StringConverter.ToUrlEncodedBase64(Value);
        }

        /// <summary>
        /// Converts the <see cref="ChecksumResult.Value"/> to its equivalent binary representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in binary, of the contents of <see cref="ChecksumResult.Value"/>.</returns>
        public override string ToBinary()
        {
            return StringConverter.ToBinary(Value);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToHexadecimal();
        }
    }
}