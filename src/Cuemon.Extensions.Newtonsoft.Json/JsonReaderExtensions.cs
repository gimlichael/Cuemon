﻿using System.Collections.Generic;
using Cuemon.Text;
using Cuemon.Collections.Generic;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonReader"/> class.
    /// </summary>
    public static class JsonReaderExtensions
    {
        private const string PropertyNameKey = "reader.Value";

        /// <summary>
        /// Converts the JSON hierarchy of an <see cref="JsonReader"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> implementation that uses <see cref="DataPair"/>.</returns>
        public static IHierarchy<DataPair> ToHierarchy(this JsonReader reader)
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
                    case JsonToken.StartArray:
                        if (reader.TokenType == JsonToken.EndArray) { goto case JsonToken.EndArray; }
                        if (reader.TokenType != JsonToken.StartArray && reader.TokenType != JsonToken.StartObject && reader.TokenType != JsonToken.EndObject)
                        {
                            typeStrongValue = ParserFactory.FromValueType().Parse(reader.Value.ToString());
                            array.Add(new DataPair(hierarchy[index].Data[PropertyNameKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        }
                        while (reader.Read()) { goto case JsonToken.StartArray; }
                        break;
                    case JsonToken.PropertyName:
                        hierarchy[Decorator.Enclose(depthIndexes).GetDepthIndex(reader.Depth, index, dimension)].Add(new DataPair(reader.Value.ToString(), null, typeof(string))).Data.Add(PropertyNameKey, reader.Value.ToString());
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
                        typeStrongValue = ParserFactory.FromValueType().Parse(reader.Value.ToString());
                        hierarchy[index].Replace(new DataPair(hierarchy[index].Data[PropertyNameKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        hierarchy[index].Data.Remove(PropertyNameKey);
                        break;
                }
            }
            return hierarchy;
        }
    }
}