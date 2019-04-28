using Cuemon.Extensions.Xml.Serialization.Converters;

namespace Cuemon.Extensions.Xml.Serialization.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="XmlFormatter"/> operations.
    /// </summary>
    public class XmlFormatterOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="XmlFormatterOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Settings"/></term>
        ///         <description><see cref="XmlSerializerOptions"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SynchronizeWithXmlConvert"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IncludeExceptionStackTrace"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XmlFormatterOptions()
        {
            Settings = new XmlSerializerOptions();
            XmlSerializerOptions.DefaultConverters = list =>
            {
                list.AddExceptionDescriptorConverter();
                list.AddExceptionConverter(() => IncludeExceptionStackTrace);
                list.AddEnumerableConverter();
                list.AddUriConverter();
                list.AddDateTimeConverter();
                list.AddTimeSpanConverter();
                list.AddStringConverter();
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether the stack of an exception is included in the converter that handles exceptions.
        /// </summary>
        /// <value><c>true</c> if the stack of an exception is included in the converter that handles exceptions; otherwise, <c>false</c>.</value>
        public bool IncludeExceptionStackTrace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Settings"/> should be synchronized on <see cref="XmlConvert.DefaultSettings"/>.
        /// </summary>
        /// <value><c>true</c> if <see cref="Settings"/> should be synchronized on <see cref="XmlConvert.DefaultSettings"/>; otherwise, <c>false</c>.</value>
        public bool SynchronizeWithXmlConvert { get; set; }

        /// <summary>
        /// Gets or sets the settings to support the <see cref="XmlFormatter"/>.
        /// </summary>
        /// <returns>A <see cref="XmlSerializerOptions"/> instance that specifies a set of features to support the <see cref="XmlFormatter"/> object.</returns>
        public XmlSerializerOptions Settings { get; }
    }
}