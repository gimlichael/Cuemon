using System;
using Cuemon.Diagnostics;
using Cuemon.IO;
using Cuemon.Xml.Serialization.Formatters;

namespace Cuemon.Extensions.Xml.Serialization.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="ExceptionDescriptor"/> class.
    /// </summary>
    public static class ExceptionDescriptorExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="descriptor"/> to its equivalent string representation in XML format.
        /// </summary>
        /// <param name="descriptor">The <see cref="ExceptionDescriptor"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>An XML formatted string that represents the specified <paramref name="descriptor"/>.</returns>
        public static string ToInsightsXmlString(this ExceptionDescriptor descriptor, Action<ExceptionDescriptorOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            var formatter = new XmlFormatter(o =>
            {
                o.SynchronizeWithXmlConvert = true;
                o.IncludeExceptionStackTrace = options.IncludeStackTrace;
                o.IncludeExceptionDescriptorFailure = options.IncludeFailure;
                o.IncludeExceptionDescriptorEvidence = options.IncludeEvidence;
            });
            using (var xml = formatter.Serialize(descriptor))
            {
                return Decorator.Enclose(xml).ToEncodedString();
            }
        }
    }
}