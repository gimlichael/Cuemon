using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Cuemon.IO;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Provides a factory based way to parse and extract values from various sources of JSON data. Compliant with RFC 7159 as it uses <see cref="JsonTextReader"/> behind the scene.
    /// </summary>
    public class JData
    {
        /// <summary>
        /// Creates a sequence of <see cref="T:IEnumerable{JDataResult}"/> from the specified <paramref name="json"/>.
        /// </summary>
        /// <param name="json">A <see cref="Stream"/> that represents a JSON data structure.</param>
        /// <param name="setup">The <see cref="StreamReaderOptions" /> which may be configured.</param>
        /// <returns>An <see cref="T:IEnumerable{JDataResult}"/> sequence from the specified <see cref="Stream"/>.</returns>
        public static IEnumerable<JDataResult> ReadAll(Stream json, Action<StreamReaderOptions> setup = null)
        {
            Validator.ThrowIfNull(json);
            var options = Patterns.Configure(setup);
            using (var sr = new StreamReader(json, options.Encoding, false, options.BufferSize, options.LeaveOpen))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    jr.CloseInput = !options.LeaveOpen;
                    return ReadAll(jr);
                }
            }
        }

        /// <summary>
        /// Creates a sequence of <see cref="T:IEnumerable{JDataResult}"/> from the specified <paramref name="json"/>.
        /// </summary>
        /// <param name="json">A <see cref="string"/> that represents a JSON data structure.</param>
        /// <returns>An <see cref="T:IEnumerable{JDataResult}"/> sequence from the specified <see cref="string"/>.</returns>
        public static IEnumerable<JDataResult> ReadAll(string json)
        {
            Validator.ThrowIfNullOrWhitespace(json);
            using (var sr = new StringReader(json))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    return ReadAll(jr);
                }
            }
        }

        /// <summary>
        /// Creates a sequence of <see cref="T:IEnumerable{JDataResult}"/> from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to parse and extract an <see cref="T:IEnumerable{JDataResult}"/> sequence from.</param>
        /// <returns>An <see cref="T:IEnumerable{JDataResult}"/> sequence from the specified <see cref="JsonReader"/>.</returns>
        public static IEnumerable<JDataResult> ReadAll(JsonReader reader)
        {
            Validator.ThrowIfNull(reader);
            Validator.ThrowIf.InvalidJsonDocument(ref reader);
            return new JData(reader).Result.Value;
        }

        internal JData(JsonReader reader)
        {
            Result = new Lazy<List<JDataResult>>(() =>
            {
                var result = new List<JDataResult>();
                while (reader.Read())
                {
                    var jr = new JDataResult();
                    switch (reader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            jr.PropertyName = reader.Value.ToString();
                            reader.Read();
                            break;
                    }

                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        return FillArrayHierarchy(reader, jr);
                    }
                    
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        return FillObjectHierarchy(reader, jr);
                    }

                    jr.Value = reader.Value;
                    jr.Path = reader.Path.RemoveBrackets();
                    jr.Type = reader.ValueType;
                    result.Add(jr);
                }
                return result;
            });
        }

        private Lazy<List<JDataResult>> Result { get; }


        private List<JDataResult> FillArrayHierarchy(JsonReader reader, JDataResult parent)
        {
            return FillHierarchy(reader, parent, r => r.TokenType == JsonToken.EndArray);
        }

        private List<JDataResult> FillObjectHierarchy(JsonReader reader, JDataResult parent)
        {
            return FillHierarchy(reader, parent, r => r.TokenType == JsonToken.EndObject);
        }

        private List<JDataResult> FillHierarchy(JsonReader reader, JDataResult parent, Func<JsonReader, bool> skipWhenTrue)
        {
            var result = new List<JDataResult>();
            while (reader.Read())
            {
                if (skipWhenTrue(reader)) { break; }
                var jr = new JDataResult { Parent = parent };
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        jr.PropertyName = reader.Value.ToString();
                        reader.Read();
                        break;
                }
                if (reader.TokenType == JsonToken.StartArray)
                {
                    jr.Children = FillArrayHierarchy(reader, jr);
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    jr.Children = FillObjectHierarchy(reader, jr);
                }
                else
                {
                    jr.Value = reader.Value;
                }
                jr.Path = reader.Path.RemoveBrackets();
                jr.Type = reader.ValueType;
                result.Add(jr);
            }
            return result;
        }
    }

    internal static class RegexExtensions
    {
        internal static readonly Lazy<Regex> LazySquareBracketsRemover = new(() => new Regex(@"\[[^]]*\]", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled));

        internal static string RemoveBrackets(this string path)
        {
            return LazySquareBracketsRemover.Value.Replace(path, "");
        }
    }
}
