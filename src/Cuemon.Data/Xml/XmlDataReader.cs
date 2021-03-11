using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;
using Cuemon.Text;

namespace Cuemon.Data.Xml
{
    /// <summary>
    /// Provides a way of reading a forward-only stream of rows from an XML based data source. This class cannot be inherited.
    /// </summary>
    public sealed class XmlDataReader : DataReader<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value. Default is <see cref="ParserFactory.FromValueType"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public XmlDataReader(XmlReader reader, Func<string, Action<FormattingOptions<CultureInfo>>, object> parser = null) : base(parser ?? ParserFactory.FromValueType().Parse)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
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
        /// Gets the currently processed row count of this instance.
        /// </summary>
        /// <value>The currently processed row count of this instance.</value>
        /// <remarks>This property is incremented when the invoked <see cref="Read"/> method returns <c>true</c>.</remarks>
        public override int RowCount { get; protected set; }

        /// <summary>
        /// Gets the value that indicates that no more rows exists.
        /// </summary>
        /// <value>The value that indicates that no more rows exists.</value>
        protected override bool NullRead => false;

        /// <summary>
        /// Advances this instance to the next element of the XML data source.
        /// </summary>
        /// <returns><c>true</c> if there are more elements; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This instance has been disposed.
        /// </exception>
        public bool Read()
        {
            if (Disposed) { throw new ObjectDisposedException(GetType().FullName); }
            return ReadNext(default);
        }

        /// <summary>
        /// Advances this instance to the next element of the XML data source.
        /// </summary>
        /// <returns><c>true</c> if there are more elements; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This instance has been disposed.
        /// </exception>
        protected override bool ReadNext(bool columns)
        {
            if (Disposed) { throw new ObjectDisposedException(GetType().FullName); }
            var reader = Reader;
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
                        if (reader.HasAttributes && reader.MoveToFirstAttribute())
                        {
                            goto case XmlNodeType.Attribute;
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
            var hasRows = fields.Count > 0;
            if (hasRows) { RowCount++; }
            return hasRows;
        }

        private void Parse(XmlReader reader, ref OrderedDictionary values)
        {
            Parse(reader.LocalName, reader, ref values);
        }

        private void Parse(string localName, XmlReader reader, ref OrderedDictionary values)
        {
            if (values.Contains(localName))
            {
                values[localName] = StringParser(reader.Value, null);
            }
            else
            {
                values.Add(localName, StringParser(reader.Value, null));
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