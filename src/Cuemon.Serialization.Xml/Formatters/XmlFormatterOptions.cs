using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using Cuemon.IO;
using Cuemon.Serialization.Formatters;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="XmlFormatter"/> operations.
    /// </summary>
    /// <seealso cref="FormatterOptions{TReader,TReaderOptions,TWriter,TWriterOptions,TConverter}" />
    public class XmlFormatterOptions : FormatterOptions<XmlReader, XmlReaderSettings, XmlWriter, XmlWriterSettings, XmlConverter>
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
        ///         <term><see cref="FormatterOptions{TReader,TReaderOptions,TWriter,TWriterOptions,TConverter}.Converter"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ReaderSettings"/></term>
        ///         <description><see cref="XmlReaderUtility.CreateSettings"/></description>
        ///     </item>
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
        public XmlFormatterOptions()
        {
            Converter = null;
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
        /// Gets or sets the <see cref="XmlWriter"/> settings to support the <see cref="XmlFormatter"/>.
        /// </summary>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlFormatter"/> object.</returns>
        public XmlWriterSettings WriterSettings { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML root element.
        /// </summary>
        /// <value>The name of the XML root element.</value>
        public XmlQualifiedEntity RootName { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that generates an object from its XML representation.
        /// </summary>
        /// <value>The function delegate that generates an object from its XML representation.</value>
        public override Func<XmlReader, XmlReaderSettings, Type, object> ReaderFormatter { get; set; }

        /// <summary>
        /// Gets or sets the delegate that converts an object into its XML representation.
        /// </summary>
        /// <value>The delegate that converts an object into its XML representation.</value>
        public override Action<XmlWriter, XmlWriterSettings, object> WriterFormatter { get; set; }

        /// <summary>
        /// Gets the, by <see cref="Type"/>, specialized delegate that converts an object into its XML representation.
        /// </summary>
        /// <value>A specialized delegate, by <see cref="Type"/>, that converts an object into its XML representation.</value>
        public override IDictionary<Type, Action<XmlWriter, XmlWriterSettings, object>> WriterFormatters { get; } = new Dictionary<Type, Action<XmlWriter, XmlWriterSettings, object>>()
        {
            {
                typeof(ExceptionDescriptor), (writer, settings, o) =>
                {
                    ExceptionDescriptor descriptor = (ExceptionDescriptor) o;
                    writer.WriteStartElement("ExceptionDescriptor");
                    if (!descriptor.RequestId.IsNullOrWhiteSpace()) { writer.WriteElementString("RequestId", descriptor.RequestId); }
                    writer.WriteElementString("Code", descriptor.Code.ToString(CultureInfo.InvariantCulture));
                    writer.WriteElementString("Message", descriptor.Message);
                    writer.WriteStartElement("Failure");
                    writer.WriteAttributeString("type", descriptor.Failure.GetType().FullName);
                    writer.WriteElementString("Message", descriptor.Failure.Message);
                    if (descriptor.Failure.Data.Count > 0)
                    {
                        writer.WriteStartElement("Data");
                        foreach (DictionaryEntry entry in descriptor.Failure.Data)
                        {
                            writer.WriteStartElement(XmlUtility.SanitizeElementName(entry.Key.ToString()));
                            writer.WriteString(XmlUtility.SanitizeElementText(entry.Value.ToString()));
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    if (descriptor.HelpLink != null)
                    {
                        writer.WriteElementString("HelpLink", descriptor.HelpLink.OriginalString);
                    }
                    writer.WriteEndElement();
                }
            },
            {
                typeof(Exception), (writer, settings, o) =>
                {
                    Exception ex = (Exception) o;
                    XmlStreamConverter.WriteException(writer, ex, true);
                }
            }
        };

        /// <summary>
        /// Gets the, by <see cref="Type"/>, specialized function delegate that generates an object from its XML representation.
        /// </summary>
        /// <value>A specialized function delegate, by <see cref="Type"/>, that generates an object from its XML representation.</value>
        public override IDictionary<Type, Func<XmlReader, XmlReaderSettings, Type, object>> ReaderFormatters { get; } = new Dictionary<Type, Func<XmlReader, XmlReaderSettings, Type, object>>()
        {
            {
                typeof(Uri), (reader, settings, type) => reader.ToHierarchy().UseUriFormatter()
            },
            {
                typeof(DateTime), (reader, settings, type) => reader.ToHierarchy().UseDateTimeFormatter()
            },
            {
                typeof(IConvertible), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(decimal), (reader, settings, type) => reader.ToHierarchy().UseDecimalFormatter()
            },
            {
                typeof(bool), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(int), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(long), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(double), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(byte), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(char), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(uint), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(ulong), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(sbyte), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(float), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(short), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(ushort), (reader, settings, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(Guid), (reader, settings, type) => reader.ToHierarchy().UseGuidFormatter()
            },
            {
                typeof(string), (reader, settings, type) => reader.ToHierarchy().UseStringFormatter()
            },
            {
                typeof(IList), (reader, settings, type) => reader.ToHierarchy().UseCollection(type.GetGenericArguments().First())
            },
            {
                typeof(IDictionary), (reader, settings, type) => reader.ToHierarchy().UseDictionary(type.GetGenericArguments())
            }
        };
    }
}