using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="ByteConverter"/> class.
    /// </summary>
    public static class ByteConverterExtensions
    {
        /// <summary>
        /// Decodes a URL string token to its equivalent byte array using base 64 digits.
        /// </summary>
        /// <param name="value">The URL string token to decode.</param>
        /// <returns>The byte array containing the decoded URL string token.</returns>
        public static byte[] FromUrlEncodedBase64(this string value)
        {
            return ByteConverter.FromUrlEncodedBase64(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array always starting from position 0 (when supported).
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <returns>A <b>byte array</b> containing the data from the stream.</returns>
        public static byte[] ToByteArray(this Stream value)
        {
            return ByteConverter.FromStream(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <b>byte array</b> containing the data from the stream.</returns>
        public static byte[] ToByteArray(this Stream value, bool leaveStreamOpen)
        {
            return ByteConverter.FromStream(value, leaveStreamOpen);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an array of bytes.
        /// </summary>
        /// <param name="value">The <see cref="IConvertible"/> value to convert.</param>
        /// <returns>An array of bytes equivalent to the data of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Boolean"/>, <see cref="Char"/>, <see cref="double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="ushort"/>, <see cref="UInt32"/> and <see cref="UInt64"/>.
        /// </exception>
        public static byte[] ToByteArray<T>(this T value) where T : struct, IConvertible
        {
            return ByteConverter.FromConvertibles(value);
        }

        /// <summary>
        /// Converts the specified sequence of <paramref name="values"/> to an array of bytes.
        /// </summary>
        /// <param name="values">A sequence of <see cref="IConvertible"/> values to convert.</param>
        /// <returns>An array of bytes equivalent to the sequence of the <paramref name="values"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="values"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Boolean"/>, <see cref="Char"/>, <see cref="double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="ushort"/>, <see cref="UInt32"/> and <see cref="UInt64"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> is null.
        /// </exception>
        public static byte[] ToByteArray<T>(this IEnumerable<T> values) where T : struct, IConvertible
        {
            return ByteConverter.FromConvertibles(values);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array using <see cref="EncodingOptions.DefaultEncoding"/> for the encoding and <see cref="EncodingOptions.DefaultPreambleSequence"/> for any preamble sequences.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>A <b>byte array</b> containing the results of encoding the specified set of characters.</returns>
        public static byte[] ToByteArray(this string value)
        {
            return ByteConverter.FromString(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <b>byte array</b> containing the results of encoding the specified set of characters.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static byte[] ToByteArray(this string value, Action<EncodingOptions> setup)
        {
            return ByteConverter.FromString(value, setup);
        }
    }
}