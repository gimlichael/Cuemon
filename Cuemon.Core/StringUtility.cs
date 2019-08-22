using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cuemon.Collections.Generic;
using Cuemon.ComponentModel.Parsers;
using Cuemon.ComponentModel.TypeConverters;

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
        /// Determines whether the specified <paramref name="value"/> contains at least one of the succession <paramref name="characters"/> of <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The value to test for consecutive characters.</param>
        /// <param name="characters">The character to locate with the specified <paramref name="length"/>.</param>
        /// <param name="length">The number of characters in succession.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> contains at least one of the succession <paramref name="characters"/> of <paramref name="length"/>; otherwise, <c>false</c>.</returns>
        public static bool HasConsecutiveCharacters(string value, IEnumerable<char> characters, int length = 2)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            if (value.Length == 1) { return false; }
            if (characters == null) { return false; }
            foreach (var sc in characters)
            {
                if (HasConsecutiveCharacters(value, sc, length)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> contains a succession <paramref name="character"/> of <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The value to test for consecutive characters.</param>
        /// <param name="character">The characters to locate with the specified <paramref name="length"/>.</param>
        /// <param name="length">The number of characters in succession.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> contains a succession <paramref name="character"/> of <paramref name="length"/>; otherwise, <c>false</c>.</returns>
        public static bool HasConsecutiveCharacters(string value, char character, int length = 2)
        {
            if (length < 2) { length = 2; }
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            if (value.Length == 1) { return false; }
            return value.Contains(new string(character, length));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
        /// <remarks>This method will skip common Base64 structures typically used as checksums. This includes 32, 128, 160, 256, 384 and 512 bit checksums.</remarks>
        public static bool IsBase64(string value)
        {
            return IsValueWithValidBase64ChecksumLength(value) && ConvertFactory.UseParser<Base64StringParser>().TryParse(value, out _);
        }

        internal static bool IsValueWithValidBase64ChecksumLength(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            if (Condition.IsEven(value.Length))
            {
                return (value.Length % 4 == 0);
            }
            return false;
        }

        /// <summary>
        /// Returns a sequence that is chunked into string-slices having a length of 1024 that is equivalent to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="string" /> to chunk into a sequence of smaller string-slices for partitioned storage or similar.</param>
        /// <returns>A sequence that is chunked into string-slices having a length of 1024 that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static IEnumerable<string> Chunk(string value)
        {
            return Chunk(value, 1024);
        }

        /// <summary>
        /// Returns a sequence that is chunked into string-slices of the specified <paramref name="length"/> that is equivalent to <paramref name="value"/>. Default is 1024.
        /// </summary>
        /// <param name="value">A <see cref="string" /> to chunk into a sequence of smaller string-slices for partitioned storage or similar.</param>
        /// <param name="length">The desired length of each string-slice in the sequence.</param>
        /// <returns>A sequence that is chunked into string-slices of the specified <paramref name="length"/> that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is less or equal to 0.
        /// </exception>
        public static IEnumerable<string> Chunk(string value, int length)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfLowerThanOrEqual(length, 0, nameof(length));
            if (value.Length <= length)
            {
                yield return value;
            }
            else
            {
                var index = 0;
                while (index < value.Length)
                {
                    var smallestLength = Math.Min(length, value.Length - index);
                    yield return value.Substring(index, smallestLength);
                    index += smallestLength;
                }
            }
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

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to compare.</param>
        /// <param name="startWithValues">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(string value, IEnumerable<string> startWithValues)
        {
            return StartsWith(value, StringComparison.OrdinalIgnoreCase, startWithValues);
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to compare.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="startWithValues">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        public static bool StartsWith(string value, StringComparison comparison, IEnumerable<string> startWithValues)
        {
            if (value == null) { return false; }
            if (startWithValues == null) { return false; }
            foreach (var startWithValue in startWithValues)
            {
                if (value.StartsWith(startWithValue, comparison)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to compare.</param>
        /// <param name="startWithValues">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(string value, params string[] startWithValues)
        {
            return StartsWith(value, (IEnumerable<string>)startWithValues);
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to compare.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="startWithValues">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(string value, StringComparison comparison, params string[] startWithValues)
        {
            return StartsWith(value, comparison, (IEnumerable<string>)startWithValues);
        }
        #endregion
    }
}