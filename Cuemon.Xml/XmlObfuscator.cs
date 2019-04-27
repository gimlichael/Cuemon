using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Security;
using Cuemon.Xml.XPath;

namespace Cuemon.Xml
{
    /// <summary>
    /// Provides methods for obfuscation and size reduction of human readable XML documents.
    /// </summary>
    public class XmlObfuscator : Obfuscator
    {
        internal const string MappingRootElement = "O";
        internal const string MappingValueElement = "V";
        internal const string MappingMapsToAttribute = "mT";

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlObfuscator"/> class using <see cref="Encoding.UTF8"/> for the text encoding.
        /// </summary>
        public XmlObfuscator() : this(Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlObfuscator"/> class.
        /// </summary>
        /// <param name="exclusions">A sequence of <see cref="T:System.String"/> values used for excluding matching original values in the obfuscation process.</param>
        public XmlObfuscator(IEnumerable<string> exclusions) : this(Encoding.UTF8, exclusions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlObfuscator"/> class using <see cref="Encoding.UTF8"/> for the text encoding.
        /// </summary>
        /// <param name="encoding">The text encoding to use.</param>
        public XmlObfuscator(Encoding encoding) : this(encoding, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlObfuscator"/> class.
        /// </summary>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="exclusions">A sequence of <see cref="T:System.String"/> values used for excluding matching original values in the obfuscation process.</param>
        public XmlObfuscator(Encoding encoding, IEnumerable<string> exclusions) : base(encoding, exclusions)
        {
        }
        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the permutation characters used in the obfuscation process using the default implmentation, but with the numeric characters removed.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IList`1"/> compatible object holding the permuation characters used in the obfuscation process, but with the numeric characters removed.</returns>
        protected override IList<char> InitializePermutationCharacters()
        {
            byte charatersToRemove = 10;
            var baseCharacters = new List<char>(base.InitializePermutationCharacters());
            baseCharacters.RemoveRange(baseCharacters.Count - charatersToRemove, charatersToRemove);
            return baseCharacters.AsReadOnly();
        }

        /// <summary>
        /// Creates and returns a mappaple XML document of the original values and the obfuscated values.
        /// </summary>
        /// <returns>A mappaple XML document of the original values and the obfuscated values.</returns>
        public override Stream CreateMapping()
        {
            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream();
                using (var writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(o => o.Encoding = Encoding)))
                {
                    writer.WriteStartDocument();
                    writer.WriteComment(string.Format(CultureInfo.InvariantCulture, " Legend: {0}=Obfuscated, {1}=Value, {2}=mapsTo ", MappingRootElement, MappingValueElement, MappingMapsToAttribute));
                    writer.WriteStartElement(MappingRootElement);
                    foreach (var obfuscatedMappingPair in Mappings)
                    {
                        writer.WriteStartElement(MappingValueElement);
                        writer.WriteAttributeString(MappingMapsToAttribute, obfuscatedMappingPair.Value.Obfuscated);
                        writer.WriteString(obfuscatedMappingPair.Value.Original);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally 
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            return output;
        }


        /// <summary>
        /// Obfuscates the XML document of the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to obfuscate.</param>
        /// <returns>A <see cref="Stream"/> object where the XML document has been obfuscated.</returns>
        public override Stream Obfuscate(Stream value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var startingPosition = value.Position;
            if (value.CanSeek) { value.Position = 0; }

            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream();
                using (var writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(o => o.Encoding = Encoding)))
                {
                    using (var reader = XmlReader.Create(value))
                    {
                        while (reader.Read())
                        {
                            var writeEndElement = false;
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Attribute:
                                    WriteAttributeString(writer, reader);
                                    while (reader.MoveToNextAttribute())
                                    {
                                        WriteAttributeString(writer, reader);
                                    }
                                    reader.MoveToElement();
                                    break;
                                case XmlNodeType.Element:
                                    WriteStartElement(writer, reader);
                                    writeEndElement = reader.IsEmptyElement;
                                    if (reader.HasAttributes)
                                    {
                                        if (reader.MoveToFirstAttribute())
                                        {
                                            goto case XmlNodeType.Attribute;
                                        }
                                    }
                                    break;
                                case XmlNodeType.EndElement:
                                    writer.WriteFullEndElement();
                                    break;
                                case XmlNodeType.Comment:
                                    writer.WriteComment(reader.Value);
                                    break;
                                case XmlNodeType.DocumentType:
                                    writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
                                    break;
                                case XmlNodeType.XmlDeclaration:
                                case XmlNodeType.ProcessingInstruction:
                                    writer.WriteProcessingInstruction(reader.Name, reader.Value);
                                    break;
                                case XmlNodeType.Text:
                                    WriteString(writer, reader);
                                    break;
                                case XmlNodeType.Whitespace:
                                case XmlNodeType.SignificantWhitespace:
                                    writer.WriteWhitespace(reader.Value);
                                    break;
                                case XmlNodeType.CDATA:
                                    WriteCData(writer, reader);
                                    break;
                                case XmlNodeType.EntityReference:
                                    writer.WriteEntityRef(reader.Name);
                                    break;
                            }
                            if (writeEndElement) { writer.WriteEndElement(); }
                        }
                    }
                    writer.Flush();
                }
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally 
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            
            if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); } // reset to original position
            return output;
        }

        /// <summary>
        /// Revert the obfuscated XML document of <paramref name="value"/> to its original state by applying the mappaple XML document of <paramref name="mapping"/>.
        /// </summary>
        /// <param name="value">The obfuscated <see cref="Stream"/> to revert.</param>
        /// <param name="mapping">A <see cref="Stream"/> containing mappaple values necessary to revert <paramref name="value"/> to its original state.</param>
        /// <returns>A <see cref="Stream"/> object where the obfuscated XML document has been reverted to its original XML document.</returns>
        public override Stream Revert(Stream value, Stream mapping)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            long obfustatedStartingPosition = -1;
            long mappingStartingPosition = -1;

            if (value.CanSeek)
            {
                obfustatedStartingPosition = value.Position;
                value.Position = 0;
            }
            if (mapping.CanSeek)
            {
                mappingStartingPosition = mapping.Position;
                mapping.Position = 0;
            }

            var document = XPathNavigableConverter.FromStream(mapping);
            var navigator = document.CreateNavigator();
            var mappingXpath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}[@{2}='{3}']", MappingRootElement, MappingValueElement, MappingMapsToAttribute, "{0}");

            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream();
                using (var writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(o => o.Encoding = Encoding)))
                {
                    using (var reader = XmlReader.Create(value))
                    {
                        while (reader.Read())
                        {
                            var writeEndElement = false;
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Attribute:
                                    writer.WriteAttributeString(reader.Prefix, Exclusions.Contains(reader.LocalName) ? reader.LocalName : navigator.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, mappingXpath, reader.LocalName)).Value, reader.NamespaceURI, Exclusions.Contains(reader.Value) ? reader.Value : navigator.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "{0}/{1}[@{2}='{3}']", MappingRootElement, MappingValueElement, MappingMapsToAttribute, reader.Value)).Value);
                                    while (reader.MoveToNextAttribute()) { goto case XmlNodeType.Attribute; }
                                    reader.MoveToElement();
                                    break;
                                case XmlNodeType.Element:
                                    writer.WriteStartElement(reader.Prefix, Exclusions.Contains(reader.LocalName) ? reader.LocalName : navigator.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, mappingXpath, reader.LocalName)).Value, reader.NamespaceURI);
                                    writeEndElement = reader.IsEmptyElement;
                                    if (reader.HasAttributes)
                                    {
                                        if (reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; }
                                    }
                                    break;
                                case XmlNodeType.EndElement:
                                    writer.WriteFullEndElement();
                                    break;
                                case XmlNodeType.Comment:
                                    writer.WriteComment(reader.Value);
                                    break;
                                case XmlNodeType.DocumentType:
                                    writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
                                    break;
                                case XmlNodeType.XmlDeclaration:
                                case XmlNodeType.ProcessingInstruction:
                                    writer.WriteProcessingInstruction(reader.Name, reader.Value);
                                    break;
                                case XmlNodeType.Text:
                                    writer.WriteString(Exclusions.Contains(reader.Value) ? reader.Value : navigator.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, mappingXpath, reader.Value)).Value);
                                    break;
                                case XmlNodeType.Whitespace:
                                case XmlNodeType.SignificantWhitespace:
                                    writer.WriteWhitespace(reader.Value);
                                    break;
                                case XmlNodeType.CDATA:
                                    writer.WriteCData(Exclusions.Contains(reader.Value) ? reader.Value : navigator.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, mappingXpath, reader.Value)).Value);
                                    break;
                                case XmlNodeType.EntityReference:
                                    writer.WriteEntityRef(reader.Name);
                                    break;
                            }
                            if (writeEndElement) { writer.WriteEndElement(); }
                        }
                    }
                    writer.Flush();
                }
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally
            {
               if (tempOutput != null) { tempOutput.Dispose(); }
            }

            if (value.CanSeek) { value.Seek(obfustatedStartingPosition, SeekOrigin.Begin); }
            if (mapping.CanSeek) { mapping.Seek(mappingStartingPosition, SeekOrigin.Begin); }

            return output;
        }

        private ObfuscatorMapping ManageStructureMappings(string nodeName, XmlNodeType nodeType)
        {
            var existing = true;
            var nodeNameHash = ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}s@t£r$u%{1}", nodeName, nodeType));
            if (!Mappings.ContainsKey(nodeNameHash))
            {
                lock (Mappings)
                {
                    existing = false;
                    switch (nodeType)
                    {
                        case XmlNodeType.Attribute:
                        case XmlNodeType.Element:
                            Mappings.Add(nodeNameHash, new ObfuscatorMapping(GenerateObfuscatedValue(), nodeName));
                            break;
                    }
                }
            }
            if (existing) { Mappings[nodeNameHash].IncrementCount(); }
            return Mappings[nodeNameHash];
        }

        private ObfuscatorMapping ManageDataMappings(string nodeValue, XmlNodeType nodeType)
        {
            var existing = true;
            var nodeValueHash = ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}%d$a£t@a{1}", nodeValue, nodeType));
            if (!Mappings.ContainsKey(nodeValueHash))
            {
                lock (Mappings)
                {
                    existing = false;
                    switch (nodeType)
                    {
                        case XmlNodeType.Attribute:
                        case XmlNodeType.Element:
                        case XmlNodeType.CDATA:
                        case XmlNodeType.Text:
                            Mappings.Add(nodeValueHash, new ObfuscatorMapping(GenerateObfuscatedValue(), nodeValue));
                            break;
                    }
                }
            }
            if (existing) { Mappings[nodeValueHash].IncrementCount(); }            
            return Mappings[nodeValueHash];
        }

        private void WriteAttributeString(XmlWriter writer, XmlReader reader)
        {
            var structureMapping = ManageStructureMappings(reader.LocalName, reader.NodeType);
            var mapping = ManageDataMappings(reader.Value, reader.NodeType);
            writer.WriteAttributeString(reader.Prefix, Exclusions.Contains(structureMapping.Original) ? structureMapping.Original : structureMapping.Obfuscated, reader.NamespaceURI, Exclusions.Contains(mapping.Original) ? mapping.Original : mapping.Obfuscated);
        }

        private void WriteStartElement(XmlWriter writer, XmlReader reader)
        {
            var mapping = ManageStructureMappings(reader.LocalName, reader.NodeType);
            writer.WriteStartElement(reader.Prefix, Exclusions.Contains(mapping.Original) ? mapping.Original : mapping.Obfuscated, reader.NamespaceURI);
        }

        private void WriteCData(XmlWriter writer, XmlReader reader)
        {
            var mapping = ManageDataMappings(reader.Value, reader.NodeType);
            writer.WriteCData(Exclusions.Contains(mapping.Original) ? mapping.Original : mapping.Obfuscated);
        }
        
        private void WriteString(XmlWriter writer, XmlReader reader)
        {
            var mapping = ManageDataMappings(reader.Value, reader.NodeType);
            writer.WriteString(Exclusions.Contains(mapping.Original) ? mapping.Original : mapping.Obfuscated);
        }
        #endregion
    }
}