using System;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.IO;
using Cuemon.Runtime.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="ExceptionDescriptor"/> class.
    /// </summary>
    public static class ExceptionDescriptorExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="descriptor"/> to its equivalent string representation in JSON format.
        /// </summary>
        /// <param name="descriptor">The <see cref="ExceptionDescriptor"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which may be configured.</param>
        /// <returns>A JSON formatted string that represents the specified <paramref name="descriptor"/>.</returns>
        public static string ToInsightsJsonString(this ExceptionDescriptor descriptor, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            var options = setup.Configure();
            var formatter = new JsonFormatter(o =>
            {
                o.SynchronizeWithJsonConvert = true;
                o.IncludeExceptionStackTrace = options.IncludeStackTrace;
                o.IncludeExceptionDescriptorFailure = options.IncludeFailure;
                o.IncludeExceptionDescriptorEvidence = options.IncludeEvidence;
            });
            using (var json = formatter.Serialize(descriptor))
            {
                return Decorator.Enclose(json).ToEncodedString();
            }
        }
    }
}