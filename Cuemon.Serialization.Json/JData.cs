using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Cuemon.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Provides a way to parse and extract values from various sources of JSON data.
    /// </summary>
    public class JData
    {
        public static IEnumerable<JDataResult> ReadAll(Stream json, bool leaveStreamOpen = false, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(json, nameof(json));
            var options = setup.Configure();
            using (var sr = new StreamReader(json, options.Encoding, false, 1024, leaveStreamOpen))
            {
                
                using (var jr = new JsonTextReader(sr))
                {
                    jr.CloseInput = !leaveStreamOpen;
                    return ReadAll(jr);
                }
            }
        }
        
        public static IEnumerable<JDataResult> ReadAll(string json)
        {
            Validator.ThrowIfNullOrWhitespace(json, nameof(json));
            using (var sr = new StringReader(json))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    return ReadAll(jr);
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="JData"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to apply a JSON document.</typeparam>
        /// <param name="instance">An initialized instance of <typeparamref name="T"/>.</param>
        /// <param name="reader">The <see cref="JsonReader"/> to apply to <paramref name="instance"/>.</param>
        /// <returns>A new instance of <see cref="JData"/>.</returns>
        public static IEnumerable<JDataResult> ReadAll(JsonReader reader)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.Throw.IfNotValidJsonDocument(ref reader, nameof(reader));
            return new JData(reader).Result.Value;
        }

        internal JData(JsonReader reader)
        {
            Result = new Lazy<List<JDataResult>>(() =>
            {
                var result = new List<JDataResult>();
                while (reader.Read())
                {
                    if (reader.Value == null) { continue; }
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
                        jr.Children = FillArray(reader, jr);
                    }
                    else
                    {
                        jr.Value = reader.Value;
                    }
                    jr.Path = reader.Path.RemoveBrackets();
                    jr.Type = reader.ValueType ?? typeof(Array);
                    result.Add(jr);
                }
                return result;
            });
        }

        private Lazy<List<JDataResult>> Result { get; }

        private List<JDataResult> FillArray(JsonReader reader, JDataResult parent)
        {
            var result = new List<JDataResult>();
            while (reader.Read())
            {
                var jr = new JDataResult();
                jr.Parent = parent;
                if (reader.TokenType == JsonToken.EndArray) { break; }
                if (reader.TokenType == JsonToken.StartObject)
                {
                    jr.Children = FillObjectArray(reader, jr);
                }
                else
                {
                    jr.Value = reader.Value;
                }
                jr.Path = reader.Path.RemoveBrackets();
                jr.Type = reader.ValueType ?? typeof(Array);
                result.Add(jr);
            }
            return result;
        }

        private List<JDataResult> FillObjectArray(JsonReader reader, JDataResult parent)
        {
            var result = new List<JDataResult>();
            while (reader.Read())
            {
                var jr = new JDataResult { Parent = parent };
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        jr.PropertyName = reader.Value.ToString();
                        reader.Read();
                        break;
                }
                if (reader.TokenType == JsonToken.EndObject) { break; }
                if (reader.TokenType == JsonToken.StartArray)
                {
                    jr.Children = FillArray(reader, jr);
                }
                else
                {
                    jr.Value = reader.Value;
                }
                jr.Path = reader.Path.RemoveBrackets();
                jr.Type = reader.ValueType ?? typeof(Array);
                result.Add(jr);
            }
            return result;
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