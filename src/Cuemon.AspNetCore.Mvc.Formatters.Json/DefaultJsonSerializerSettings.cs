using Cuemon.AspNetCore.Mvc.Formatters.Json.Converters;
using Cuemon.Serialization.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cuemon.AspNetCore.Mvc.Formatters.Json
{
    /// <summary>
    /// Specifies the default settings on a <see cref="JsonSerializer"/> object as interpreted by this framework.
    /// </summary>
    public class DefaultJsonSerializerSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializerSettings"/> class.
        /// </summary>
        public DefaultJsonSerializerSettings()
        {
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            MissingMemberHandling = MissingMemberHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            DateParseHandling = DateParseHandling.DateTimeOffset;
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Converters.AddStringValuesConverter();
            Converters.AddHttpExceptionDescriptorConverter();
            Converters.AddExceptionConverter(() => IncludeExceptionStackTrace);
            Converters.AddDataPairConverter();
            Converters.AddStringEnumConverter();
            Converters.AddStringFlagsEnumConverter();
            Converters.AddTimeSpanConverter();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the stack of an exception is included in the converter that handles exceptions.
        /// </summary>
        /// <value><c>true</c> if the stack of an exception is included in the converter that handles exceptions; otherwise, <c>false</c>.</value>
        public bool IncludeExceptionStackTrace { get; set; }
    }
}