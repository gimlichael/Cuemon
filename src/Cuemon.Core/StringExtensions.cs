using System;
using System.Globalization;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="string"/> class using various methods already found in the Microsoft .NET Framework.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the specified absolute <paramref name="uriString"/> to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="Uri"/> instance.</param>
        /// <returns>A <see cref="Uri"/> that corresponds to <paramref name="uriString"/>.</returns>
        /// <remarks>This method uses the <see cref="UriKind.Absolute"/> when converting the <paramref name="uriString"/>.</remarks>
        public static Uri ToUri(this string uriString)
        {
            return ToUri(uriString, UriKind.Absolute);
        }

        /// <summary>
        /// Converts the specified <paramref name="uriString"/> to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="Uri"/> instance.</param>
        /// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
        /// <returns>A <see cref="Uri"/> that corresponds to <paramref name="uriString"/> and <paramref name="uriKind"/>.</returns>
        public static Uri ToUri(this string uriString, UriKind uriKind)
        {
            Validator.ThrowIfNullOrEmpty(uriString, nameof(uriString));
            return new Uri(uriString, uriKind);
        }

        /// <summary>
        /// Replaces the <paramref name="format"/> item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return FormatWith(format, CultureInfo.InvariantCulture, args);
        }

        /// <summary>
        /// Replaces the <paramref name="format"/> item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
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

        /// <summary>
        /// Converts the specified <paramref name="value"/> to either lowercase, UPPERCASE, Title Case or unaltered using the specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="value">The value to convert to one of the values in <see cref="CasingMethod"/>.</param>
        /// <param name="method">The method to use in the conversion.</param>
        /// <returns>A <see cref="string"/> that corresponds to <paramref name="value"/> with the applied conversion <paramref name="method"/>.</returns>
        /// <remarks>Uses <see cref="CultureInfo.InvariantCulture"/> for the conversion.</remarks>
        public static string ToCasing(this string value, CasingMethod method)
        {
            return ToCasing(value, method, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to either lowercase, UPPERCASE, Title Case or unaltered using the specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="value">The value to convert to one of the values in <see cref="CasingMethod"/>.</param>
        /// <param name="method">The method to use in the conversion.</param>
        /// <param name="culture">The culture rules to apply the conversion.</param>
        /// <returns>A <see cref="string"/> that corresponds to <paramref name="value"/> with the applied conversion <paramref name="method"/>.</returns>
        public static string ToCasing(this string value, CasingMethod method, CultureInfo culture)
        {
            switch (method)
            {
                case CasingMethod.Default:
                    return value;
                case CasingMethod.LowerCase:
                    return culture.TextInfo.ToLower(value);
                case CasingMethod.TitleCase:
                    var toTitleCase = culture.TextInfo.GetType().GetMethod("ToTitleCase", ReflectionUtility.BindingInstancePublicAndPrivate);
                    return toTitleCase?.Invoke(culture.TextInfo, new[] { value }) as string;
                case CasingMethod.UpperCase:
                    return culture.TextInfo.ToUpper(value);
            }
            return value;
        }
    }
}