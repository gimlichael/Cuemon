using Cuemon.Serialization.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Serialization.Json.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="JsonFormatter"/> operations.
    /// </summary>
    public class JsonFormatterOptions
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
        ///         <term><see cref="Settings"/></term>
        ///         <description><see cref="JsonSerializerSettings"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SynchronizeWithJsonConvert"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IncludeExceptionStackTrace"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public JsonFormatterOptions()
        {
            Settings = new JsonSerializerSettings()
            {
                CheckAdditionalContent = true,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Settings.Converters.AddStringFlagsEnumConverter();
            Settings.Converters.AddStringEnumConverter();
            Settings.Converters.AddExceptionConverter(() => IncludeExceptionStackTrace);
            Settings.Converters.AddExceptionDescriptorConverter();
            Settings.Converters.AddTimeSpanConverter();
            Settings.Converters.AddDataPairConverter();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the stack of an exception is included in the converter that handles exceptions.
        /// </summary>
        /// <value><c>true</c> if the stack of an exception is included in the converter that handles exceptions; otherwise, <c>false</c>.</value>
        public bool IncludeExceptionStackTrace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Settings"/> should be synchronized on <see cref="JsonConvert.DefaultSettings"/>.
        /// </summary>
        /// <value><c>true</c> if <see cref="Settings"/> should be synchronized on <see cref="JsonConvert.DefaultSettings"/>; otherwise, <c>false</c>.</value>
        public bool SynchronizeWithJsonConvert { get; set; }

        /// <summary>
        /// Gets or sets the settings to support the <see cref="JsonFormatter"/>.
        /// </summary>
        /// <returns>A <see cref="JsonSerializerSettings"/> instance that specifies a set of features to support the <see cref="JsonFormatter"/> object.</returns>
        public JsonSerializerSettings Settings { get; set; }
    }
}