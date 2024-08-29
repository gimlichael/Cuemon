using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json.Converters;
using Cuemon.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Formatters
{
    /// <summary>
    /// Specifies options that is related to <see cref="NewtonsoftJsonFormatter"/> operations.
    /// </summary>
    public class NewtonsoftJsonFormatterOptions : IExceptionDescriptorOptions, IContentNegotiation, IValidatableParameterObject
    {
        private readonly Lock _locker = new();
        private bool _refreshed;

        /// <summary>
        /// Provides the default/fallback media type that the associated formatter should use when content negotiation either fails or is absent.
        /// </summary>
        /// <value>The media type that the associated formatter should use when content negotiation either fails or is absent.</value>
        public static MediaTypeHeaderValue DefaultMediaType { get; } = new("application/json");

        static NewtonsoftJsonFormatterOptions()
        {
            DefaultConverters = list =>
            {
                list.AddDataPairConverter();
                list.AddStringFlagsEnumConverter();
                list.AddStringEnumConverter();
                list.AddTransientFaultExceptionConverter();
                list.AddFailureConverter();
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonFormatterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="NewtonsoftJsonFormatterOptions"/>.
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
        ///         <description><c>false</c></description>
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
        public NewtonsoftJsonFormatterOptions()
        {
            Settings = new JsonSerializerSettings()
            {
                CheckAdditionalContent = true,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTime,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "O",
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                ContractResolver = DynamicContractResolver.Create(new CamelCasePropertyNamesContractResolver()
                {
                    IgnoreSerializableInterface = true
                }, JsonPropertyHandler)
            };
            DefaultConverters?.Invoke(Settings.Converters);
            SensitivityDetails = FaultSensitivityDetails.None;
            SupportedMediaTypes = new List<MediaTypeHeaderValue>()
            {
                DefaultMediaType,
                new("text/json"),
                new("application/problem+json")
            };
        }

        private void JsonPropertyHandler(PropertyInfo pi, JsonProperty jp)
        {
            Func<Type, bool> skipPropertyType = source =>
            {
                switch (Type.GetTypeCode(source))
                {
                    default:
                        if (Decorator.Enclose(source).HasKeyValuePairImplementation()) { return true; }
                        if (Decorator.Enclose(source).HasTypes(typeof(MemberInfo)) && source != typeof(Type)) { return true; }
                        return false;
                }
            };
            Func<PropertyInfo, bool> skipProperty = property =>
            {
                return (property.PropertyType.GetTypeInfo().IsMarshalByRef ||
                        property.PropertyType.GetTypeInfo().IsSubclassOf(typeof(Delegate)) ||
                        property.Name.Equals("SyncRoot", StringComparison.Ordinal) ||
                        property.Name.Equals("IsReadOnly", StringComparison.Ordinal) ||
                        property.Name.Equals("IsFixedSize", StringComparison.Ordinal) ||
                        property.Name.Equals("IsSynchronized", StringComparison.Ordinal) ||
                        property.Name.Equals("Count", StringComparison.Ordinal) ||
                        property.Name.Equals("HResult", StringComparison.Ordinal) ||
                        property.Name.Equals("Parent", StringComparison.Ordinal) ||
                        property.Name.Equals("TargetSite", StringComparison.Ordinal));
            };

            var skipSerialization = skipProperty(pi) || skipPropertyType(pi.PropertyType);
            jp.ShouldSerialize =  _ => !skipSerialization;
            jp.ShouldDeserialize = _ => !skipSerialization;
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="NewtonsoftJsonFormatterOptions"/> is initialized and propagates registered <see cref="JsonConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="JsonConverter"/> implementations when <see cref="NewtonsoftJsonFormatterOptions"/> is initialized.</value>
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
        /// Gets or sets the settings to support the <see cref="NewtonsoftJsonFormatter"/>.
        /// </summary>
        /// <returns>A <see cref="JsonSerializerSettings"/> instance that specifies a set of features to support the <see cref="NewtonsoftJsonFormatter"/> object.</returns>
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="NewtonsoftJsonFormatter"/>.
        /// </summary>
        /// <returns>A collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="NewtonsoftJsonFormatter"/>.</returns>
        public IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }

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
            Validator.ThrowIfInvalidState(Settings == null);
            Validator.ThrowIfInvalidState(SupportedMediaTypes == null);
        }
    }
}
