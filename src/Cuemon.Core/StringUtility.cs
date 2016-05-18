using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="string"/> operations easier to work with.
    /// </summary>
    public static class StringUtility
    {
        private static readonly object PadLock = new object();
        private static readonly Dictionary<string, Regex> CompiledSplitExpressions = new Dictionary<string, Regex>();

        #region Constants
        /// <summary>
        /// Carriage-return/linefeed character combination.
        /// </summary>
        public const string NewLine = CarriageReturn + Linefeed;

        /// <summary>
        /// Tab character.
        /// </summary>
        public const string Tab = "\t";

        /// <summary>
        /// Linefeed character.
        /// </summary>
        public const string Linefeed = "\n";

        /// <summary>
        /// Carriage-return character.
        /// </summary>
        public const string CarriageReturn = "\r";

        /// <summary>
        /// A representation of a numeric character set consisting of the numbers 0 to 9.
        /// </summary>
        public const string NumericCharacters = "0123456789";

        /// <summary>
        /// A single case representation of an alphanumeric character set consisting of the numbers 0 to 9 and the letters A to Z.
        /// </summary>
        public const string AlphanumericCharactersSingleCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + NumericCharacters;

        /// <summary>
        /// A case sensitive representation of an alphanumeric character set consisting of the numbers 0 to 9 and the letters Aa to Zz.
        /// </summary>
        public const string AlphanumericCharactersCaseSensitive = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz" + NumericCharacters;

        /// <summary>
        /// An uppercase representation of the English alphabet character set consisting of the letters A to Z.
        /// </summary>
        public static readonly string EnglishAlphabetCharactersMajuscule = AlphanumericCharactersSingleCase.Remove(AlphanumericCharactersSingleCase.Length - 10);

        /// <summary>
        /// A lowercase representation of the English alphabet character set consisting of the letters a to z.
        /// </summary>
        public static readonly string EnglishAlphabetCharactersMinuscule = EnglishAlphabetCharactersMajuscule.ToLowerInvariant();

        /// <summary>
        /// A representation of the most common punctuation marks that conforms to the ASCII printable characters.
        /// </summary>
        public static readonly string PunctuationMarks = "!@#$%^&*()_-+=[{]};:<>|.,/?`~\\\"'";

        /// <summary>
        /// A representation of the most common whitespace characters.
        /// </summary>
        public static readonly string WhiteSpaceCharacters = "\u0009\u000A\u000B\u000C\u000D\u0020\u0085\u00A0\u1680\u180E\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u2028\u2029\u202F\u205F\u3000";

        /// <summary>
        /// A representation of a hexadecimal character set consisting of the numbers 0 to 9 and the letters A to F.
        /// </summary>
        public static readonly string HexadecimalCharacters = NumericCharacters + "ABCDEF";
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
        /// <remarks>This method will skip common Base64 structures typically used as checksums. This includes 32, 128, 160, 256, 384 and 512 bit checksums.</remarks>
        public static bool IsBase64(string value)
        {
            return IsBase64(value, IsValueWithValidBase64ChecksumLength);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <param name="predicate">A function delegate that provides custom rules for bypassing the Base64 structure check.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
        public static bool IsBase64(string value, Doer<string, bool> predicate)
        {
            byte[] result;
            return ByteConverter.TryFromBase64String(value, predicate, out result);
        }

        internal static bool IsValueWithValidBase64ChecksumLength(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            if (NumberUtility.IsEven(value.Length))
            {
                int commonChecksumLengths = value.Length / 4;
                switch (commonChecksumLengths)
                {
                    case 8:
                    case 32:
                    case 40:
                    case 64:
                    case 96:
                    case 128:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a sequence of countable characters (hence, characters being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="value">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a sequence of countable characters (hence, characters being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            if (value.Length < 2) { return false; }
            return NumberUtility.IsCountableSequence(value.Select(CastAsIntegralSequence));
        }

        private static long CastAsIntegralSequence(char character)
        {
            return character;
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
        public static string[] Split(string value)
        {
            return Split(value, ",");
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <returns>An array whose elements contain the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.</returns>
        public static string[] Split(string value, string delimiter)
        {
            return Split(value, delimiter, "\"");
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>. A parameter specifies the <paramref name="textQualifier"/> that surrounds a field.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="textQualifier">The text qualifier specification that surrounds a field.</param>
        /// <returns>An array whose elements contain the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.</returns>
        public static string[] Split(string value, string delimiter, string textQualifier)
        {
            return Split(value, delimiter, textQualifier, CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>. A parameter specifies the <paramref name="textQualifier"/> that surrounds a field.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="textQualifier">The text qualifier specification that surrounds a field.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>An array whose elements contain the substrings of <paramref name="value"/> that are delimited by <paramref name="delimiter"/>.</returns>
        /// <remarks>
        /// This method was inspired by an article on StackOverflow @ http://stackoverflow.com/questions/2807536/split-string-in-c-sharp.
        /// </remarks>
        public static string[] Split(string value, string delimiter, string textQualifier, IFormatProvider provider)
        {
            if (value == null) { return new string[] { null }; }
            if (delimiter == null) { delimiter = ","; }
            if (textQualifier == null) { textQualifier = "\""; }

            Regex compiledSplit;
            string key = string.Concat(delimiter, textQualifier);
            lock (PadLock)
            {
                if (!CompiledSplitExpressions.TryGetValue(key, out compiledSplit))
                {
                    compiledSplit = new Regex(string.Format(provider, @"{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", Regex.Escape(delimiter), Regex.Escape(textQualifier)),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    CompiledSplitExpressions.Add(key, compiledSplit);
                }
            }
            return compiledSplit.Split(value);
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
                int index = 0;
                while (index < value.Length)
                {
                    int smallestLength = Math.Min(length, value.Length - index);
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
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (trimChars == null || trimChars.Length == 0) { trimChars = WhiteSpaceCharacters.ToCharArray(); }
            List<char> result = new List<char>();
            foreach (char c in value)
            {
                bool skip = false;
                foreach (char t in trimChars)
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
        /// Computes a suitable hash code from the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> value.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static int GetHashCode(string value)
        {
            if (value == null) { return StructUtility.HashCodeForNullValue; }
            return StructUtility.GetHashCode32(value);
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
        public static int Count(string source, char character)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == character) { count++; }
            }
            return count;
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string Remove(string source, params string[] filter)
        {
            return Remove(source, StringComparison.Ordinal, filter);
        }
        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        public static string Remove(string source, StringComparison comparison, params string[] filter)
        {
            if (string.IsNullOrEmpty(source)) { return source; }
            if (filter == null || filter.Length == 0) { return source; }
            foreach (string f in filter)
            {
                source = Replace(source, f, "", comparison);
            }
            return source;
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="source"/> except for the removed characters.</returns>
        public static string Remove(string source, params char[] filter)
        {
            if (string.IsNullOrEmpty(source)) { return source; }
            StringBuilder result = new StringBuilder(source.Length);
            foreach (char c in source)
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
        public static string[] Remove(string[] source, params string[] filter)
        {
            return Remove(source, StringComparison.Ordinal, filter);
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The source array to delete occurrences found in <paramref name="filter"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="filter">The filter containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        public static string[] Remove(string[] source, StringComparison comparison, params string[] filter)
        {
            if (source == null || source.Length == 0) { return source; }
            if (filter == null || filter.Length == 0) { return source; }
            List<string> result = new List<string>();
            foreach (string s in source)
            {
                result.Add(Remove(s, comparison, filter));
            }
            return result.ToArray();
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
        public static string Shuffle(params string[] values)
        {
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            if (values.Length == 0) { throw new ArgumentOutOfRangeException(nameof(values), "You must specify at least one string value to shuffle."); }

            List<char> allChars = new List<char>();
            foreach (string value in values) { allChars.AddRange(value); }

            char[] result = allChars.ToArray();
            int allCharsLength = result.Length;
            while (allCharsLength > 1)
            {
                allCharsLength--;
                int random = NumberUtility.GetRandomNumber(0, allCharsLength + 1);
                char shuffledChar = result[random];
                result[random] = result[allCharsLength];
                result[allCharsLength] = shuffledChar;
            }
            return new string(result);
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
        public static string Shuffle(IEnumerable<string> values)
        {
            return Shuffle(values.ToArray());
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        /// <remarks>This method performs an <see cref="StringComparison.OrdinalIgnoreCase"/> search to find <paramref name="oldValue"/>.</remarks>
        public static string Replace(string value, string oldValue, string newValue)
        {
            return Replace(value, oldValue, newValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        public static string Replace(string value, string oldValue, string newValue, StringComparison comparison)
        {
            Validator.ThrowIfNull(oldValue, nameof(oldValue));
            Validator.ThrowIfNull(newValue, nameof(newValue));
            return Replace(value, EnumerableUtility.Yield(new StringReplacePair(oldValue, newValue)), comparison);
        }

        /// <summary>
        /// Replaces all occurrences of the <see cref="StringReplacePair.OldValue"/> with <see cref="StringReplacePair.NewValue"/> of the <paramref name="replacePairs"/> sequence in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="replacePairs">A sequence of <see cref="StringReplacePair"/> values.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <see cref="StringReplacePair.OldValue"/> replaced with <see cref="StringReplacePair.NewValue"/>.</returns>
        public static string Replace(string value, IEnumerable<StringReplacePair> replacePairs, StringComparison comparison)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(replacePairs, nameof(replacePairs));
            StringReplaceEngine replaceEngine = new StringReplaceEngine(value, replacePairs, comparison);
            return replaceEngine.ToString();
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
            foreach (string value in values)
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
        /// Generates a string from the specified Unicode character repeated until the specified length.
        /// </summary>
        /// <param name="character">A Unicode character.</param>
        /// <param name="length">The number of times <paramref name="character"/> occurs.</param>
        /// <returns>A <see cref="string"/> filled with the specified <paramref name="character"/> until the specified <paramref name="length"/>.</returns>
        public static string CreateFixedString(char character, int length)
        {
            return new string(character, length);
        }

        /// <summary>
        /// Generates a random string with the specified length using values of <see cref="AlphanumericCharactersCaseSensitive"/>.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <returns>A random string from the values of <see cref="AlphanumericCharactersCaseSensitive"/>.</returns>
        public static string CreateRandomString(int length)
        {
            return CreateRandomString(length, AlphanumericCharactersCaseSensitive);
        }

        /// <summary>
        /// Generates a random string with the specified length from the provided values.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <param name="values">The values to use in the randomization process.</param>
        /// <returns>A random string from the values provided.</returns>
        public static string CreateRandomString(int length, params string[] values)
        {
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = NumberUtility.GetRandomNumber(values.Length);
                int indexLength = values[index].Length;
                result.Append(values[index][NumberUtility.GetRandomNumber(indexLength)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Parses the given string for any format arguments (eg. Text{0}-{1}.).
        /// </summary>
        /// <param name="format">The desired string format to parse.</param>
        /// <param name="foundArguments">The number of arguments found in the string format.</param>
        /// <returns>
        /// 	<c>true</c> if one or more format arguments is found; otherwise <c>false</c>.
        /// </returns>
        public static bool ParseFormat(string format, out int foundArguments)
        {
            return ParseFormat(format, -1, out foundArguments);
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
        public static bool ParseFormat(string format, int maxArguments, out int foundArguments)
        {
            Regex rx = new Regex(@"{[\d]*?}");
            MatchCollection matches = rx.Matches(format);
            foundArguments = matches.Count;
            if (maxArguments == -1 && foundArguments == 0) { return false; }
            if (maxArguments == -1 && foundArguments > 0) { return true; }
            return (foundArguments == maxArguments);
        }

        /// <summary>
        /// Escapes the given <see cref="string"/> the same way as the well known JavaScrip escape() function.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        public static string Escape(string value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            StringBuilder builder = new StringBuilder(value.Length);
            foreach (char character in value)
            {
                if (DoEscapeOrUnescape(character))
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, character < Byte.MaxValue ? "%{0:x2}" : "%u{0:x4}", (uint)character);
                }
                else
                {
                    builder.Append(character);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Unescapes the given <see cref="string"/> the same way as the well known Javascript unescape() function.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to unescape.</param>
        /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
        public static string Unescape(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            StringBuilder builder = new StringBuilder(value);
            Regex unicode = new Regex("%u([0-9]|[a-f])([0-9]|[a-f])([0-9]|[a-f])([0-9]|[a-f])", RegexOptions.IgnoreCase);
            MatchCollection matches = unicode.Matches(value);
            foreach (Match unicodeMatch in matches)
            {
                builder.Replace(unicodeMatch.Value, Convert.ToChar(Int32.Parse(unicodeMatch.Value.Remove(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString());
            }

            for (byte i = Byte.MinValue; i < Byte.MaxValue; i++)
            {
                if (DoEscapeOrUnescape(i))
                {
                    builder.Replace(string.Format(CultureInfo.InvariantCulture, "%{0:x2}", i), Convert.ToChar(i).ToString());
                }
            }
            return builder.ToString();
        }

        private static bool DoEscapeOrUnescape(int charValue)
        {
            return ((charValue < 42 || charValue > 126) || (charValue > 57 && charValue < 64) || (charValue == 92));
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
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
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
            if (source == null) { throw new ArgumentNullException(nameof(value)); }
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
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            foreach (string valueToFind in values)
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
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            foreach (char value in values)
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
        /// <exception cref="System.ArgumentNullException">
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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="values"/> is null.
        /// </exception>
        public static bool Equals(string source, StringComparison comparison, params string[] values)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            foreach (string value in values)
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
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (startWithValues == null) { throw new ArgumentNullException(nameof(startWithValues)); }
            foreach (string startWithValue in startWithValues)
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

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(IEnumerable<string> source)
        {
            return IsSequenceOf<T>(source, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(IEnumerable<string> source, CultureInfo culture)
        {
            return IsSequenceOf<T>(source, culture, (ITypeDescriptorContext)null);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <param name="context">The type-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(IEnumerable<string> source, CultureInfo culture, ITypeDescriptorContext context)
        {
            return IsSequenceOfCore<T>(source, culture, context, null);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="parser">The function delegate that evaluates if the elements  of <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(IEnumerable<string> source, Doer<string, CultureInfo, bool> parser)
        {
            return IsSequenceOf<T>(source, CultureInfo.InvariantCulture, parser);
        }

        /// <summary>
        /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence in which to evaluate if a string value is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
        /// <param name="parser">The function delegate that evaluates if the elements  of <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool IsSequenceOf<T>(IEnumerable<string> source, CultureInfo culture, Doer<string, CultureInfo, bool> parser)
        {
            Validator.ThrowIfNull(parser, nameof(parser));
            return IsSequenceOfCore<T>(source, culture, null, parser);
        }

        private static bool IsSequenceOfCore<T>(IEnumerable<string> source, CultureInfo culture, ITypeDescriptorContext context, Doer<string, CultureInfo, bool> parser)
        {
            Validator.ThrowIfNull(source, nameof(source));
            bool converterHasValue = (parser != null);
            bool valid = true;
            foreach (string substring in source)
            {
                valid &= converterHasValue ? parser(substring, culture) : Converter.Parse(substring, CanConvertString<T>, culture, context);
            }
            return valid;
        }

        private static bool CanConvertString<T>(string s, CultureInfo culture, ITypeDescriptorContext context)
        {
            T result;
            return Converter.TryFromString(s, culture, context, out result);
        }

        #endregion
    }
}