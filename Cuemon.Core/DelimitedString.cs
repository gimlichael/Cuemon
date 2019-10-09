using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods to break a delimited string into substrings.
    /// </summary>
    public static class DelimitedString
    {
        private static readonly ConcurrentDictionary<string, Regex> CompiledSplitExpressions = new ConcurrentDictionary<string, Regex>();

        /// <summary>
        /// Creates a delimited string representation from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> of delimited values that is a result of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> cannot be null.
        /// </exception>
        public static string Create<T>(IEnumerable<T> source, Action<DelimitedStringOptions<T>> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var options = Patterns.Configure(setup);
            var delimitedValues = new StringBuilder();
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    delimitedValues.Append(FormattableString.Invariant($"{options.StringConverter(enumerator.Current)}{options.Delimiter}"));
                }
            }
            return delimitedValues.Length > 0 ? delimitedValues.ToString(0, delimitedValues.Length - options.Delimiter.Length) : delimitedValues.ToString();
        }

        /// <summary>
        /// Returns a <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> delimited by a <see cref="DelimitedStringOptions.Delimiter"/> that may be quoted by <see cref="DelimitedStringOptions.Qualifier"/>.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> delimited by a <see cref="DelimitedStringOptions.Delimiter"/> and optionally surrounded within <see cref="DelimitedStringOptions.Qualifier"/>.</returns>
        /// <remarks>
        /// This method was inspired by two articles on StackOverflow @ http://stackoverflow.com/questions/2807536/split-string-in-c-sharp and https://stackoverflow.com/questions/3776458/split-a-comma-separated-string-with-both-quoted-and-unquoted-strings.
        /// The default implementation conforms with the RFC-4180 standard.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// An error occurred while splitting <paramref name="value"/> into substrings separated by <see cref="DelimitedStringOptions.Delimiter"/> and quoted with <see cref="DelimitedStringOptions.Qualifier"/>.
        /// This is typically related to data corruption, eg. a field has not been properly closed with the <see cref="DelimitedStringOptions.Qualifier"/> specified.
        /// </exception>
        public static string[] Split(string value, Action<DelimitedStringOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            var options = Patterns.Configure(setup);
            var delimiter = options.Delimiter;
            var qualifier = options.Qualifier;
            var key = string.Concat(delimiter, "<-dq->", qualifier);
            if (!CompiledSplitExpressions.TryGetValue(key, out var compiledSplit))
            {
                compiledSplit = new Regex(string.Format(CultureInfo.InvariantCulture, "(?:^|{0})({1}(?:[^{1}]+|{1}{1})*{1}|[^{0}]*)", Regex.Escape(delimiter), Regex.Escape(qualifier)), RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(2));
                CompiledSplitExpressions.TryAdd(key, compiledSplit);
            }

            try
            {
                return compiledSplit.Matches(value).Cast<Match>().Where(m => m.Length > 0).Select(m => m.Value.TrimStart(delimiter.ToCharArray())).ToArray();
            }
            catch (RegexMatchTimeoutException)
            {
                throw new InvalidOperationException(FormattableString.Invariant($"An error occurred while splitting '{value}' into substrings separated by '{delimiter}' and quoted with '{qualifier}'. This is typically related to data corruption, eg. a field has not been properly closed with the {nameof(options.Qualifier)} specified."));
            }
        }
    }
}