using System;
using Cuemon.AspNetCore.Mvc.Formatters.Json.Converters;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json.Converters;
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
            IncludeExceptionDescriptorFailure = true;
            IncludeExceptionDescriptorEvidence = true;
            IncludeExceptionStackTrace = false;
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            MissingMemberHandling = MissingMemberHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            DateParseHandling = DateParseHandling.DateTimeOffset;
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Converters.AddStringValuesConverter();
            Converters.AddHttpExceptionDescriptorConverter(o =>
            {
                o.IncludeEvidence = IncludeExceptionDescriptorEvidence;
                o.IncludeFailure = IncludeExceptionDescriptorFailure;
            });
            Converters.AddExceptionConverter(() => IncludeExceptionStackTrace);
            Converters.AddDataPairConverter();
            Converters.AddStringEnumConverter();
            Converters.AddStringFlagsEnumConverter();
            Converters.AddTimeSpanConverter();
        }

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
    }
}