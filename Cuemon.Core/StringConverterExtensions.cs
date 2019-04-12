using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="StringConverter"/> class.
    /// </summary>
    public static class StringConverterExtensions
    {
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
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The <see cref="System.IO.Stream"/> to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="string"/> containing the decoded result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToEncodedString(this Stream value, Action<EncodingOptions> setup = null)
        {
            return StringConverter.FromStream(value, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The <see cref="System.IO.Stream"/> to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="string"/> containing the decoded result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToEncodedString(this Stream value, Action<EncodingOptions> setup, bool leaveStreamOpen)
        {
            return StringConverter.FromStream(value, setup, leaveStreamOpen);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="String"/> sequence.
        /// </summary>
        /// <param name="value">The value to convert into a sequence.</param>
        /// <returns>A <see cref="String"/> sequence equivalent to the specified <paramref name="value"/>.</returns>
        public static IEnumerable<string> ToEnumerable(this IEnumerable<char> value)
        {
            return StringConverter.ToEnumerable(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="String"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="Char"/> sequence to convert.</param>
        /// <returns>A <see cref="String"/> equivalent to the specified <paramref name="value"/>.</returns>
        public static string FromChars(this IEnumerable<char> value)
        {
            return StringConverter.FromChars(value);
        }

        /// <summary>
        /// Converts the specified hexadecimal <paramref name="value"/> to its equivalent <see cref="String"/> representation.
        /// </summary>
        /// <param name="value">The hexadecimal string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="String"/> representation of the hexadecimal characters in <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string FromHexadecimal(this string value, Action<EncodingOptions> setup = null)
        {
            return StringConverter.FromHexadecimal(value, setup);
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
        /// Converts the specified <paramref name="value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A hexadecimal <see cref="String"/> representation of the characters in <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToHexadecimal(this string value, Action<EncodingOptions> setup = null)
        {
            return StringConverter.ToHexadecimal(value, setup);
        }

        /// <summary>
        /// Renders the <paramref name="exception"/> to a human readable <see cref="String"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to render human readable.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack trace of the exception is included in the rendered result.</param>
        /// <returns>A human readable <see cref="String"/> variant of the specified <paramref name="exception"/>.</returns>
        public static string ToEncodedString(this Exception exception, bool includeStackTrace)
        {
            return StringConverter.FromException(exception, Encoding.Unicode, includeStackTrace);
        }

        /// <summary>
        /// Renders the <paramref name="exception"/> to a human readable <see cref="String"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to render human readable.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack trace of the exception is included in the rendered result.</param>
        /// <param name="encoding">The encoding to use when rendering the <paramref name="exception"/>.</param>
        /// <returns>A human readable <see cref="String"/> variant of the specified <paramref name="exception"/>.</returns>
        public static string ToEncodedString(this Exception exception, bool includeStackTrace, Encoding encoding)
        {
            return StringConverter.FromException(exception, encoding, includeStackTrace);
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
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <returns>A sanitized <see cref="String"/> representation of <paramref name="source"/>.</returns>
        /// <remarks>Only the simple name of the <paramref name="source"/> is returned, not the fully qualified name.</remarks>
        public static string ToFriendlyName(this Type source)
        {
            return StringConverter.FromType(source);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <returns>A sanitized <see cref="String"/> representation of <paramref name="source"/>.</returns>
        public static string ToFriendlyName(this Type source, bool fullName)
        {
            return StringConverter.FromType(source, fullName);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <param name="excludeGenericArguments">Specify <c>true</c> to exclude generic arguments from the result; otherwise <c>false</c> to include generic arguments should the <paramref name="source"/> be a generic type.</param>
        /// <returns>A sanitized <see cref="String"/> representation of <paramref name="source"/>.</returns>
        public static string ToFriendlyName(this Type source, bool fullName, bool excludeGenericArguments)
        {
            return StringConverter.FromType(source, fullName, excludeGenericArguments);
        }
    }
}