using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Cuemon.Reflection;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Retrieves a substring from the specified <paramref name="value"/>. The substring starts at position 0 and continues until the first occurrence of <paramref name="match"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <param name="match">The match that will define the stopping point.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>A substring that contains only the value just before <paramref name="match"/>.</returns>
        public static string SubstringBefore(this string value, string match, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            var indexOf = value.IndexOf(match, comparisonType);
            return indexOf == -1 ? "" : value.Substring(0, indexOf);
        }

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
        /// Converts the specified <paramref name="value"/> to either lowercase, UPPERCASE, Title Case or unaltered.
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
                    return toTitleCase?.Invoke(culture.TextInfo, new object[] { value }) as string;
                case CasingMethod.UpperCase:
                    return culture.TextInfo.ToUpper(value);
            }
            return value;
        }

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

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the outcome of the conversion.</param>
        /// <returns>A <see cref="TimeSpan"/> that corresponds to <paramref name="value"/> from <paramref name="timeUnit"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// The <paramref name="value"/> paired with <paramref name="timeUnit"/> is outside its valid range.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="timeUnit"/> was outside its valid range.
        /// </exception>
        public static TimeSpan ToTimeSpan(this string value, TimeUnit timeUnit)
        {
            return TimeSpanConverter.FromString(value, timeUnit);
        }

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
        public static string RemoveAll(this string source, params string[] filter)
        {
            return StringUtility.RemoveAll(source, filter);
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
        public static string RemoveAll(this string source, StringComparison comparison, params string[] filter)
        {
            return StringUtility.RemoveAll(source, comparison, filter);
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
        public static string[] RemoveAll(this string[] source, params string[] filter)
        {
            return StringUtility.RemoveAll(source, filter);
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
        public static string[] RemoveAll(this string[] source, StringComparison comparison, params string[] filter)
        {
            return StringUtility.RemoveAll(source, comparison, filter);
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters.</returns>
        public static string RemoveAll(this string source, params char[] filter)
        {
            return StringUtility.RemoveAll(source, filter);
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
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A hexadecimal <see cref="String"/> representation of the characters in <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToHexadecimal(this string value, Action<EncodingOptions> setup = null)
        {
            return StringConverter.ToHexadecimal(value, setup);
        }

        /// <summary>
        /// Decodes a URL string token to its equivalent byte array using base 64 digits.
        /// </summary>
        /// <param name="value">The URL string token to decode.</param>
        /// <returns>The byte array containing the decoded URL string token.</returns>
        public static byte[] FromUrlEncodedBase64(this string value)
        {
            return ByteConverter.FromUrlEncodedBase64String(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array using <see cref="EncodingOptions.DefaultEncoding"/> for the encoding and <see cref="EncodingOptions.DefaultPreambleSequence"/> for any preamble sequences.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>A <b>byte array</b> containing the results of encoding the specified set of characters.</returns>
        public static byte[] ToByteArray(this string value)
        {
            return ByteConverter.FromString(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <b>byte array</b> containing the results of encoding the specified set of characters.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static byte[] ToByteArray(this string value, Action<EncodingOptions> setup)
        {
            return ByteConverter.FromString(value, setup);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric <paramref name="value"/> of one or more enumerated constants to an equivalent enumerated <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An enum of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, IConvertible
        {
            return EnumUtility.Parse<TEnum>(value, true);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric <paramref name="value"/> of one or more enumerated constants to an equivalent enumerated <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <returns>An enum of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase) where TEnum : struct, IConvertible
        {
            return EnumUtility.Parse<TEnum>(value, ignoreCase);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> of a GUID to its equivalent <see cref="Guid"/> structure.
        /// </summary>
        /// <param name="value">The GUID to be converted.</param>   
        /// <returns>A <see cref="Guid"/> that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// The specified <paramref name="value"/> was not recognized to be a GUID.
        /// </exception>
        public static Guid ToGuid(this string value)
        {
            return GuidConverter.FromString(value);
        }

        /// <summary>
        /// Converts the given <see cref="String"/> to an equivalent sequence of characters.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="String"/> value.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static char[] ToCharArray(this string value, Action<EncodingOptions> setup = null)
        {
            return CharConverter.FromString(value, setup);
        }
    }
}