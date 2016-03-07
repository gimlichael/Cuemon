using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="Condition"/> class.
    /// </summary>
    public static class ConditionExtension
    {
        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has a valid format of an email address.
        /// </summary>
        /// <param name="value">The string to verify has a valid format of an email address.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has a valid format of an email address; otherwise, <c>false</c>.</returns>
        public static bool IsEmailAddress(this string value)
        {
            return Condition.IsEmailAddress(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has a valid format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The string to verify has a valid format of a <see cref="Guid"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has a format of a <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This implementation only evaluates for GUID formats of: <see cref="GuidFormats.DigitFormat"/> | <see cref="GuidFormats.BraceFormat"/> | <see cref="GuidFormats.ParenthesisFormat"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.NumberFormat"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static bool IsGuid(this string value)
        {
            return Condition.IsGuid(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has a valid format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The string to verify has a valid format of a <see cref="Guid"/>.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has a format of a <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
        public static bool IsGuid(this string value, GuidFormats format)
        {
            return Condition.IsGuid(value, format);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The string to verify is hexadecimal.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is hexadecimal; otherwise, <c>false</c>.</returns>
        public static bool IsHex(this string value)
        {
            return Condition.IsHex(value);
        }

        /// <summary>
        /// Determines whether the specified value can be evaluated as a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <returns><c>true</c> if the specified value can be evaluated as a number; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method implements a default permitted format of <paramref name="value"/> as <see cref="NumberStyles.Number"/>.<br/>
        /// This method implements a default culture-specific formatting information about <paramref name="value"/> specified to <see cref="CultureInfo.InvariantCulture"/>.
        /// </remarks>
        public static bool IsNumeric(this string value)
        {
            return Condition.IsNumeric(value);
        }

        /// <summary>
        /// Determines whether the specified value can be evaluated as a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <returns><c>true</c> if the specified value can be evaluated as a number; otherwise, <c>false</c>.</returns>
        /// <remarks>This method implements a default culture-specific formatting information about <paramref name="value"/> specified to <see cref="CultureInfo.InvariantCulture"/>.</remarks>
        public static bool IsNumeric(this string value, NumberStyles style)
        {
            return Condition.IsNumeric(value, style);
        }

        /// <summary>
        /// Determines whether the specified value can be evaluated as a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
        /// <returns><c>true</c> if the specified value can be evaluated as a number; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this string value, NumberStyles style, IFormatProvider provider)
        {
            return Condition.IsNumeric(value, style, provider);
        }
    }
}