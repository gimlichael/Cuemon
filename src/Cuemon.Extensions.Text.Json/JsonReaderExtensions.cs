using System.Collections.Generic;
using System.Text.Json;
using Cuemon.Text;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="Utf8JsonReader"/> struct.
    /// </summary>
    public static class JsonReaderExtensions
    {
        private const string PropertyNameKey = "reader.Value";

        /// <summary>
        /// Converts the JSON hierarchy of an <see cref="Utf8JsonReader"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> implementation that uses <see cref="DataPair"/>.</returns>
        public static IHierarchy<DataPair> ToHierarchy(this Utf8JsonReader reader)
        {
            var index = 0;
            var depthIndexes = new Dictionary<int, Dictionary<int, int>>();
            var dimension = 0;
            IHierarchy<DataPair> hierarchy = new Hierarchy<DataPair>();
            var array = new List<DataPair>();
            hierarchy.Add(new DataPair("root", null, typeof(string)));
            while (reader.Read())
            {
                object typeStrongValue;
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        if (reader.TokenType == JsonTokenType.EndArray) { goto case JsonTokenType.EndArray; }
                        if (reader.TokenType != JsonTokenType.StartArray && reader.TokenType != JsonTokenType.StartObject && reader.TokenType != JsonTokenType.EndObject)
                        {
                            typeStrongValue = ParserFactory.FromValueType().Parse(reader.GetString());
                            array.Add(new DataPair(hierarchy[index].Data[PropertyNameKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        }
                        while (reader.Read()) { goto case JsonTokenType.StartArray; }
                        break;
                    case JsonTokenType.PropertyName:
                        hierarchy[Decorator.Enclose(depthIndexes).GetDepthIndex(reader.CurrentDepth, index, dimension)].Add(new DataPair(reader.GetString(), null, typeof(string))).Data.Add(PropertyNameKey, reader.GetString());
                        index++;
                        break;
                    case JsonTokenType.EndArray:
                        var indexCopy = index;
                        foreach (var item in array)
                        {
                            hierarchy[indexCopy].Add(item);
                            index++;
                        }
                        array.Clear();
                        break;
                    case JsonTokenType.EndObject:
                        if (reader.CurrentDepth == 1) { dimension++; }
                        break;
                    case JsonTokenType.Number:
                    case JsonTokenType.String:
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                    case JsonTokenType.Null:
                        typeStrongValue = ParserFactory.FromValueType().Parse(reader.GetString());
                        hierarchy[index].Replace(new DataPair(hierarchy[index].Data[PropertyNameKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        hierarchy[index].Data.Remove(PropertyNameKey);
                        break;
                }
            }
            return hierarchy;
        }
    }
}
