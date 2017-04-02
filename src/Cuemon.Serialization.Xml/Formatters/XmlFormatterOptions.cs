using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using Cuemon.Serialization.Formatters;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="XmlFormatter"/> operations.
    /// </summary>
    /// <seealso cref="FormatterOptions{TReader,TWriter,TConverter}" />
    public class XmlFormatterOptions : FormatterOptions<XmlReader, XmlWriter, XmlConverter>
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
        ///         <term><see cref="FormatterOptions{TReader,TWriter,TConverter}.Converter"/></term>
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
        public override Func<XmlReader, Type, object> ReaderFormatter { get; set; }

        /// <summary>
        /// Gets or sets the delegate that converts an object into its XML representation.
        /// </summary>
        /// <value>The delegate that converts an object into its XML representation.</value>
        public override Action<XmlWriter, object> WriterFormatter { get; set; }

        /// <summary>
        /// Gets the, by <see cref="Type"/>, specialized delegate that converts an object into its XML representation.
        /// </summary>
        /// <value>A specialized delegate, by <see cref="Type"/>, that converts an object into its XML representation.</value>
        public override IDictionary<Type, Action<XmlWriter, object>> WriterFormatters { get; } = new Dictionary<Type, Action<XmlWriter, object>>()
        {
            {
                typeof(ExceptionDescriptor), (writer, o) =>
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
            }
        };

        /// <summary>
        /// Gets the, by <see cref="Type"/>, specialized function delegate that generates an object from its XML representation.
        /// </summary>
        /// <value>A specialized function delegate, by <see cref="Type"/>, that generates an object from its XML representation.</value>
        public override IDictionary<Type, Func<XmlReader, Type, object>> ReaderFormatters { get; } = new Dictionary<Type, Func<XmlReader, Type, object>>()
        {
            {
                typeof(Uri), (reader, type) => reader.ToHierarchy().UseUriFormatter()
            },
            {
                typeof(DateTime), (reader, type) => reader.ToHierarchy().UseDateTimeFormatter()
            },
            {
                typeof(IConvertible), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(decimal), (reader, type) => reader.ToHierarchy().UseDecimalFormatter()
            },
            {
                typeof(bool), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(int), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(long), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(double), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(byte), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(char), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(uint), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(ulong), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(sbyte), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(float), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(short), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(ushort), (reader, type) => reader.ToHierarchy().UseConvertibleFormatter()
            },
            {
                typeof(Guid), (reader, type) => reader.ToHierarchy().UseGuidFormatter()
            },
            {
                typeof(string), (reader, type) => reader.ToHierarchy().UseStringFormatter()
            },
            {
                typeof(IList), (reader, type) => reader.ToHierarchy().UseCollection(type.GetGenericArguments().First())
            },
            {
                typeof(IDictionary), (reader, type) => reader.ToHierarchy().UseDictionary(type.GetGenericArguments())
            }
        };
    }
}