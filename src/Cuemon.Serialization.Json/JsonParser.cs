using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Provides access to factory methods for creating <see cref="JsonParser{T}"/> instances.
    /// </summary>
    public static class JsonParser
    {
        internal static readonly Lazy<Regex> LazySquareBracketsRemover = new Lazy<Regex>(() => new Regex(@"(\[).*(\])", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled));

        /// <summary>
        /// Creates a new <see cref="JsonParser{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to apply a JSON document.</typeparam>
        /// <param name="instance">An initialized instance of <typeparamref name="T"/>.</param>
        /// <param name="reader">The <see cref="JsonReader"/> to apply to <paramref name="instance"/>.</param>
        /// <returns>A new instance of <see cref="JsonParser{T}"/>.</returns>
        public static JsonParser<T> Create<T>(T instance, JsonReader reader)
        {
            return new JsonParser<T>(instance, reader);
        }
    }

    /// <summary>
    /// Provides a way to parse and extract values from a JSON document.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply a JSON document.</typeparam>
    public class JsonParser<T>
    {
        internal JsonParser(T instance, JsonReader reader)
        {
            Validator.ThrowIfNull(instance, nameof(instance));
            Validator.ThrowIfNull(reader, nameof(reader));
            Reader = reader;
            Instance = instance;
        }

        private JsonReader Reader { get; }

        private T Instance { get; }

        /// <summary>
        /// Reads and extract the <paramref name="propertyNames"/> using the specified delegate <paramref name="parser"/>.
        /// </summary>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="parser">The delegate that will extract the <paramref name="propertyNames"/>.</param>
        /// <returns>A fully initialized instance of <typeparamref name="T"/>.</returns>
        public T Parse(string propertyNames, Action<T, object[]> parser)
        {
            Validator.ThrowIfNullOrWhitespace(propertyNames, nameof(propertyNames));
            Validator.ThrowIfNull(parser, nameof(parser));
            return Parse(propertyNames.Split(','), parser);
        }

        /// <summary>
        /// Reads and extract the <paramref name="propertyNames"/> using the specified delegate <paramref name="parser"/>.
        /// </summary>
        /// <param name="propertyNames">The property names (JSON path) to math in a JSON document.</param>
        /// <param name="parser">The delegate that will extract the <paramref name="propertyNames"/>.</param>
        /// <returns>A fully initialized instance of <typeparamref name="T"/>.</returns>
        public T Parse(string[] propertyNames, Action<T, object[]> parser)
        {
            var extractor = new JsonExtractor<T>(propertyNames, parser);
            var names = extractor.PropertyNames.ToList();
            var partial = new List<object>();
            while (Reader.Read())
            {
                if (Reader.Value == null) { continue; }

                var path = JsonParser.LazySquareBracketsRemover.Value.Replace(Reader.Path, "");
                if (names.Exists(s => s.Equals(path, StringComparison.OrdinalIgnoreCase)))
                {
                    Reader.Read();
                    if (Reader.TokenType == JsonToken.StartArray)
                    {
                        partial.Add(FillArray());
                    }
                    else
                    {
                        partial.Add(Reader.Value);
                    }
                }

                if (partial.Count == names.Count)
                {
                    extractor.Parser(Instance, partial.ToArray());
                    partial.Clear();
                }
            }
            return Instance;
        }

        private IEnumerable<object> FillArray()
        {
            var result = new List<object>();
            while (Reader.Read())
            {
                if (Reader.TokenType == JsonToken.EndArray) { break; }
                if (Reader.TokenType == JsonToken.StartObject) { throw new InvalidOperationException("Only simple array types are supported in this version."); }
                result.Add(Reader.Value);
            }
            return result;
        }
    }
}