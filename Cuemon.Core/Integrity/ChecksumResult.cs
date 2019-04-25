using System;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Represents the result of a computed checksum operation.
    /// </summary>
    public class ChecksumResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumResult"/> class.
        /// </summary>
        /// <param name="value">The checksum represented as a byte-array.</param>
        public ChecksumResult(byte[] value)
        {
            Value = value ?? new byte[0];
        }

        /// <summary>
        /// Gets the original value that reflects a computed operation.
        /// </summary>
        /// <value>The value original value that reflects a computed operation.</value>
        public byte[] Value { get; }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the contents of <see cref="Value"/>.</returns>
        public virtual string ToHexadecimalString()
        {
            return StringConverter.ToHexadecimal(Value);
        }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64, of the contents of <see cref="Value"/>.</returns>
        public virtual string ToBase64String()
        {
            return Convert.ToBase64String(Value);
        }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent string representation that is encoded with base-64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in base 64 which is usable for transmission on the URL, of the contents of <see cref="Value"/>.</returns>
        public virtual string ToUrlEncodedBase64String()
        {
            return StringConverter.ToUrlEncodedBase64(Value);
        }

        /// <summary>
        /// Converts the <see cref="Value"/> to its equivalent binary representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in binary, of the contents of <see cref="Value"/>.</returns>
        public virtual string ToBinaryString()
        {
            return StringConverter.ToBinary(Value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToHexadecimalString();
        }
    }
}