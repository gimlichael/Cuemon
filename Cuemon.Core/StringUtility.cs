using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.ComponentModel.Parsers;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="string"/> operations easier to work with.
    /// </summary>
    public static class StringUtility
    {
        #region Methods
        /// <summary>
        /// Parses whether the two specified values has a distinct difference from each other.
        /// </summary>
        /// <param name="definite">The value that specifies valid characters.</param>
        /// <param name="arbitrary">The value to distinctively compare with <paramref name="definite"/>.</param>
        /// <param name="difference">The distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/> or <see cref="string.Empty"/> if no difference.</param>
        /// <returns>
        /// 	<c>true</c> if there is a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>; otherwise <c>false</c>.
        /// </returns>
        public static bool ParseDistinctDifference(string definite, string arbitrary, out string difference)
        {
            if (definite == null) { definite = string.Empty; }
            if (arbitrary == null) { arbitrary = string.Empty; }
            difference = string.Concat(arbitrary.Distinct().Except(definite.Distinct()));
            return difference.Any();
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="value">The <see cref="string"/> to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool Contains(string source, string value)
        {
            return Contains(source, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="value">The <see cref="string"/> to search within <paramref name="source"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string source, string value, StringComparison comparison)
        {
            if (source == null) { return false; }
            if (value == null) { return false; }
            return (source.IndexOf(value, comparison) >= 0);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="value">The <see cref="char"/> to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool Contains(string source, char value)
        {
            return Contains(source, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="value">The <see cref="char"/> to search within <paramref name="source"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string source, char value, StringComparison comparison)
        {
            if (source == null) { return false; }
            return (source.IndexOf(new string(value, 1), 0, source.Length, comparison) >= 0);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="values"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool Contains(string source, params string[] values)
        {
            return Contains(source, StringComparison.OrdinalIgnoreCase, values);
        }

        /// <summary>
        /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string source, StringComparison comparison, params string[] values)
        {
            if (source == null) { return false; }
            if (values == null) { return false; }
            foreach (var valueToFind in values)
            {
                if (Contains(source, valueToFind, comparison)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="values">The <see cref="char"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool Contains(string source, params char[] values)
        {
            return Contains(source, StringComparison.Ordinal, values);
        }

        /// <summary>
        /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="values">The <see cref="char"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string source, StringComparison comparison, params char[] values)
        {
            if (source == null) { return false; }
            if (values == null) { return false; }
            foreach (var value in values)
            {
                if (Contains(source, value, comparison)) { return true; }
            }
            return false;
        }
        #endregion
    }
}