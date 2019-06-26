using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Represents the result of a computed checksum operation.
    /// </summary>
    public class HashResult
    {
        private readonly byte[] _input;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashResult"/> class.
        /// </summary>
        /// <param name="input">The computed checksum represented as a byte-array.</param>
        public HashResult(byte[] input)
        {
            _input = input ?? new byte[0];
        }

        /// <summary>
        /// Creates a copy of the original value that reflects a computed operation.
        /// </summary>
        /// <value>The copy of the original value that reflects a computed operation.</value>
        public byte[] GetBytes()
        {
            var copy = new byte[_input.Length];
            Array.Copy(_input, copy, copy.Length);
            return copy;
        }

        /// <summary>
        /// Converts the underlying value of this instance to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the contents of the underlying value of this instance.</returns>
        public virtual string ToHexadecimalString()
        {
            return ConvertFactory.UseConverter<HexadecimalByteArrayConverter>().ChangeType(_input);
        }

        /// <summary>
        /// Converts the underlying value of this instance to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64, of the contents of the underlying value of this instance.</returns>
        public virtual string ToBase64String()
        {
            return Convert.ToBase64String(_input);
        }

        /// <summary>
        /// Converts the underlying value of this instance to its equivalent string representation that is encoded with base-64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64 which is usable for transmission on the URL, of the contents of the underlying value of this instance.</returns>
        public virtual string ToUrlEncodedBase64String()
        {
            return ConvertFactory.UseConverter<UrlEncodedBase64ByteArrayConverter>().ChangeType(_input);
        }

        /// <summary>
        /// Converts the underlying value of this instance to its equivalent binary representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in binary, of the contents of the underlying value of this instance.</returns>
        public virtual string ToBinaryString()
        {
            return ConvertFactory.UseConverter<BinaryDigitsByteArrayConverter>().ChangeType(_input);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _input.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToHexadecimalString();
        }

        /// <summary>
        /// Provides a generic converter of a <see cref="T:byte[]"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="converter">The function delegate that takes the underlying value of this instance and converts it into <typeparamref name="T"/>.</param>
        /// <returns>An instance or value of <typeparamref name="T"/>.</returns>
        public T To<T>(Func<byte[], T> converter)
        {
            return converter(_input);
        }
    }
}