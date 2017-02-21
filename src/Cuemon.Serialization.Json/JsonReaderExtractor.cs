using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Serialization.Json
{

    internal class JsonReaderExtractor<T>
    {
        internal JsonReaderExtractor(string propertyNames, Action<T, IDictionary<string, JsonReaderResult>> parser)
        {
            Validator.ThrowIfNullOrWhitespace(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            PropertyNames = propertyNames.Split(',').Select(s => s.Trim()).ToArray();
            Parser = parser;
        }

        internal JsonReaderExtractor(string[] propertyNames, Action<T, IDictionary<string, JsonReaderResult>> parser)
        {
            Validator.ThrowIfNull(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            PropertyNames = propertyNames;
            Parser = parser;
        }

        internal JsonReaderExtractor(string propertyNames, Action<T, IDictionary<string, IEnumerable<JsonReaderResult>>> parser)
        {
            Validator.ThrowIfNullOrWhitespace(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            PropertyNames = propertyNames.Split(',').Select(s => s.Trim()).ToArray();
            ManyParser = parser;
        }

        internal JsonReaderExtractor(string[] propertyNames, Action<T, IDictionary<string, IEnumerable<JsonReaderResult>>> parser)
        {
            Validator.ThrowIfNull(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            PropertyNames = propertyNames;
            ManyParser = parser;
        }

        internal string[] PropertyNames { get; }

        internal Action<T, IDictionary<string, JsonReaderResult>> Parser { get; }

        internal Action<T, IDictionary<string, IEnumerable<JsonReaderResult>>> ManyParser { get; }
    }
}