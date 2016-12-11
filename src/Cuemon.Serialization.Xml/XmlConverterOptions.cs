using System.Xml;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlConverterOptions"/> class.
    /// </summary>
    public class XmlConverterOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlConverterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="XmlConverter"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="WriterSettings"/></term>
        ///         <description><see cref="XmlWriterUtility.CreateSettings"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RootName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XmlConverterOptions()
        {
            WriterSettings = XmlWriterUtility.CreateSettings();
            ReaderSettings = XmlReaderUtility.CreateSettings();
            RootName = null;
        }

        /// <summary>
        /// Gets or sets the <see cref="XmlReader"/> settings to support the <see cref="XmlConverter"/>.
        /// </summary>
        /// <returns>A <see cref="XmlReaderSettings"/> instance that specifies a set of features to support the <see cref="XmlConverter"/> object.</returns>
        public XmlReaderSettings ReaderSettings { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="XmlWriter"/> settings to support the <see cref="XmlConverter"/>.
        /// </summary>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlConverter"/> object.</returns>
        public XmlWriterSettings WriterSettings { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML root element.
        /// </summary>
        /// <value>The name of the XML root element.</value>
        public XmlQualifiedEntity RootName { get; set; }
    }
}