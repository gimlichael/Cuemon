using System.Collections.Generic;
using System.Xml;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlReader"/>.
    /// </summary>
    public static class XmlReaderExtensions
    {
        private const string XmlReaderKey = "reader.Name";

        /// <summary>
        /// Converts the XML hierarchy of an <see cref="XmlReader"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> implementation that uses <see cref="DataPair"/>.</returns>
        public static IHierarchy<DataPair> ToHierarchy(this XmlReader reader)
        {
            var index = 0;
            var depthIndexes = new Dictionary<int, Dictionary<int, int>>();
            var dimension = 0;
            IHierarchy<DataPair> hierarchy = new Hierarchy<DataPair>();
            List<DataPair> attributes = null;
            while (reader.Read())
            {
                object typeStrongValue;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        typeStrongValue = ObjectConverter.FromString(reader.Value);
                        attributes.Add(new DataPair(reader.Name, typeStrongValue, typeStrongValue.GetType()));
                        while (reader.MoveToNextAttribute()) { goto case XmlNodeType.Attribute; }
                        foreach (var attribute in attributes)
                        {
                            hierarchy[index].Add(attribute);
                            index++;
                        }
                        reader.MoveToElement();
                        break;
                    case XmlNodeType.Element:
                        attributes = new List<DataPair>();
                        if (reader.Depth == 0)
                        {
                            hierarchy.Add(new DataPair(reader.Name, null, typeof(string))).Data.Add(XmlReaderKey, reader.Name);
                            continue;
                        }

                        hierarchy[depthIndexes.GetDepthIndex(reader, index, dimension)].Add(new DataPair(reader.Name, null, typeof(string))).Data.Add(XmlReaderKey, reader.Name);
                        index++;

                        if (reader.HasAttributes) { if (reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; } }
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Depth == 1) { dimension++; }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        typeStrongValue = ObjectConverter.FromString(reader.Value);
                        hierarchy[index].Replace(new DataPair(hierarchy[index].Data[XmlReaderKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        hierarchy[index].Data.Remove(XmlReaderKey);
                        break;
                }
            }
            return hierarchy;
        }

        private static int GetDepthIndex(this IDictionary<int, Dictionary<int, int>> depthIndexes, XmlReader reader, int index, int dimension)
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