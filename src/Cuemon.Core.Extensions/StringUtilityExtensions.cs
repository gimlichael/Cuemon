using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="StringUtility"/> class.
    /// </summary>
    public static class StringUtilityExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
        /// <remarks>This method will skip common Base64 structures typically used as checksums. This includes 32, 128, 160, 256, 384 and 512 bit checksums.</remarks>
        public static bool IsBase64(this string value)
        {
            return StringUtility.IsBase64(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <param name="predicate">A function delegate that provides custom rules for bypassing the Base64 structure check.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
        public static bool IsBase64(this string value, Func<string, bool> predicate)
        {
            return StringUtility.IsBase64(value, predicate);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a sequence of countable characters (hence, characters being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="value">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a sequence of countable characters (hence, characters being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(this string value)
        {
            return StringUtility.IsCountableSequence(value);
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> that are delimited by a comma (",").
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <returns>An array whose elements contain the substrings of <paramref name="value"/> that are delimited by a comma (",").</returns>
        /// <remarks>
        /// The following table shows the default values for the overloads of this method.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter</term>
        ///         <description>Default Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term>delimiter</term>
        ///         <description><c>,</c></description>
        ///     </item>
        ///     <item>
        ///         <term>textQualifier</term>
        ///         <description><c>"</c></description>
        ///     </item>
        ///     <item>
        ///         <term>provider</term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static string[] Split(this string value)
        {
            return StringUtility.Split(value);
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <returns>An array whose elements contain the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.</returns>
        public static string[] Split(this string value, string delimiter)
        {
            return StringUtility.Split(value, delimiter);
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>. A parameter specifies the <paramref name="textQualifier"/> that surrounds a field.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="textQualifier">The text qualifier specification that surrounds a field.</param>
        /// <returns>An array whose elements contain the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.</returns>
        public static string[] Split(this string value, string delimiter, string textQualifier)
        {
            return StringUtility.Split(value, delimiter, textQualifier);
        }

        /// <summary>
        /// Computes a suitable hash code from the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> value.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static int GetHashCode32(this string value)
        {
            return StringUtility.GetHashCode(value);
        }

        /// <summary>
        /// Counts the occurrences of <paramref name="character"/> in the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to count occurrences of <paramref name="character"/>.</param>
        /// <param name="character">The <see cref="char"/> value to count in <paramref name="source"/>.</param>
        /// <returns>The number of times the <paramref name="character"/> was found in the <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static int Count(this string source, char character)
        {
            return StringUtility.Count(source, character);
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null or <paramref name="filter"/> is null.
        /// </exception>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string Remove(this string source, params string[] filter)
        {
            return StringUtility.Remove(source, filter);
        }
        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null or <paramref name="filter"/> is null.
        /// </exception>
        public static string Remove(this string source, StringComparison comparison, params string[] filter)
        {
            return StringUtility.Remove(source, comparison, filter);
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The source array to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null or <paramref name="filter"/> is null.
        /// </exception>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string[] Remove(this string[] source, params string[] filter)
        {
            return StringUtility.Remove(source, filter);
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The source array to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null or <paramref name="filter"/> is null.
        /// </exception>
        public static string[] Remove(this string[] source, StringComparison comparison, params string[] filter)
        {
            return StringUtility.Remove(source, comparison, filter);
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters.</returns>
        public static string Remove(this string source, params char[] filter)
        {
            return StringUtility.Remove(source, filter);
        }

        /// <summary>
        /// Shuffles the specified <paramref name="values"/> like a deck of cards.
        /// </summary>
        /// <param name="values">The values to be shuffled in the randomization process.</param>
        /// <returns>A random string from the shuffled <paramref name="values"/> provided.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="values"/> is empty.
        /// </exception>
        public static string Shuffle(this IEnumerable<string> values)
        {
            return StringUtility.Shuffle(values);
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to perform the replacement on.</param>
        /// <param name="oldValue">The <see cref="String"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="String"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <returns>A <see cref="String"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        /// <remarks>This method performs an <see cref="StringComparison.OrdinalIgnoreCase"/> search to find <paramref name="oldValue"/>.</remarks>
        public static string ReplaceAll(this string value, string oldValue, string newValue)
        {
            return StringUtility.Replace(value, oldValue, newValue);
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to perform the replacement on.</param>
        /// <param name="oldValue">The <see cref="String"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="String"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A <see cref="String"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        public static string ReplaceAll(this string value, string oldValue, string newValue, StringComparison comparison)
        {
            return StringUtility.Replace(value, oldValue, newValue, comparison);
        }

        /// <summary>
        /// Replaces all occurrences of the <see cref="StringReplacePair.OldValue"/> with <see cref="StringReplacePair.NewValue"/> of the <paramref name="replacePairs"/> sequence in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to perform the replacement on.</param>
        /// <param name="replacePairs">A sequence of <see cref="StringReplacePair"/> values.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A <see cref="String"/> equivalent to <paramref name="value"/> but with all instances of <see cref="StringReplacePair.OldValue"/> replaced with <see cref="StringReplacePair.NewValue"/>.</returns>
        public static string ReplaceAll(this string value, IEnumerable<StringReplacePair> replacePairs, StringComparison comparison)
        {
            return StringUtility.Replace(value, replacePairs, comparison);
        }

        /// <summary>
        /// Determines whether a string sequence has at least one value that equals to null or empty.
        /// </summary>
        /// <param name="values">A string sequence in which to test for the presence of null or empty.</param>
        /// <returns>
        /// 	<c>true</c> if a string sequence has at least one value that equals to null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this IEnumerable<string> values)
        {
            return StringUtility.IsNullOrEmpty(values);
        }

        /// <summary>
        /// Parses the given string for any format arguments (eg. Text{0}-{1}.).
        /// </summary>
        /// <param name="format">The desired string format to parse.</param>
        /// <param name="foundArguments">The number of arguments found in the string format.</param>
        /// <returns>
        /// 	<c>true</c> if one or more format arguments is found; otherwise <c>false</c>.
        /// </returns>
        public static bool ParseFormat(this string format, out int foundArguments)
        {
            return StringUtility.ParseFormat(format, out foundArguments);
        }

        /// <summary>
        /// Parses the given string for any format arguments (eg. Text{0}-{1}.).
        /// </summary>
        /// <param name="format">The desired string format to parse.</param>
        /// <param name="maxArguments">The maximum allowed arguments in the string format.</param>
        /// <param name="foundArguments">The number of arguments found in the string format.</param>
        /// <returns>
        /// 	<c>true</c> if one or more format arguments is found and the found arguments does not exceed the maxArguments parameter; otherwise <c>false</c>.
        /// </returns>
        public static bool ParseFormat(this string format, int maxArguments, out int foundArguments)
        {
            return StringUtility.ParseFormat(format, maxArguments, out foundArguments);
        }

        /// <summary>
        /// Escapes the given <see cref="String"/> the same way as the well known JavaScrip escape() function.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        public static string Escape(this string value)
        {
            return StringUtility.Escape(value);
        }

        /// <summary>
        /// Unescapes the given <see cref="String"/> the same way as the well known Javascript unescape() function.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to unescape.</param>
        /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
        public static string Unescape(this string value)
        {
            return StringUtility.Unescape(value);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="value">The <see cref="String"/> to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool ContainsAny(this string source, string value)
        {
            return StringUtility.Contains(source, value);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="value">The <see cref="String"/> to search within <paramref name="source"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string source, string value, StringComparison comparison)
        {
            return StringUtility.Contains(source, comparison, value);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="value">The <see cref="char"/> to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool ContainsAny(this string source, char value)
        {
            return StringUtility.Contains(source, value);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="value">The <see cref="char"/> to search within <paramref name="source"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="value"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string source, char value, StringComparison comparison)
        {
            return StringUtility.Contains(source, comparison, value);
        }

        /// <summary>
        /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="values">The <see cref="String"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool ContainsAny(this string source, params string[] values)
        {
            return StringUtility.Contains(source, values);
        }

        /// <summary>
        /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="values">The <see cref="String"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string source, StringComparison comparison, params string[] values)
        {
            return StringUtility.Contains(source, comparison, values);
        }

        /// <summary>
        /// Returns a value indicating whether all of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="values">The <see cref="String"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if all of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool ContainsAll(this string source, params string[] values)
        {
            return ContainsAll(source, StringComparison.OrdinalIgnoreCase, values);
        }

        /// <summary>
        /// Returns a value indicating whether all of the specified <paramref name="values"/> occurs within the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="values">The <see cref="String"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if all of the <paramref name="values"/> occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAll(this string source, StringComparison comparison, params string[] values)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(values, nameof(values));
            bool result = true;
            foreach (string value in values)
            {
                result &= ContainsAny(source, comparison, value);
            }
            return result;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="values"/> occurs within the <paramref name="source"/> object.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="values">The <see cref="char"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="values"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool ContainsAny(this string source, params char[] values)
        {
            return StringUtility.Contains(source, values);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="values"/> occurs within the <paramref name="source"/> object.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="values">The <see cref="char"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="values"/> parameter occurs within the <paramref name="source"/>, or if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string source, StringComparison comparison, params char[] values)
        {
            return StringUtility.Contains(source, comparison, values);
        }

        /// <summary>
        /// Returns a value indicating the specified <paramref name="source"/> equals one of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="values">The <see cref="String"/> sequence to search within <paramref name="source"/>.</param>
        /// <returns><c>true</c> if one the <paramref name="values"/> is the same as the <paramref name="source"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="values"/> is null.
        /// </exception>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static bool EqualsAny(this string source, params string[] values)
        {
            return StringUtility.Equals(source, values);
        }

        /// <summary>
        /// Returns a value indicating the specified <paramref name="source"/> equals one of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="source">The <see cref="String"/> to seek.</param>
        /// <param name="values">The <see cref="String"/> sequence to search within <paramref name="source"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><c>true</c> if one the <paramref name="values"/> is the same as the <paramref name="source"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="values"/> is null.
        /// </exception>
        public static bool EqualsAny(this string source, StringComparison comparison, params string[] values)
        {
            return StringUtility.Equals(source, comparison, values);
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="String"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to compare.</param>
        /// <param name="startWithValues">A sequence of <see cref="String"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this string value, IEnumerable<string> startWithValues)
        {
            return StringUtility.StartsWith(value, startWithValues);
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="String"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to compare.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="startWithValues">A sequence of <see cref="String"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        public static bool StartsWith(this string value, StringComparison comparison, IEnumerable<string> startWithValues)
        {
            return StringUtility.StartsWith(value, comparison, startWithValues);
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="String"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to compare.</param>
        /// <param name="startWithValues">A sequence of <see cref="String"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this string value, params string[] startWithValues)
        {
            return StringUtility.StartsWith(value, startWithValues);
        }

        /// <summary>
        /// Determines whether the beginning of an instance of <see cref="String"/> matches at least one string in the specified sequence of strings.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to compare.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="startWithValues">A sequence of <see cref="String"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this string value, StringComparison comparison, params string[] startWithValues)
        {
            return StringUtility.StartsWith(value, comparison, startWithValues);
        }

        /// <summary>
        /// Removes all occurrences of white-space characters from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="String"/> value.</param>
        /// <returns>The string that remains after all occurrences of white-space characters are removed from the specified <paramref name="value"/>.</returns>
        public static string TrimAll(this string value)
        {
            return StringUtility.TrimAll(value);
        }

        /// <summary>
        /// Removes all occurrences of a set of characters specified in <paramref name="trimChars"/> from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="String"/> value.</param>
        /// <param name="trimChars">An array of Unicode characters to remove.</param>
        /// <returns>The string that remains after all occurrences of the characters in the <paramref name="trimChars"/> parameter are removed from the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string TrimAll(this string value, params char[] trimChars)
        {
            return StringUtility.TrimAll(value, trimChars);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(this IEnumerable<string> source)
        {
            return StringUtility.IsSequenceOf<T>(source);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(this IEnumerable<string> source, CultureInfo culture)
        {
            return StringUtility.IsSequenceOf<T>(source, culture);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="parser">The function delegate that evaluates if the elements  of <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(this IEnumerable<string> source, Func<string, CultureInfo, bool> parser)
        {
            return StringUtility.IsSequenceOf<T>(source, parser);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <param name="parser">The function delegate that evaluates if the elements  of <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(this IEnumerable<string> source, CultureInfo culture, Func<string, CultureInfo, bool> parser)
        {
            return StringUtility.IsSequenceOf<T>(source, culture, parser);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <param name="context">The type-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(this IEnumerable<string> source, CultureInfo culture, ITypeDescriptorContext context)
        {
            return StringUtility.IsSequenceOf<T>(source, culture, context);
        }
    }
}