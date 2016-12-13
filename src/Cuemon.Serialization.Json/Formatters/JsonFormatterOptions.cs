using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cuemon.Serialization.Formatters;
using Cuemon.Serialization.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Serialization.Json.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="JsonFormatter"/> operations.
    /// </summary>
    /// <seealso cref="FormatterOptions{TReader,TWriter,TConverter}" />
    public class JsonFormatterOptions : FormatterOptions<JsonReader, JsonWriter, JsonConverter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="JsonFormatterOptions"/>.
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
        ///         <term><see cref="Settings"/></term>
        ///         <description><see cref="JsonSerializerSettings"/></description>
        ///     </item>
        /// </list>
        /// A default initialization of <see cref="JsonSerializerSettings"/> is performed while adding a <see cref="CamelCasePropertyNamesContractResolver"/> to
        /// <see cref="JsonSerializerSettings.ContractResolver"/>, adding a <see cref="StringFlagsEnumConverter"/> to <see cref="JsonSerializerSettings.Converters"/> and 
        /// setting <see cref="JsonSerializerSettings.NullValueHandling"/> to <see cref="NullValueHandling.Ignore"/>.
        /// </remarks>
        public JsonFormatterOptions()
        {
            Converter = null;
            Settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Settings.Converters.Add(new StringFlagsEnumConverter());
        }

        /// <summary>
        /// Gets or sets the settings to support the <see cref="JsonFormatter"/>.
        /// </summary>
        /// <returns>A <see cref="JsonSerializerSettings"/> instance that specifies a set of features to support the <see cref="JsonFormatter"/> object.</returns>
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that generates an object from its JSON representation.
        /// </summary>
        /// <value>The function delegate that generates an object from its JSON representation.</value>
        public override Func<JsonReader, Type, object> ReaderFormatter { get; set; }

        /// <summary>
        /// Gets or sets the delegate that converts an object into its JSON representation.
        /// </summary>
        /// <value>The delegate that converts an object into its JSON representation.</value>
        public override Action<JsonWriter, object> WriterFormatter { get; set; }

        /// <summary>
        /// Gets the, by <see cref="Type"/>, specialized delegate that converts an object into its JSON representation.
        /// </summary>
        /// <value>A specialized delegate, by <see cref="Type"/>, that converts an object into its JSON representation.</value>
        public override IDictionary<Type, Action<JsonWriter, object>> WriterFormatters { get; } = new Dictionary<Type, Action<JsonWriter, object>>()
        {
            {
                typeof(TimeSpan), (writer, o) =>
                {
                    var span = (TimeSpan)o;
                    writer.WriteStartObject();
                    writer.WritePropertyName("ticks");
                    writer.WriteValue(span.Ticks);
                    writer.WritePropertyName("days");
                    writer.WriteValue(span.Days);
                    writer.WritePropertyName("hours");
                    writer.WriteValue(span.Hours);
                    writer.WritePropertyName("minutes");
                    writer.WriteValue(span.Minutes);
                    writer.WritePropertyName("seconds");
                    writer.WriteValue(span.Seconds);
                    writer.WritePropertyName("totalDays");
                    writer.WriteValue(span.TotalDays);
                    writer.WritePropertyName("totalHours");
                    writer.WriteValue(span.TotalHours);
                    writer.WritePropertyName("totalMilliseconds");
                    writer.WriteValue(span.TotalMilliseconds);
                    writer.WritePropertyName("totalMinutes");
                    writer.WriteValue(span.TotalMinutes);
                    writer.WritePropertyName("totalSeconds");
                    writer.WriteValue(span.TotalSeconds);
                    writer.WriteEndObject();
                }
            },
            {
                typeof(ExceptionDescriptor), (writer, o) =>
                {
                    ExceptionDescriptor descriptor = (ExceptionDescriptor) o;
                    writer.WriteStartObject();
                    if (!descriptor.RequestId.IsNullOrWhiteSpace())
                    {
                        writer.WritePropertyName("requestId");
                        writer.WriteValue(descriptor.RequestId);
                    }
                    writer.WritePropertyName("code");
                    writer.WriteValue(descriptor.Code.ToString(CultureInfo.InvariantCulture));
                    writer.WritePropertyName("message");
                    writer.WriteValue(descriptor.Message);
                    writer.WritePropertyName("failure");
                    writer.WriteStartObject();
                    writer.WritePropertyName("type");
                    writer.WriteValue(descriptor.Failure.GetType().FullName);
                    writer.WritePropertyName("message");
                    writer.WriteValue(descriptor.Failure.Message);
                    if (descriptor.Failure.Data.Count > 0)
                    {
                        writer.WritePropertyName("data");
                        writer.WriteStartObject();
                        foreach (DictionaryEntry entry in descriptor.Failure.Data)
                        {
                            writer.WritePropertyName(entry.Key.ToString());
                            writer.WriteValue(entry.Value);
                        }
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }
            }
        };

        /// <summary>
        /// Gets the, by <see cref="Type"/>, specialized function delegate that generates an object from its JSON representation.
        /// </summary>
        /// <value>A specialized function delegate, by <see cref="Type"/>, that generates an object from its JSON representation.</value>
        public override IDictionary<Type, Func<JsonReader, Type, object>> ReaderFormatters { get; } = new Dictionary<Type, Func<JsonReader, Type, object>>()
        {
            {
                typeof(TimeSpan), (reader, type) => reader.ToHierarchy().UseTimeSpanFormatter()
            }
        };
    }
}