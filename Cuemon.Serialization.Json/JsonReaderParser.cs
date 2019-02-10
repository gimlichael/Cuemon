using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Provides access to factory methods for creating <see cref="JsonReaderParser{T}"/> instances.
    /// </summary>
    public static class JsonReaderParser
    {
        /// <summary>
        /// Creates a new <see cref="JsonReaderParser{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to apply a JSON document.</typeparam>
        /// <param name="instance">An initialized instance of <typeparamref name="T"/>.</param>
        /// <param name="reader">The <see cref="JsonReader"/> to apply to <paramref name="instance"/>.</param>
        /// <returns>A new instance of <see cref="JsonReaderParser{T}"/>.</returns>
        public static JsonReaderParser<T> Create<T>(T instance, JsonReader reader)
        {
            Validator.ThrowIfNull(instance, nameof(instance));
            Validator.ThrowIfNull(reader, nameof(reader));
            return new JsonReaderParser<T>(instance, reader);
        }
    }

    /// <summary>
    /// Provides a way to parse and extract values from a JSON document.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply a JSON document.</typeparam>
    public class JsonReaderParser<T>
    {
        internal JsonReaderParser(T instance, JsonReader reader)
        {
            Instance = instance;
            Reader = reader;
            Result = new Lazy<List<JsonReaderResult>>(() =>
            {
                var result = new List<JsonReaderResult>();
                while (Reader.Read())
                {
                    if (Reader.Value == null) { continue; }
                    var jr = new JsonReaderResult();
                    switch (Reader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            jr.PropertyName = Reader.Value.ToString();
                            Reader.Read();
                            break;
                    }
                    if (Reader.TokenType == JsonToken.StartArray)
                    {
                        jr.Children = FillArray(jr);
                    }
                    else
                    {
                        jr.Value = Reader.Value;
                    }
                    jr.Path = Reader.Path.RemoveBrackets();
                    jr.Type = Reader.ValueType ?? typeof(Array);
                    result.Add(jr);
                }
                return result;
            });
        }

        private JsonReader Reader { get; }

        /// <summary>
        /// Gets the instance specified on <see cref="JsonReaderParser.Create{T}"/>, but initialized with <see cref="GetValues"/> and/or <see cref="GetValuesFromArray"/>.
        /// </summary>
        /// <value>The instance specified on <see cref="JsonReaderParser.Create{T}"/>.</value>
        public T Instance { get; }


        /// <summary>
        /// Retrieves one or more values from the specified <paramref name="propertyNames"/> using the specified delegate <paramref name="parser"/>.
        /// </summary>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="parser">The delegate that will extract values from <paramref name="propertyNames"/>.</param>
        /// <returns>A reference to this <see cref="JsonReaderParser{T}"/> instance.</returns>
        public JsonReaderParser<T> GetValues(string propertyNames, Action<T, IDictionary<string, JsonReaderResult>> parser)
        {
            var json = Result.Value;
            var extractor = new JsonReaderExtractor<T>(propertyNames, parser);
            var names = extractor.PropertyNames.ToList();
            var partial = new List<JsonReaderResult>();

            foreach (var jr in json)
            {
                if (names.Exists(s => s.Equals(jr.Path, StringComparison.OrdinalIgnoreCase)))
                {
                    partial.Add(jr);
                }

                if (partial.Count == names.Count)
                {
                    extractor.Parser(Instance, partial.ToDictionary(jk => jk.PropertyName, jv => jv));
                    partial.Clear();
                }
            }
            return this;
        }

        /// <summary>
        /// Retreives one or more values from the specified <paramref name="propertyNames"/> using the specified delegate <paramref name="parser"/>. Optimized for JSON arrays.
        /// </summary>
        /// <param name="propertyNames">The comma-delimited property names (JSON path) to math in a JSON document.</param>
        /// <param name="parser">The delegate that will extract values from <paramref name="propertyNames"/>.</param>
        /// <returns>A reference to this <see cref="JsonReaderParser{T}"/> instance.</returns>
        public JsonReaderParser<T> GetValuesFromArray(string propertyNames, Action<T, IDictionary<string, IEnumerable<JsonReaderResult>>> parser) 
        {
            var json = Result.Value;
            var extractor = new JsonReaderExtractor<T>(propertyNames, parser);
            var names = extractor.PropertyNames.ToList();
            var partial = new List<JsonReaderResult>();

            foreach (var jr in json)
            {
                if (names.Exists(s => s.Equals(jr.Path, StringComparison.OrdinalIgnoreCase) || (HasMatchWithAsterisk(s, jr.Path))))
                {
                    partial.Add(jr);
                }

                if (partial.Count == names.Count)
                {
                    extractor.ManyParser(Instance, partial.ToDictionary(jk => jk.PropertyName, jv => jv.Children as IEnumerable<JsonReaderResult>));
                    partial.Clear();
                }
            }
            return this;
        }

        private Lazy<List<JsonReaderResult>> Result { get; }

        private List<JsonReaderResult> FillArray(JsonReaderResult parent)
        {
            var result = new List<JsonReaderResult>();
            while (Reader.Read())
            {
                var jr = new JsonReaderResult();
                jr.Parent = parent;
                if (Reader.TokenType == JsonToken.EndArray) { break; }
                if (Reader.TokenType == JsonToken.StartObject)
                {
                    jr.Children = FillObjectArray(jr);
                }
                else
                {
                    jr.Value = Reader.Value;
                }
                jr.Path = Reader.Path.RemoveBrackets();
                jr.Type = Reader.ValueType ?? typeof(Array);
                result.Add(jr);
            }
            return result;
        }

        private List<JsonReaderResult> FillObjectArray(JsonReaderResult parent)
        {
            var result = new List<JsonReaderResult>();
            while (Reader.Read())
            {
                var jr = new JsonReaderResult();
                jr.Parent = parent;
                switch (Reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        jr.PropertyName = Reader.Value.ToString();
                        Reader.Read();
                        break;
                }
                if (Reader.TokenType == JsonToken.EndObject) { break; }
                if (Reader.TokenType == JsonToken.StartArray)
                {
                    jr.Children = FillArray(jr);
                }
                else
                {
                    jr.Value = Reader.Value;
                }
                jr.Path = Reader.Path.RemoveBrackets();
                jr.Type = Reader.ValueType ?? typeof(Array);
                result.Add(jr);
            }
            return result;
        }

        private bool HasMatchWithAsterisk(string s, string path)
        {
            return s.EndsWith("*") && path.Contains('.') && s.Remove(s.LastIndexOf('.')).Equals(path.Remove(path.LastIndexOf('.')));
        }
    }

    internal static class RegexExtensions
    {
        internal static readonly Lazy<Regex> LazySquareBracketsRemover = new Lazy<Regex>(() => new Regex(@"\[[^]]*\]", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled));

        internal static string RemoveBrackets(this string path)
        {
            return LazySquareBracketsRemover.Value.Replace(path, "");
        }
    }
}