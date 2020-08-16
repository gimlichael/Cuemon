using System;
using System.Collections.Generic;
using System.Xml;
using Cuemon.Text;

namespace Cuemon.Xml
{
 /// <summary>
    /// Extension methods for the <see cref="XmlReader"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class XmlReaderDecoratorExtensions
    {
        private const string XmlReaderKey = "reader.Name";

        /// <summary>
        /// Converts the XML hierarchy of the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{XmlReader}" /> to extend.</param>
        /// <returns>An <see cref="T:IHierarchy{DataPair}"/> implementation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IHierarchy<DataPair> ToHierarchy(this IDecorator<XmlReader> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var index = 0;
            var depthIndexes = new Dictionary<int, Dictionary<int, int>>();
            var dimension = 0;
            var reader = decorator.Inner;
            IHierarchy<DataPair> hierarchy = new Hierarchy<DataPair>();
            List<DataPair> attributes = null;
            while (reader.Read())
            {
                object typeStrongValue;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        typeStrongValue = ParserFactory.FromValueType().Parse(reader.Value);
                        if (attributes != null)
                        {
                            attributes.Add(new DataPair(reader.Name, typeStrongValue, typeStrongValue.GetType()));
                            while (reader.MoveToNextAttribute())
                            {
                                goto case XmlNodeType.Attribute;
                            }

                            var elementIndex = index;
                            foreach (var attribute in attributes)
                            {
                                hierarchy[index].Add(attribute).Data.Add("parent", elementIndex);
                                index++;
                            }
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

                        if (reader.HasAttributes && reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; }
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Depth == 1) { dimension++; }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        var indexToApplyText = hierarchy[index].Data.ContainsKey(XmlReaderKey) ? index : Decorator.Enclose(hierarchy[index].Data["parent"]).ChangeTypeOrDefault<int>();
                        typeStrongValue = ParserFactory.FromValueType().Parse(reader.Value);
                        hierarchy[indexToApplyText].Replace(new DataPair(hierarchy[indexToApplyText].Data[XmlReaderKey]?.ToString(), typeStrongValue, typeStrongValue.GetType()));
                        hierarchy[indexToApplyText].Data.Remove(XmlReaderKey);
                        break;
                }
            }
            return hierarchy;
        }

        private static int GetDepthIndex(this IDictionary<int, Dictionary<int, int>> depthIndexes, XmlReader reader, int index, int dimension)
        {
            if (depthIndexes.TryGetValue(dimension, out var row))
            {
                if (!row.TryGetValue(reader.Depth, out _))
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
