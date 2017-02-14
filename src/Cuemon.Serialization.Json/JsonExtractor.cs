using System;

namespace Cuemon.Serialization.Json
{

    internal class JsonExtractor<T>
    {
        internal JsonExtractor(string propertyNames, Action<T, object[]> parser)
        {
            Validator.ThrowIfNullOrWhitespace(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            PropertyNames = propertyNames.Split(',');
            Parser = parser;
        }

        internal JsonExtractor(string[] propertyNames, Action<T, object[]> parser)
        {
            Validator.ThrowIfNull(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            PropertyNames = propertyNames;
            Parser = parser;
        }

        internal string[] PropertyNames { get; }

        internal Action<T, object[]> Parser { get; }
    }
}