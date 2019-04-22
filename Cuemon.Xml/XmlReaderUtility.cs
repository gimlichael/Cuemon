using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make <see cref="XmlReader"/> related operations easier to work with.
    /// </summary>
    public static class XmlReaderUtility
    {
        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlReader"/> object.
        /// </summary>
        /// <param name="setup">The <see cref="XmlReaderSettings"/> which may be configured.</param>
        /// <returns>A <see cref="XmlReaderSettings"/> instance that specifies a set of features to support the <see cref="XmlReader"/> object.</returns>
        /// <remarks>
        /// The following table shows the overridden initial property values for an instance of <see cref="XmlReaderSettings"/>.<br/>
        /// The initial property values can be viewed here: https://msdn.microsoft.com/EN-US/library/2ycwc04f(v=VS.110,d=hv.2).aspx
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="XmlReaderSettings.DtdProcessing"/></term>
        ///         <description><see cref="DtdProcessing.Ignore"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static XmlReaderSettings CreateSettings(Action<XmlReaderSettings> setup = null)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
            setup?.Invoke(settings);
            return settings;
        }

        /// <summary>
        /// Creates and returns a sequence of chunked <see cref="XmlReader"/> objects with a maximum of the specified <paramref name="size"/> of XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data to chunk into smaller <see cref="XmlReader"/> objects for a batch run or similar.</param>
        /// <param name="size">The amount of XML node elements allowed per <see cref="XmlReader"/> object. Default is 128 XML node element.</param>
        /// <param name="setup">The <see cref="XmlWriterSettings"/> which need to be configured.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> objects that contains no more than the specified <paramref name="size"/> of XML node elements from the <paramref name="reader" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the <paramref name="reader"/> object has already been called.
        /// </exception>
        public static IEnumerable<XmlReader> Chunk(XmlReader reader, int size = 128, Action<XmlWriterSettings> setup = null)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfTrue(reader.ReadState != ReadState.Initial, nameof(reader), "The Read method of the XmlReader object has already been called.");
            var outerReaders = new List<XmlReader>();
            var readerSettings = reader.Settings;
            if (MoveToFirstElement(reader))
            {
                var rootElement = new XmlQualifiedEntity(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                var innerReaders = new List<XmlReader>();
                Stream result;
                while (reader.Read())
                {
                    if (reader.Depth > 1) { continue; }
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            var document = new XPathDocument(reader.ReadSubtree());
                            var navigator = document.CreateNavigator();
                            innerReaders.Add(navigator.ReadSubtree());
                            break;
                    }

                    if (innerReaders.Count != size) { continue; }

                    result = XmlWriterUtility.CreateStream(writer => ChunkCore(writer, innerReaders, rootElement), setup);
                    outerReaders.Add(XmlReader.Create(result, readerSettings));
                    innerReaders.Clear();
                }

                if (innerReaders.Count > 0)
                {
                    result = XmlWriterUtility.CreateStream(writer => ChunkCore(writer, innerReaders, rootElement), setup);
                    outerReaders.Add(XmlReader.Create(result, readerSettings));
                    innerReaders.Clear();
                }
            }
            return outerReaders;
        }

        private static void ChunkCore(XmlWriter writer, IEnumerable<XmlReader> readers, XmlQualifiedEntity rootElement)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            Validator.ThrowIfNull(readers, nameof(readers));
            writer.WriteStartElement(rootElement.Prefix, rootElement.LocalName, rootElement.Namespace);
            foreach (var reader in readers)
            {
                try
                {
                    writer.WriteNode(reader, true);
                }
                finally
                {
                    reader.Dispose();
                }
            }
            writer.WriteEndDocument();
        }

        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream CopyToStream(XmlReader reader, Action<XmlCopyOptions> setup = null)
        {
            return CopyToStream(reader, (w, r, o) =>
            {
                try
                {
                    while (r.Read())
                    {
                        switch (r.NodeType)
                        {
                            case XmlNodeType.CDATA:
                                w.WriteCData(r.Value);
                                break;
                            case XmlNodeType.Comment:
                                w.WriteComment(r.Value);
                                break;
                            case XmlNodeType.DocumentType:
                                w.WriteDocType(r.Name, r.GetAttribute("PUBLIC"), r.GetAttribute("SYSTEM"), r.Value);
                                break;
                            case XmlNodeType.Element:
                                w.WriteStartElement(r.Prefix, r.LocalName, r.NamespaceURI);
                                w.WriteAttributes(r, true);
                                if (r.IsEmptyElement) { w.WriteEndElement(); }
                                break;
                            case XmlNodeType.EndElement:
                                w.WriteFullEndElement();
                                break;
                            case XmlNodeType.Attribute:
                            case XmlNodeType.Document:
                            case XmlNodeType.DocumentFragment:
                            case XmlNodeType.EndEntity:
                            case XmlNodeType.None:
                            case XmlNodeType.Notation:
                            case XmlNodeType.Entity:
                                break;
                            case XmlNodeType.EntityReference:
                                w.WriteEntityRef(r.Name);
                                break;
                            case XmlNodeType.Whitespace:
                            case XmlNodeType.SignificantWhitespace:
                                w.WriteWhitespace(r.Value);
                                break;
                            case XmlNodeType.Text:
                                w.WriteString(r.Value);
                                break;
                            case XmlNodeType.ProcessingInstruction:
                            case XmlNodeType.XmlDeclaration:
                                w.WriteProcessingInstruction(r.Name, r.Value);
                                break;
                        }
                    }
                }
                finally
                {
                    if (!o.LeaveOpen) { r?.Dispose(); }
                }

            }, setup);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings"/>.</remarks>
        public static Stream CopyToStream(XmlReader reader, Action<XmlWriter, XmlReader, DisposableOptions> copier, Action<XmlCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfNull(copier, nameof(copier));
            var options = Patterns.Configure(setup);
            return XmlWriterUtility.CreateStream(writer => copier(writer, reader, options), options.WriterSettings);
        }

        /// <summary>
        /// Moves the <paramref name="reader"/> object to the first element.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <returns><c>true</c> if an element exists (the reader moves to the first element), otherwise, <c>false</c> (the reader has reached <see cref="XmlReader.EOF"/>).</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the <paramref name="reader"/> object has already been called.
        /// </exception>
        public static bool MoveToFirstElement(XmlReader reader)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            if (reader.ReadState != ReadState.Initial) { throw new ArgumentException("The Read method of the XmlReader object has already been called.", nameof(reader)); }
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        return true;
                }
            }
            return false;
        }
    }
}