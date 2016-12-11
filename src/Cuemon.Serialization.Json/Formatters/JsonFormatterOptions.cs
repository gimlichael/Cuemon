using System;
using System.Collections.Generic;
using Cuemon.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="JsonFormatterOptions"/> operations.
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
        /// </remarks>
        public JsonFormatterOptions()
        {
            Converter = null;
            Settings = new JsonSerializerSettings();
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
                    writer.WritePropertyName("Ticks");
                    writer.WriteValue(span.Ticks);
                    writer.WritePropertyName("Days");
                    writer.WriteValue(span.Days);
                    writer.WritePropertyName("Hours");
                    writer.WriteValue(span.Hours);
                    writer.WritePropertyName("Minutes");
                    writer.WriteValue(span.Minutes);
                    writer.WritePropertyName("Seconds");
                    writer.WriteValue(span.Seconds);
                    writer.WritePropertyName("TotalDays");
                    writer.WriteValue(span.TotalDays);
                    writer.WritePropertyName("TotalHours");
                    writer.WriteValue(span.TotalHours);
                    writer.WritePropertyName("TotalMilliseconds");
                    writer.WriteValue(span.TotalMilliseconds);
                    writer.WritePropertyName("TotalMinutes");
                    writer.WriteValue(span.TotalMinutes);
                    writer.WritePropertyName("TotalSeconds");
                    writer.WriteValue(span.TotalSeconds);
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