using System;
using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<string, Regex> CompiledSplitExpressions = new ConcurrentDictionary<string, Regex>();

        #region Constants
        /// <summary>
        /// A network-path reference, eg. two forward slashes (//).
        /// </summary>
        public const string NetworkPathReference = "//";

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
            difference = StringConverter.FromChars(arbitrary.Distinct().Except(definite.Distinct()));
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
            return IsBase64(value, IsValueWithValidBase64ChecksumLength);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <param name="predicate">A function delegate that provides custom rules for bypassing the Base64 structure check.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
        public static bool IsBase64(string value, Func<string, bool> predicate)
        {
            return ByteConverter.TryFromBase64String(value, predicate, out _);
        }

        internal static bool IsValueWithValidBase64ChecksumLength(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            if (NumberUtility.IsEven(value.Length))
            {
                return (value.Length % 4 == 0);
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
        /// Returns a <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> delimited by a <paramref name="delimiter"/> that may be quoted by <paramref name="qualifier"/>.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter that seperates the fields.</param>
        /// <param name="qualifier">The qualifier placed around each field to signify that it is the same field.</param>
        /// <returns>A <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> delimited by a <paramref name="delimiter"/> and optionally surrounded within <paramref name="qualifier"/>.</returns>
        /// <remarks>
        /// This method was inspired by two articles on StackOverflow @ http://stackoverflow.com/questions/2807536/split-string-in-c-sharp and https://stackoverflow.com/questions/3776458/split-a-comma-separated-string-with-both-quoted-and-unquoted-strings.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="delimiter"/> is null -or-
        /// <paramref name="qualifier"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is empty or consist only of white-space characters -or-
        /// <paramref name="delimiter"/> is empty or consist only of white-space characters -or-
        /// <paramref name="qualifier"/> is empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An error occured while splitting <paramref name="value"/> into substrings seperated by <paramref name="delimiter"/> and quoted with <paramref name="qualifier"/>.
        /// This is typically related to data corruption, eg. a field has not been properly closed with the <paramref name="qualifier"/> specified.
        /// </exception>
        public static string[] SplitDsv(string value, string delimiter, string qualifier)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            Validator.ThrowIfNullOrWhitespace(delimiter, nameof(delimiter));
            Validator.ThrowIfNullOrWhitespace(qualifier, nameof(qualifier));

            var key = string.Concat(delimiter, "<-dq->", qualifier);
            if (!CompiledSplitExpressions.TryGetValue(key, out var compiledSplit))
            {
                compiledSplit = new Regex(string.Format(CultureInfo.InvariantCulture, "(?:^|{0})({1}(?:[^{1}]+|{1}{1})*{1}|[^{0}]*)", Regex.Escape(delimiter), Regex.Escape(qualifier)), RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(2));
                CompiledSplitExpressions.TryAdd(key, compiledSplit);
            }

            try
            {
                return compiledSplit.Matches(value).Cast<Match>().Where(m => m.Length > 0).Select(m => m.Value.TrimStart(delimiter.ToCharArray()).TrimStart(qualifier.ToCharArray()).TrimEnd(qualifier.ToCharArray())).ToArray();
            }
            catch (RegexMatchTimeoutException)
            {
                throw new InvalidOperationException($"An error occured while splitting '{value}' into substrings seperated by '{delimiter}' and quoted with '{qualifier}'. This is typically related to data corruption, eg. a field has not been properly closed with the {nameof(qualifier)} specified.");
            }
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> delimited by a comma (",") that may be quoted with double quotes ("").
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <returns>A <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> that are delimited by a comma (",").</returns>
        /// <remarks>Conforms with the RFC-4180 standard.</remarks>
        public static string[] SplitCsvQuoted(string value)
        {
            return SplitDsv(value, ",", "\"");
        }

        /// <summary>
        /// Returns a string array that contains the substrings of <paramref name="value"/> delimited by a <paramref name="delimiter"/> that may be quoted with double quotes ("").
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="delimiter">The delimiter that seperates the fields.</param>
        /// <returns>A <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> that are delimited by a <paramref name="delimiter"/>.</returns>
        public static string[] SplitDsvQuoted(string value, string delimiter)
        {
            return SplitDsv(value, delimiter, "\"");
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
            if (trimChars == null || trimChars.Length == 0) { trimChars = WhiteSpaceCharacters.ToCharArray(); }
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
        /// Computes a suitable hash code from the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A <see cref="string"/> value.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static int GetHashCode(string value)
        {
            if (value == null) { return StructUtility.HashCodeForNullValue; }
            return StructUtility.GetHashCode32(value);
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
                value = Replace(value, f, "", comparison);
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
        /// Shuffles the specified <paramref name="values"/> like a deck of cards.
        /// </summary>
        /// <param name="values">The values to be shuffled in the randomization process.</param>
        /// <returns>A random string from the shuffled <paramref name="values"/> provided.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="values"/> is empty.
        /// </exception>
        public static string Shuffle(params string[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            Validator.ThrowIfEqual(0, values.Length, nameof(values), "You must specify at least one string value to shuffle.");

            var allChars = new List<char>();
            foreach (var value in values) { allChars.AddRange(value); }

            var result = allChars.ToArray();
            var allCharsLength = result.Length;
            while (allCharsLength > 1)
            {
                allCharsLength--;
                var random = NumberUtility.GetRandomNumber(0, allCharsLength + 1);
                var shuffledChar = result[random];
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
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
            var replaceEngine = new StringReplaceEngine(value, replacePairs, comparison);
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
            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var index = NumberUtility.GetRandomNumber(values.Length);
                var indexLength = values[index].Length;
                result.Append(values[index][NumberUtility.GetRandomNumber(indexLength)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Escapes the given <see cref="string"/> the same way as the well known JavaScript escape() function.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        public static string Escape(string value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            var builder = new StringBuilder(value.Length);
            foreach (var character in value)
            {
                if (DoEscapeOrUnescape(character))
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, character < byte.MaxValue ? "%{0:x2}" : "%u{0:x4}", (uint)character);
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
            var builder = new StringBuilder(value);
            var unicode = new Regex("%u([0-9]|[a-f])([0-9]|[a-f])([0-9]|[a-f])([0-9]|[a-f])", RegexOptions.IgnoreCase);
            var matches = unicode.Matches(value);
            foreach (Match unicodeMatch in matches)
            {
                builder.Replace(unicodeMatch.Value, Convert.ToChar(int.Parse(unicodeMatch.Value.Remove(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString());
            }

            for (var i = byte.MinValue; i < byte.MaxValue; i++)
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
        public static bool IsSequenceOf<T>(IEnumerable<string> source, Func<string, CultureInfo, bool> parser)
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
        public static bool IsSequenceOf<T>(IEnumerable<string> source, CultureInfo culture, Func<string, CultureInfo, bool> parser)
        {
            Validator.ThrowIfNull(parser, nameof(parser));
            return IsSequenceOfCore<T>(source, culture, null, parser);
        }

        private static bool IsSequenceOfCore<T>(IEnumerable<string> source, CultureInfo culture, ITypeDescriptorContext context, Func<string, CultureInfo, bool> parser)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var converterHasValue = (parser != null);
            var valid = true;
            foreach (var substring in source)
            {
                valid &= converterHasValue ? parser(substring, culture) : Converter.Parse(substring, CanConvertString<T>, culture, context);
            }
            return valid;
        }

        private static bool CanConvertString<T>(string s, CultureInfo culture, ITypeDescriptorContext context)
        {
            return Converter.TryFromString<T>(s, culture, context, out _);
        }
        #endregion
    }
}