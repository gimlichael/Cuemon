using Cuemon.Integrity;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="Convertible"/>.
    /// </summary>
    /// <seealso cref="EndianOptions"/>
    public class ConvertibleOptions : EndianOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertibleOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ConvertibleOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Converters"/></term>
        ///         <description><see cref="ConvertibleConverterCollection"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ConvertibleOptions()
        {
            Converters = new ConvertibleConverterCollection();
        }

        /// <summary>
        /// Gets the converters associated with this instance.
        /// </summary>
        /// <value>The converters associated with this instance.</value>
        public ConvertibleConverterCollection Converters { get; }
    }
}