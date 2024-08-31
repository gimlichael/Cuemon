using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Extensions.YamlDotNet.Converters;
using Cuemon.Net.Http;
using Cuemon.Text;

namespace Cuemon.Extensions.YamlDotNet.Formatters
{
    /// <summary>
    /// Configuration options for <see cref="YamlFormatter"/>.
    /// </summary>
    public class YamlFormatterOptions : EncodingOptions, IExceptionDescriptorOptions, IContentNegotiation, IValidatableParameterObject
    {
        private readonly Lock _locker = new();
        private bool _refreshed;

        /// <summary>
        /// Provides the default/fallback media type that the associated formatter should use when content negotiation either fails or is absent.
        /// </summary>
        /// <value>The media type that the associated formatter should use when content negotiation either fails or is absent.</value>
        public static MediaTypeHeaderValue DefaultMediaType { get; } = new("text/plain")
        {
            CharSet = "utf-8"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlFormatterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="YamlFormatterOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Settings"/></term>
        ///         <description><c>new YamlSerializerOptions();</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SensitivityDetails"/></term>
        ///         <description><see cref="FaultSensitivityDetails.None"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SupportedMediaTypes"/></term>
        ///         <description><code>
        ///new List&lt;MediaTypeHeaderValue&gt;()
        ///{
        ///    DefaultMediaType,
        ///    new("text/plain"),
        ///    new("application/yaml"),
        ///    new("text/yaml"),
        ///    new("*/*")
        ///};</code></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public YamlFormatterOptions()
        {
            Settings = new YamlSerializerOptions();
            DefaultConverters?.Invoke(Settings.Converters);
            SensitivityDetails = FaultSensitivityDetails.None;
            SupportedMediaTypes = new List<MediaTypeHeaderValue>()
            {
                DefaultMediaType,
                new("text/plain"),
                new("application/yaml"),
                new("text/yaml"),
                new("*/*")
            };
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="YamlFormatterOptions"/> is initialized and propagates registered <see cref="YamlConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="YamlConverter"/> implementations when <see cref="YamlFormatterOptions"/> is initialized.</value>
        public static Action<IList<YamlConverter>> DefaultConverters { get; set; }

        /// <summary>
        /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
        /// </summary>
        /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
        public FaultSensitivityDetails SensitivityDetails { get; set; }

        /// <summary>
        /// Gets or sets the settings to support the <see cref="YamlConverter"/>.
        /// </summary>
        /// <returns>A <see cref="YamlSerializerOptions"/> instance that specifies a set of features to support the <see cref="YamlConverter"/> object.</returns>
        public YamlSerializerOptions Settings { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="YamlFormatter"/>.
        /// </summary>
        /// <returns>A collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="YamlFormatter"/>.</returns>
        public IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }

        internal YamlSerializerOptions RefreshWithConverterDependencies()
        {
            lock (_locker)
            {
                if (!_refreshed)
                {
                    _refreshed = true;
                    if (Settings.Converters.All(c => c.GetType() != typeof(ExceptionConverter))) { Settings.Converters.Add(new ExceptionConverter(SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))); }
                    if (Settings.Converters.All(c => c.GetType() != typeof(ExceptionDescriptorConverter))) { Settings.Converters.Add(new ExceptionDescriptorConverter(o => o.SensitivityDetails = SensitivityDetails)); }
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
