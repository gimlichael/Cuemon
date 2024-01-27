using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Text.Yaml.Formatters
{
    /// <summary>
    /// Configuration options for <see cref="YamlFormatter"/>.
    /// </summary>
    public sealed class YamlFormatterOptions : EncodingOptions, IExceptionDescriptorOptions, IValidatableParameterObject
    {
	    private readonly object _locker = new();
	    private bool _refreshed;

        static YamlFormatterOptions()
        {
        }

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
        /// </list>
        /// </remarks>
        public YamlFormatterOptions()
        {
            Settings = new YamlSerializerOptions();
            DefaultConverters?.Invoke(Settings.Converters);
            SensitivityDetails = FaultSensitivityDetails.None;
            SupportedMediaTypes = new List<MediaTypeHeaderValue>()
            {
	            new("application/yaml"),
	            new("text/yaml"),
	            new("text/plain"),
				new("text/plain")
				{
                    CharSet = "utf-8"
				}
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
        public IList<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }

        internal YamlSerializerOptions RefreshWithConverterDependencies()
        {
	        lock (_locker)
	        {
		        if (!_refreshed)
		        {
			        _refreshed = true;
			        Settings.Converters.Add(new ExceptionConverter(SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), SensitivityDetails.HasFlag(FaultSensitivityDetails.Data)));
			        Settings.Converters.Add(new ExceptionDescriptorConverter(o => o.SensitivityDetails = SensitivityDetails));
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
