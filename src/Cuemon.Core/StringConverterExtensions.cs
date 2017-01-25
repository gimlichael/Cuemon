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

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of comma delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <returns>A <see cref="string"/> of comma delimited values.</returns>
        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source)
        {
            return StringConverter.ToDelimitedString(source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
        {
            return StringConverter.ToDelimitedString(source, delimiter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter, Func<TSource, string> converter)
        {
            return StringConverter.ToDelimitedString(source, delimiter, converter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T>(this IEnumerable<TSource> source, string delimiter, Func<TSource, T, string> converter, T arg)
        {
            return StringConverter.ToDelimitedString(source, delimiter, converter, arg);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2>(this IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, string> converter, T1 arg1, T2 arg2)
        {
            return StringConverter.ToDelimitedString(source, delimiter, converter, arg1, arg2);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3>(this IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, T3, string> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            return StringConverter.ToDelimitedString(source, delimiter, converter, arg1, arg2, arg3);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4>(this IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, T3, T4, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return StringConverter.ToDelimitedString(source, delimiter, converter, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4, T5>(this IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, T3, T4, T5, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return StringConverter.ToDelimitedString(source, delimiter, converter, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter, string format)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter, string format, Func<TSource, string> converter)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format, converter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T>(this IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T, string> converter, T arg)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format, converter, arg);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2>(this IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, string> converter, T1 arg1, T2 arg2)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format, converter, arg1, arg2);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3>(this IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, T3, string> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format, converter, arg1, arg2, arg3);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4>(this IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, T3, T4, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format, converter, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4, T5>(this IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, T3, T4, T5, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return StringConverter.ToDelimitedString(source, delimiter, format, converter, arg1, arg2, arg3, arg4, arg5);
        }
    }
}