using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JDataResult"/> class.
    /// </summary>
    public static class JDataResultExtensions
    {
        /// <summary>
        /// Flattens the entirety of the JSON hierarchical <paramref name="source"/> into an <see cref="IEnumerable{JDataResult}"/> sequence.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{JDataResult}"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="JDataResult"/> objects.</returns>
        public static IEnumerable<JDataResult> Flatten(this IEnumerable<JDataResult> source)
        {
            Validator.ThrowIfNull(source);
            return FlattenCore(source);
        }

        private static IEnumerable<JDataResult> FlattenCore(IEnumerable<JDataResult> source)
        {
            return source.SelectMany(s => s.Children.Any() ? Arguments.Yield(s).Concat(Flatten(s.Children)) : Arguments.Yield(s));
        }

        /// <summary>
        /// Extracts one or more values from JSON objects using the specified <paramref name="propertyNames"/> and <paramref name="extractor"/> delegate.
        /// </summary>
        /// <param name="source">The sequence of <see cref="JDataResult"/> to parse.</param>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="extractor">The delegate that will extract values from <paramref name="propertyNames"/>.</param>
        public static void ExtractObjectValues(this IEnumerable<JDataResult> source, string propertyNames, Action<IDictionary<string, JDataResult>> extractor)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNullOrWhitespace(propertyNames);
            Validator.ThrowIfNull(extractor);

            var names = propertyNames.Split(',').Select(s => s.Trim()).ToList();
            var partial = new List<JDataResult>();

            foreach (var jr in source)
            {
                if (names.Exists(s => s.Equals(jr.Path, StringComparison.OrdinalIgnoreCase)))
                {
                    partial.Add(jr);
                }

                if (partial.Count == names.Count)
                {
                    extractor(partial.ToDictionary(jk => jk.PropertyName, jv => jv));
                    partial.Clear();
                }
            }
        }

        /// <summary>
        /// Extracts one or more values from JSON arrays using the specified <paramref name="propertyNames"/> and <paramref name="extractor"/> delegate.
        /// </summary>
        /// <param name="source">The sequence of <see cref="JDataResult"/> to parse.</param>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="extractor">The delegate that will extract values from <paramref name="propertyNames"/>.</param>
        public static void ExtractArrayValues(this IEnumerable<JDataResult> source, string propertyNames, Action<IDictionary<string, IEnumerable<JDataResult>>> extractor)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNullOrWhitespace(propertyNames);
            Validator.ThrowIfNull(extractor);

            var names = propertyNames.Split(',').Select(s => s.Trim()).ToList();
            var partial = new List<JDataResult>();

            foreach (var jr in source)
            {
                if (names.Exists(s => s.Equals(jr.Path, StringComparison.OrdinalIgnoreCase) || (HasMatchWithAsterisk(s, jr.Path))))
                {
                    partial.Add(jr);
                }

                if (partial.Count == names.Count)
                {
                    extractor(partial.ToDictionary(jk => jk.PropertyName, jv => jv.Children as IEnumerable<JDataResult>));
                    partial.Clear();
                }
            }
        }

        private static bool HasMatchWithAsterisk(string s, string path)
        {
            return s.EndsWith("*") && path.Contains('.') && s.Remove(s.LastIndexOf('.')).Equals(path.Remove(path.LastIndexOf('.')));
        }
    }
}