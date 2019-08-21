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
        /// Removes all occurrences of white-space characters from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> value.</param>
        /// <returns>The string that remains after all occurrences of white-space characters are removed from the specified <paramref name="value"/>.</returns>
        public static string TrimAll(string value)
        {
            return TrimAll(value, null);
        }

        /// <summary>
        /// Removes all occurrences of a set of characters specified in <paramref name="trimChars"/> from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> value.</param>
        /// <param name="trimChars">An array of Unicode characters to remove.</param>
        /// <returns>The string that remains after all occurrences of the characters in the <paramref name="trimChars"/> parameter are removed from the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string TrimAll(string value, params char[] trimChars)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (trimChars == null || trimChars.Length == 0) { trimChars = Alphanumeric.WhiteSpace.ToCharArray(); }
            var result = new List<char>();
            foreach (var c in value)
            {
                var skip = false;
                foreach (var t in trimChars)
                {
                    if (c.Equals(t))
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip) { result.Add(c); }
            }
            return new string(result.ToArray());
        }

        /// <summary>
        /// Counts the occurrences of <paramref name="character"/> in the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The source to count occurrences of <paramref name="character"/>.</param>
        /// <param name="character">The <see cref="char"/> value to count in <paramref name="value"/>.</param>
        /// <returns>The number of times the <paramref name="character"/> was found in the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static int Count(string value, char character)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var count = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] == character) { count++; }
            }
            return count;
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters and/or words.</returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string RemoveAll(string value, params string[] filter)
        {
            return RemoveAll(value, StringComparison.Ordinal, filter);
        }
        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters and/or words.</returns>
        public static string RemoveAll(string value, StringComparison comparison, params string[] filter)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            if (filter == null || filter.Length == 0) { return value; }
            foreach (var f in filter)
            {
                value = StringReplacePair.ReplaceAll(value, f, "", comparison);
            }
            return value;
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters.</returns>
        public static string RemoveAll(string value, params char[] filter)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            var result = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                if (filter.Contains(c)) { continue; }
                result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The source array to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string[] RemoveAll(string[] source, params string[] filter)
        {
            return RemoveAll(source, StringComparison.Ordinal, filter);
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The source array to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        public static string[] RemoveAll(string[] source, StringComparison comparison, params string[] filter)
        {
            if (source == null || source.Length == 0) { return source; }
            if (filter == null || filter.Length == 0) { return source; }
            var result = new List<string>();
            foreach (var s in source)
            {
                result.Add(RemoveAll(s, comparison, filter));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Determines whether a string sequence has at least one value that equals to null or empty.
        /// </summary>
        /// <param name="values">A string sequence in which to test for the presence of null or empty.</param>
        /// <returns>
        /// 	<c>true</c> if a string sequence has at least one value that equals to null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(IEnumerable<string> values)
        {
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether one or more string values equals to null or empty.
        /// </summary>
        /// <param name="values">One or more string values to test for the presence of null or empty.</param>
        /// <returns>
        /// 	<c>true</c> if one or more string values equals to null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(params string[] values)
        {
            return IsNullOrEmpty((IEnumerable<string>)values);
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
        /// Returns a value indicating the specified <paramref name="source"/> equals one of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns><c>true</c> if one the <paramref name="values"/> is the same as the <paramref name="source"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="values"/> is null.
        /// </exception>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool Equals(string source, params string[] values)
        {
            return Equals(source, StringComparison.Ordinal, values);
        }

        /// <summary>
        /// Returns a value indicating the specified <paramref name="source"/> equals one of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to seek.</param>
        /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="source"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><c>true</c> if one the <paramref name="values"/> is the same as the <paramref name="source"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="values"/> is null.
        /// </exception>
        public static bool Equals(string source, StringComparison comparison, params string[] values)
        {
            if (source == null) { return false; }
            if (values == null) { return false; }
            foreach (var value in values)
            {
                if (source.Equals(value, comparison)) { return true; }
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