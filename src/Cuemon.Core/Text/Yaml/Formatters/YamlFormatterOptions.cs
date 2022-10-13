using System;
using System.Collections.Generic;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Text.Yaml.Formatters
{
    /// <summary>
    /// Configuration options for <see cref="YamlFormatter"/>.
    /// </summary>
    public sealed class YamlFormatterOptions : EncodingOptions
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
        public YamlSerializerOptions Settings { get; }
    }
}
