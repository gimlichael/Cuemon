using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cuemon.Data
{
    /// <summary>
    /// Extension methods for the <see cref="QueryFormat"/> enum.
    /// </summary>
    public static class QueryFormatExtensions
    {
        /// <summary>
        /// Embeds the specified <see cref="int"/> sequence within the desired query fragment format.
        /// </summary>
        /// <param name="format">The <see cref="QueryFormat"/> to extend.</param>
        /// <param name="values">The values to be generated in the specified format for the query fragment.</param>
        /// <param name="distinct">if set to <c>true</c>, <paramref name="values"/> will be filtered for doublets.</param>
        /// <returns>A query fragment in the desired format.</returns>
        public static string Embed(this QueryFormat format, IEnumerable<int> values, bool distinct = false)
        {
            return Embed(format, DelimitedString.Create(values).Split(','), distinct);
        }

        /// <summary>
        /// Embeds the specified <see cref="long"/> sequence within the desired query fragment format.
        /// </summary>
        /// <param name="format">The <see cref="QueryFormat"/> to extend.</param>
        /// <param name="values">The values to be generated in the specified format for the query fragment.</param>
        /// <param name="distinct">if set to <c>true</c>, <paramref name="values"/> will be filtered for doublets.</param>
        /// <returns>A query fragment in the desired format.</returns>
        public static string Embed(this QueryFormat format, IEnumerable<long> values, bool distinct = false)
        {
            return Embed(format, DelimitedString.Create(values).Split(','), distinct);
        }

        /// <summary>
        /// Embeds the specified <see cref="string"/> sequence within the desired query fragment format.
        /// </summary>
        /// <param name="format">The <see cref="QueryFormat"/> to extend.</param>
        /// <param name="values">The values to be generated in the specified format for the query fragment.</param>
        /// <param name="distinct">if set to <c>true</c>, <paramref name="values"/> will be filtered for doublets.</param>
        /// <returns>A query fragment in the desired format.</returns>
        public static string Embed(this QueryFormat format, IEnumerable<string> values, bool distinct = false)
        {
            Validator.ThrowIfNull(values, nameof(values));
            Validator.ThrowIfEmptySequence(values, nameof(values));
            if (distinct) { values = new List<string>(values.Distinct()).ToArray(); }
            switch (format)
            {
                case QueryFormat.Delimited:
                    return DelimitedString.Create(values);
                case QueryFormat.DelimitedString:
                    return DelimitedString.Create(values, o =>
                    {
                        o.Delimiter = ",";
                        o.StringConverter = s => FormattableString.Invariant($"'{s}'");
                    });
                case QueryFormat.DelimitedSquareBracket:
                    return DelimitedString.Create(values, o =>
                    {
                        o.Delimiter = ",";
                        o.StringConverter = s => FormattableString.Invariant($"[{s}]");
                    });
                default:
                    throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(QueryFormat));
            }
        }
    }
}