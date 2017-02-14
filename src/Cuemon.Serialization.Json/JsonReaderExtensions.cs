using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonReader"/>.
    /// </summary>
    public static class JsonReaderExtensions
    {
        private const string PropertyNameKey = "reader.Value";

        /// <summary>
        /// Converts the XML hierarchy of an <see cref="JsonReader"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> implementation that uses <see cref="DataPair"/>.</returns>
        public static IHierarchy<DataPair> ToHierarchy(this JsonReader reader)
        {
            var index = 0;
            var depthIndexes = new Dictionary<int, Dictionary<int, int>>();
            var dimension = 0;
            IHierarchy<DataPair> hierarchy = new Hierarchy<DataPair>();
            List<DataPair> array = new List<DataPair>();
            hierarchy.Add(new DataPair("root", null, typeof(string)));
            while (reader.Read())
            {
                object typeStrongValue;
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        if (reader.TokenType == JsonToken.EndArray) { goto case JsonToken.EndArray; }
                        if (reader.TokenType != JsonToken.StartArray && reader.TokenType != JsonToken.StartObject && reader.TokenType != JsonToken.EndObject)
                        {
                            typeStrongValue = ObjectConverter.FromString("{0}".FormatWith(reader.Value));
                            array.Add(new DataPair(hierarchy[index].Data[PropertyNameKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        }
                        while (reader.Read()) { goto case JsonToken.StartArray; }
                        break;
                    case JsonToken.PropertyName:
                        hierarchy[depthIndexes.GetDepthIndex(reader, index, dimension)].Add(new DataPair("{0}".FormatWith(reader.Value), null, typeof(string))).Data.Add(PropertyNameKey, "{0}".FormatWith(reader.Value));
                        index++;
                        break;
                    case JsonToken.EndArray:
                        var indexCopy = index;
                        foreach (var item in array)
                        {
                            hierarchy[indexCopy].Add(item);
                            index++;
                        }
                        array.Clear();
                        break;
                    case JsonToken.EndObject:
                        if (reader.Depth == 1) { dimension++; }
                        break;
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Integer:
                    case JsonToken.Null:
                    case JsonToken.String:
                        typeStrongValue = ObjectConverter.FromString("{0}".FormatWith(reader.Value));
                        hierarchy[index].Replace(new DataPair(hierarchy[index].Data[PropertyNameKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        hierarchy[index].Data.Remove(PropertyNameKey);
                        break;
                }
            }
            return hierarchy;
        }

        private static int GetDepthIndex(this IDictionary<int, Dictionary<int, int>> depthIndexes, JsonReader reader, int index, int dimension)
        {
            Dictionary<int, int> row;
            if (depthIndexes.TryGetValue(dimension, out row))
            {
                int localIndex;
                if (!row.TryGetValue(reader.Depth, out localIndex))
                {
                    row.Add(reader.Depth, index);
                }
            }
            else
            {
                depthIndexes.Add(dimension, new Dictionary<int, int>());
                if (dimension == 0)
                {
                    depthIndexes[dimension].Add(reader.Depth, index);
                }
                else
                {
                    depthIndexes[dimension].Add(reader.Depth, depthIndexes[dimension - 1][reader.Depth]);
                }
            }
            return depthIndexes[dimension][reader.Depth];
        }
    }
}