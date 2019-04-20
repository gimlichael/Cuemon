using System;
using System.Text;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> struct.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Returns an <see cref="IConvertible"/> primitive converted from the specified array <paramref name="value"/> of bytes.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value"/> after conversion.</typeparam>
        /// <param name="value">The value to convert into an <see cref="IConvertible"/>.</param>
        /// <returns>An <see cref="IConvertible"/> primitive formed by n-bytes beginning at 0.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Boolean"/>, <see cref="Char"/>, <see cref="double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="ushort"/>, <see cref="UInt32"/> and <see cref="UInt64"/>.
        /// </exception>
        public static T ToConvertible<T>(this byte[] value) where T : struct, IConvertible
        {
            return ConvertibleConverter.FromBytes<T>(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="string"/> containing the results of decoding the specified sequence of bytes.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToEncodedString(this byte[] value, Action<EncodingOptions> setup = null)
        {
            return StringConverter.FromBytes(value, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A hexadecimal <see cref="string"/> representation of the elements in <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string ToHexadecimal(this byte[] value)
        {
            return StringConverter.ToHexadecimal(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent binary representation.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A binary <see cref="string"/> representation of the elements in <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string ToBinary(this byte[] value)
        {
            return StringConverter.ToBinary(value);
        }

        /// <summary>
        /// Encodes a byte array into its equivalent string representation using base 64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <param name="value">The byte array to encode.</param>
        /// <returns>The string containing the encoded token if the byte array length is greater than one; otherwise, an empty string ("").</returns>
        public static string ToUrlEncodedBase64(this byte[] value)
        {
            return StringConverter.ToUrlEncodedBase64(value);
        }

        /// <summary>
        /// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="bytes">An array of 8-bit unsigned integers.</param>
        /// <returns>The string representation, in base 64, of the contents of <paramref name="bytes"/>.</returns>
        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="bytes"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="bytes"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="bytes"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(this byte[] bytes, out Encoding result)
        {
            return ByteUtility.TryDetectUnicodeEncoding(bytes, out result);
        }

        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="input">The input <see cref="byte"/> array to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <returns>A <see cref="byte"/> array without preamble information.</returns>
        public static byte[] RemovePreamble(this byte[] input, Encoding encoding)
        {
            return ByteUtility.RemovePreamble(input, encoding);
        }

        /// <summary>
        /// Removes trailing zero information (if any) from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="input">The input <see cref="byte"/> array to process.</param>
        /// <returns>A <see cref="byte"/> array without trailing zeros.</returns>
        public static byte[] RemoveTrailingZeros(this byte[] input)
        {
            return ByteUtility.RemoveTrailingZeros(input);
        }
    }
}