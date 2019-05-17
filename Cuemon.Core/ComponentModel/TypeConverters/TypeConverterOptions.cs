using System;
using System.ComponentModel;
using System.Globalization;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Configuration options for <see cref="StringTypeParser"/> and <see cref="ObjectTypeConverter"/>.
    /// </summary>
    public class TypeConverterOptions : FormattingOptions<IFormatProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeConverterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="TypeConverterOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="DescriptorContext"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TypeConverterOptions()
        {
            FormatProvider = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets or sets the type specific format context.
        /// </summary>
        /// <value>The type specific format context.</value>
        public ITypeDescriptorContext DescriptorContext { get; set; }

        /// <summary>
        /// Gets or sets the culture specific formatting information.
        /// </summary>
        /// <value>The culture specific formatting information.</value>
        public sealed override IFormatProvider FormatProvider { get; set; }
    }
}