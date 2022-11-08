using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="JsonFormatter"/> operations.
    /// </summary>
    public class JsonFormatterOptions : IValidatableParameterObject
    {
        private readonly object _locker = new();
        private bool _refreshed;

        static JsonFormatterOptions()
        {
            DefaultConverters = list =>
            {
                list.AddDataPairConverter();
                list.AddStringFlagsEnumConverter();
                list.AddStringEnumConverter();
            };
        }

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
        ///         <term><see cref="SensitivityDetails"/></term>
        ///         <description><see cref="FaultSensitivityDetails.None"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SupportedMediaTypes"/></term>
        ///         <description>
        ///             <code>
        ///new List&lt;MediaTypeHeaderValue&gt;()
        ///{
        ///    new("application/json"),
        ///    new("text/json")
        ///};
        ///             </code>
        ///         </description>
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
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                {
                    IgnoreSerializableInterface = true
                }
            };
            DefaultConverters?.Invoke(Settings.Converters);
            SensitivityDetails = FaultSensitivityDetails.None;
            SupportedMediaTypes = new List<MediaTypeHeaderValue>()
            {
                new("application/json"),
                new("text/json")
            };
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="JsonFormatterOptions"/> is initialized and propagates registered <see cref="JsonConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="JsonConverter"/> implementations when <see cref="JsonFormatterOptions"/> is initialized.</value>
        public static Action<IList<JsonConverter>> DefaultConverters { get; set; }

        /// <summary>
        /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
        /// </summary>
        /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
        public FaultSensitivityDetails SensitivityDetails { get; set; }

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

        /// <summary>
        /// Gets or sets the collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="JsonFormatter"/>.
        /// </summary>
        /// <returns>A collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="JsonFormatter"/>.</returns>
        public IList<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }

        internal JsonSerializerSettings RefreshWithConverterDependencies()
        {
            lock (_locker)
            {
                if (!_refreshed)
                {
                    _refreshed = true;
                    Settings.Converters.AddExceptionConverter(SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), SensitivityDetails.HasFlag(FaultSensitivityDetails.Data));
                    Settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(o => o.SensitivityDetails = SensitivityDetails);
                }
                return Settings;
            }
        }
        
        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Settings"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfObjectInDistress(Settings == null);
            Validator.ThrowIfObjectInDistress(SupportedMediaTypes == null);
        }
    }
}
