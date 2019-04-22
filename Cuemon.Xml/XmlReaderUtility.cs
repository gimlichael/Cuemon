﻿using System;
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
            var settings = new XmlReaderSettings();
            OverrideDefaultSettings(settings);
            setup?.Invoke(settings);
            return settings;
        }

        private static void OverrideDefaultSettings(XmlReaderSettings settings)
        {
            settings.DtdProcessing = DtdProcessing.Ignore;
        }

        /// <summary>
        /// Creates and returns a chunked sequence of <see cref="XmlReader"/> objects with a maximum of 128 XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data to chunk into smaller <see cref="XmlReader"/> objects for a batch run or similar.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> objects that contains no more than 128 XML node elements from the <paramref name="reader" /> object.</returns>
        public static IEnumerable<XmlReader> Chunk(XmlReader reader)
        {
            return Chunk(reader, 128);
        }

        /// <summary>
        /// Creates and returns a chunked sequence of <see cref="XmlReader"/> objects with a maximum of the specified <paramref name="size"/> of XML node elements located on a depth of 1.
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
        public static IEnumerable<XmlReader> Chunk(XmlReader reader, int size, Action<XmlWriterSettings> setup = null)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (reader.ReadState != ReadState.Initial) { throw new ArgumentException("The Read method of the XmlReader object has already been called.", nameof(reader)); }
            List<XmlReader> outerReaders = new List<XmlReader>();
            XmlReaderSettings readerSettings = reader.Settings;
            if (MoveToFirstElement(reader))
            {
                XmlQualifiedEntity rootElement = new XmlQualifiedEntity(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                List<XmlReader> innerReaders = new List<XmlReader>();
                Stream result;
                while (reader.Read())
                {
                    if (reader.Depth > 1) { continue; }
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            XPathDocument document = new XPathDocument(reader.ReadSubtree());
                            XPathNavigator navigator = document.CreateNavigator();
                            innerReaders.Add(navigator.ReadSubtree());
                            break;
                    }

                    if (innerReaders.Count != size) { continue; }

                    result = XmlWriterUtility.CreateXml(ChunkCore, innerReaders, rootElement, setup);
                    outerReaders.Add(XmlReader.Create(result, readerSettings));
                    innerReaders.Clear();
                }

                if (innerReaders.Count > 0)
                {
                    result = XmlWriterUtility.CreateXml(ChunkCore, innerReaders, rootElement, setup);
                    outerReaders.Add(XmlReader.Create(result, readerSettings));
                    innerReaders.Clear();
                }
            }
            return outerReaders;
        }

        private static void ChunkCore(XmlWriter writer, IEnumerable<XmlReader> readers, XmlQualifiedEntity rootElement)
        {
            if (readers == null) { throw new ArgumentNullException(nameof(readers)); }
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            writer.WriteStartElement(rootElement.Prefix, rootElement.LocalName, rootElement.Namespace);
            foreach (XmlReader reader in readers)
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
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
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