using System;
using System.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Xml.Serialization.Formatters
{
    /// <summary>
    /// Configuration options for <see cref="XmlFormatter"/>.
    /// </summary>
    public class XmlFormatterOptions
    {
        static XmlFormatterOptions()
        {
            DefaultConverters = list =>
            {
                Decorator.Enclose(list)
                    .AddExceptionDescriptorConverter()
                    .AddEnumerableConverter()
                    .AddUriConverter()
                    .AddDateTimeConverter()
                    .AddTimeSpanConverter()
                    .AddStringConverter();
            };
        }

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
        ///     <item>
        ///         <term><see cref="IncludeExceptionDescriptorFailure"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IncludeExceptionDescriptorEvidence"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XmlFormatterOptions()
        {
            IncludeExceptionDescriptorFailure = true;
            IncludeExceptionDescriptorEvidence = true;
            IncludeExceptionStackTrace = false;
            Settings = new XmlSerializerOptions();
            Decorator.Enclose(Settings.Converters).AddExceptionConverter(() => IncludeExceptionStackTrace);
            DefaultConverters?.Invoke(Settings.Converters);
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="XmlFormatterOptions"/> is initialized and propagates registered <see cref="XmlConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="XmlConverter"/> implementations when <see cref="XmlFormatterOptions"/> is initialized.</value>
        public static Action<IList<XmlConverter>> DefaultConverters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the stack of an <see cref="Exception"/> is included in the converter that handles exceptions.
        /// </summary>
        /// <value><c>true</c> if the stack of an <see cref="Exception"/> is included in the converter that handles exceptions; otherwise, <c>false</c>.</value>
        public bool IncludeExceptionStackTrace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the failure of an <see cref="ExceptionDescriptor"/> is included in the converter that handles exception descriptors.
        /// </summary>
        /// <value><c>true</c> if the failure of an <see cref="ExceptionDescriptor"/> is included in the converter that handles exception descriptors; otherwise, <c>false</c>.</value>
        public bool IncludeExceptionDescriptorFailure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the evidence of an <see cref="ExceptionDescriptor"/> is included in the converter that handles exception descriptors.
        /// </summary>
        /// <value><c>true</c> if the evidence of an <see cref="ExceptionDescriptor"/> is included in the converter that handles exception descriptors; otherwise, <c>false</c>.</value>
        public bool IncludeExceptionDescriptorEvidence { get; set; }

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