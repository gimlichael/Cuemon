using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Specifies allowed GUID formats in parsing related methods.
    /// </summary>
    [Flags]
    public enum GuidFormats
    {
        /// <summary>
        /// Specified the number format (N) which consist of 32 digits, eg. 12345678123412341234123456789abc.
        /// </summary>
        NumberFormat = 1,
        /// <summary>
        /// Specified the digit format (D) which consist of 32 digits separated by hyphens, eg. 12345678-1234-1234-1234-123456789abc.
        /// </summary>
        DigitFormat = 2,
        /// <summary>
        /// Specified the brace format (B) which consist of 32 digits separated by hyphens, enclosed in brackets, eg. {12345678-1234-1234-1234-123456789abc}.
        /// </summary>
        BraceFormat = 4,
        /// <summary>
        /// Specified the brace format (P) which consist of 32 digits separated by hyphens, enclosed in parentheses, eg. (12345678-1234-1234-1234-123456789abc).
        /// </summary>
        ParenthesisFormat = 8,
        /// <summary>
        /// Specified any of the supported GUID formats (N,D,B,P).
        /// </summary>
        Any = NumberFormat | DigitFormat | BraceFormat | ParenthesisFormat
    }

    /// <summary>
    /// This utility class is designed to make some <see cref="Guid"/> operations easier to work with.
    /// </summary>
    public static class GuidUtility
    {
        private static readonly List<char> Hexadecimal = new List<char>(EnumerableUtility.Concat(StringUtility.HexadecimalCharacters.ToCharArray(), "abcdef".ToCharArray()));

        /// <summary>
        /// Converts the string representation of a GUID to its equivalent <see cref="Guid"/> structure.
        /// </summary>
        /// <param name="value">The GUID to convert.</param>
        /// <param name="result">The structure that will contain the parsed value.</param>
        /// <returns><c>true</c> if the parse operation was successful; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method returns <c>false</c> if <paramref name="value"/> is <c>null</c> or not in a recognized format, and does not throw an exception.<br/>
        /// Default implementation for this overload evaluates only on <see cref="GuidFormats.DigitFormat"/> | <see cref="GuidFormats.BraceFormat"/> | <see cref="GuidFormats.ParenthesisFormat"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.NumberFormat"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static bool TryParse(string value, out Guid result)
        {
            return TryParse(value, GuidFormats.BraceFormat | GuidFormats.DigitFormat | GuidFormats.ParenthesisFormat, out result);
        }

        /// <summary>
        /// Converts the string representation of a GUID to its equivalent <see cref="Guid"/> structure.
        /// </summary>
        /// <param name="value">The GUID to convert.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="result">The structure that will contain the parsed value.</param>
        /// <returns><c>true</c> if the parse operation was successful; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method returns <c>false</c> if <paramref name="value"/> is <c>null</c> or not in a recognized format, and does not throw an exception.<br/>
        /// Only 32 digits (<see cref="GuidFormats.NumberFormat"/>); 32 digits separated by hyphens (<see cref="GuidFormats.DigitFormat"/>); 32 digits separated by hyphens, enclosed in brackets (<see cref="GuidFormats.BraceFormat"/>) and 32 digits separated by hyphens, enclosed in parentheses (<see cref="GuidFormats.ParenthesisFormat"/>) is supported.<br/>
        /// For more information refer to this page @ StackOverflow: http://stackoverflow.com/questions/968175/what-is-the-string-length-of-a-guid
        /// </remarks>
        public static bool TryParse(string value, GuidFormats format, out Guid result)
        {
            result = Guid.Empty;
            if (string.IsNullOrEmpty(value)) { return true; }
            if (value.Length < 32) { return false; }
            if (value.Length > 38) { return false; }

            int startPosition = 0;
            bool hasDashes = (value.IndexOf('-') > 0);
            bool hasBraces = (value.StartsWith("{", StringComparison.OrdinalIgnoreCase) && value.EndsWith("}", StringComparison.OrdinalIgnoreCase));
            bool hasParenthesis = (value.StartsWith("(", StringComparison.OrdinalIgnoreCase) && value.EndsWith(")", StringComparison.OrdinalIgnoreCase));


            if (hasBraces || hasParenthesis) // BraceFormat or ParenthesisFormat
            {
                if (!EnumUtility.HasFlag(format, GuidFormats.BraceFormat) && !EnumUtility.HasFlag(format, GuidFormats.ParenthesisFormat)) { return false; }
                if (value.Length != 38)
                {
                    return false;
                }
                startPosition = 1;
            }

            if (hasDashes) // DigitFormat
            {
                if (!EnumUtility.HasFlag(format, GuidFormats.DigitFormat) && startPosition == 0) { return false; }
                if ((!EnumUtility.HasFlag(format, GuidFormats.BraceFormat) && startPosition == 1) && (!EnumUtility.HasFlag(format, GuidFormats.ParenthesisFormat) && startPosition == 1)) { return false; }
                if (value.Length != 36 && startPosition == 0)
                {
                    return false;
                }

                if (value[startPosition + 8] != '-' ||
                    value[startPosition + 13] != '-' ||
                    value[startPosition + 18] != '-' ||
                    value[startPosition + 23] != '-')
                {
                    return false;
                }
            }
            else // NumberFormat
            {
                if (!EnumUtility.HasFlag(format, GuidFormats.NumberFormat)) { return false; }
                if (value.Length != 32)
                {
                    return false;
                }
            }


            if (!IsCharWiseValid(value, hasDashes, hasParenthesis, hasBraces)) { return false; }

            try
            {
                result = new Guid(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool IsCharWiseValid(string value, bool hasDashes, bool hasParenthesis, bool hasBraces)
        {
            if (hasDashes) { value = value.Replace("-", ""); }
            if (hasParenthesis) { value = value.Replace("(", "").Replace(")", ""); }
            if (hasBraces) { value = value.Replace("{", "").Replace("}", ""); }
            for (int i = 0; i < value.Length; i++)
            {
                if (!Hexadecimal.Contains(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
