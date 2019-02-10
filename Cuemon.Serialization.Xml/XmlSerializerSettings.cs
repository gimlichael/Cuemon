using System;
using System.Collections.Generic;
using System.Xml;
using Cuemon.Serialization.Xml.Converters;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Specifies options that is related to <see cref="XmlSerializer"/> operations.
    /// </summary>
    public class XmlSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializerSettings"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DefaultXmlConverter"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Reader"/></term>
        ///         <description><see cref="XmlReaderUtility.CreateSettings"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Writer"/></term>
        ///         <description><see cref="XmlWriterUtility.CreateSettings"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Converters"/></term>
        ///         <description><see cref="List{XmlConverter}"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RootName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XmlSerializerSettings()
        {
            Writer = XmlWriterUtility.CreateSettings();
            Reader = XmlReaderUtility.CreateSettings();
            Converters = new List<XmlConverter>();
            DefaultConverters?.Invoke(Converters);
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="XmlSerializerSettings"/> is initialized and propagates registered <see cref="XmlConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="XmlConverter"/> implementations when <see cref="XmlSerializerSettings"/> is initialized.</value>
        public static Action<IList<XmlConverter>> DefaultConverters { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="XmlConverter" /> collection that will be used during serialization.
        /// </summary>
        /// <value>The converters that will be used during serialization.</value>
        public IList<XmlConverter> Converters { get; }

        /// <summary>
        /// Gets or sets the <see cref="XmlReaderSettings"/> to support the <see cref="XmlSerializer"/>.
        /// </summary>
        /// <value>A <see cref="XmlReaderSettings"/> instance that specifies a set of features to support the <see cref="XmlSerializer"/> object.</value>
        public XmlReaderSettings Reader { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="XmlWriterSettings"/> to support the <see cref="XmlSerializer"/>.
        /// </summary>
        /// <value>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlSerializer"/> object.</value>
        public XmlWriterSettings Writer { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML root element.
        /// </summary>
        /// <value>The name of the XML root element.</value>
        public XmlQualifiedEntity RootName { get; set; }
    }
}