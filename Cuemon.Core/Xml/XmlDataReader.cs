using System;
using System.Collections.Specialized;
using System.Xml;
using Cuemon.Data;

namespace Cuemon.Xml
{
    /// <summary>
    /// Provides a way of reading a forward-only stream of rows from an XML based data source. This class cannot be inherited.
    /// </summary>
    public sealed class XmlDataReader : StringDataReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataReader" /> class.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public XmlDataReader(XmlReader reader) : this(reader, ObjectConverter.FromString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null -or- <paramref name="parser"/> is null.
        /// </exception>
        /// <remarks>The default implementation uses <see cref="ObjectConverter.FromString(string)"/> as <paramref name="parser"/>.</remarks>
        public XmlDataReader(XmlReader reader, Func<string, object> parser) : base(parser)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfNull(parser, nameof(parser));
            Reader = reader;
        }

        private XmlReader Reader { get; set; }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current element.
        /// </summary>
        /// <value>The level of nesting.</value>
        public override int Depth => CurrentDepth;

        private int CurrentDepth { get; set; }

        /// <summary>
        /// Advances this instance to the next element of the XML data source.
        /// </summary>
        /// <returns><c>true</c> if there are more elements; otherwise, <c>false</c>.</returns>
        protected override bool ReadNext()
        {
            if (Disposed) { throw new ObjectDisposedException(GetType().FullName); }
            return ReadNextCore(Reader);
        }

        private bool ReadNextCore(XmlReader reader)
        {
            var fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
            string elementName = null;
            while (reader.Read())
            {
                CurrentDepth = reader.Depth;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        Parse(reader, ref fields);
                        while (reader.MoveToNextAttribute())
                        {
                            Parse(reader, ref fields);
                        }
                        if (reader.MoveToElement()) { goto addFields; }
                        break;
                    case XmlNodeType.Element:
                        elementName = reader.LocalName;
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToFirstAttribute())
                            {
                                goto case XmlNodeType.Attribute;
                            }
                        }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        if (elementName != null) { Parse(elementName, reader, ref fields); }
                        elementName = null;
                        break;
                    case XmlNodeType.EndElement:
                        break;
                    default:
                        if (fields.Count > 0) { goto addFields; }
                        break;
                }
            }
        addFields:
            SetFields(fields);
            return (fields.Count > 0);
        }

        private void Parse(XmlReader reader, ref OrderedDictionary values)
        {
            Parse(reader.LocalName, reader, ref values);
        }

        private void Parse(string localName, XmlReader reader, ref OrderedDictionary values)
        {
            if (values.Contains(localName))
            {
                values[localName] = StringParser(reader.Value);
            }
            else
            {
                values.Add(localName, StringParser(reader.Value));
            }
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(bool)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            Reader?.Dispose();
        }
    }
}