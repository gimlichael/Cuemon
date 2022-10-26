using System;
using System.Collections.Generic;
using Cuemon.Configuration;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Text.Yaml.Formatters
{
    /// <summary>
    /// Configuration options for <see cref="YamlFormatter"/>.
    /// </summary>
    public sealed class YamlFormatterOptions : EncodingOptions, IValidatableParameterObject
    {
        static YamlFormatterOptions()
        {
            DefaultConverters = list =>
            {
                list.Add(new ExceptionConverter());
            };
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
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="YamlFormatterOptions"/> is initialized and propagates registered <see cref="YamlConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="YamlConverter"/> implementations when <see cref="YamlFormatterOptions"/> is initialized.</value>
        public static Action<IList<YamlConverter>> DefaultConverters { get; set; }

        /// <summary>
        /// Gets or sets the settings to support the <see cref="YamlConverter"/>.
        /// </summary>
        /// <returns>A <see cref="YamlSerializerOptions"/> instance that specifies a set of features to support the <see cref="YamlConverter"/> object.</returns>
        public YamlSerializerOptions Settings { get; set; }

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
        }
    }
}
