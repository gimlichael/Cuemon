using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JDataResult"/> class.
    /// </summary>
    public static class JDataResultExtensions
    {
        /// <summary>
        /// Extracts one or more values from JSON objects using the specified <paramref name="propertyNames"/> and <paramref name="parser"/> delegate.
        /// </summary>
        /// <param name="source">The sequence of <see cref="JDataResult"/> to parse.</param>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="parser">The delegate that will extract values from <paramref name="propertyNames"/>.</param>
        public static void ExtractObjectValues(this IEnumerable<JDataResult> source, string propertyNames, Action<IDictionary<string, JDataResult>> parser)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrWhitespace(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));

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
                    parser(partial.ToDictionary(jk => jk.PropertyName, jv => jv));
                    partial.Clear();
                }
            }
        }

        /// <summary>
        /// Extracts one or more values from JSON arrays using the specified <paramref name="propertyNames"/> and <paramref name="parser"/> delegate.
        /// </summary>
        /// <param name="source">The sequence of <see cref="JDataResult"/> to parse.</param>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="parser">The delegate that will extract values from <paramref name="propertyNames"/>.</param>
        public static void ExtractArrayValues(this IEnumerable<JDataResult> source, string propertyNames, Action<IDictionary<string, IEnumerable<JDataResult>>> parser) 
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrWhitespace(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));

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
                    parser(partial.ToDictionary(jk => jk.PropertyName, jv => jv.Children as IEnumerable<JDataResult>));
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