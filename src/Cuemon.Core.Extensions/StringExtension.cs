using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="string"/> class using various methods already found in the Microsoft .NET Framework.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Replaces the <paramref name="format"/> item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Converts the specified string, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="value"/>.</returns>
        public static byte[] FromBase64(this string value)
        {
            return Convert.FromBase64String(value);
        }

        /// <summary>
        /// Concatenates all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="value">An array that contains the elements to concatenate.</param>
        /// <param name="separator">The string to use as a separator. The separator is included in the returned string only if value has more than one element.</param>
        /// <returns>A string that consists of the elements in <paramref name="value"/> delimited by the <paramref name="separator"/> string. If value is an empty array, the method returns <see cref="string.Empty"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string Join(this string[] value, string separator)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return string.Join(separator, value);
        }

        /// <summary>
        /// Concatenates all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="value">An array that contains the elements to concatenate.</param>
        /// <param name="separator">The string to use as a separator. The separator is included in the returned string only if value has more than one element.</param>
        /// <param name="startIndex">The first element in <paramref name="value"/> to use.</param>
        /// <param name="count">The number of elements of <paramref name="value"/> to use.</param>
        /// <returns>A string that consists of the elements in <paramref name="value" /> delimited by the <paramref name="separator" /> string. If value is an empty array, the method returns <see cref="string.Empty" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value" /> is null.
        /// </exception>
        public static string Join(this string[] value, string separator, int startIndex, int count)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return string.Join(separator, value, startIndex, count);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is null or an <see cref="string.Empty"/> string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> is null or an empty string (""); otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><c>true</c> if the value parameter is null or an empty string (""), or if value consists exclusively of white-space characters; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}