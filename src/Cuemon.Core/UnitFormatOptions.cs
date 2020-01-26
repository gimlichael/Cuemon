using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="BitUnit" /> and <see cref="ByteUnit" />.
    /// Implements the <see cref="FormattingOptions{CultureInfo}" />
    /// </summary>
    /// <seealso cref="FormattingOptions{CultureInfo}" />
    /// <seealso cref="IPrefixMultiple"/>
    /// <seealso cref="BinaryPrefix"/>
    /// <seealso cref="DecimalPrefix"/>
    /// <seealso cref="NamingStyle"/>
    public sealed class UnitFormatOptions : FormattingOptions<CultureInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitFormatOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="UnitFormatOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Style"/></term>
        ///         <description><see cref="NamingStyle.Symbol"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NumberFormat"/></term>
        ///         <description><c>#,##0.##</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FormattingOptions{T}.FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public UnitFormatOptions()
        {
            Style = NamingStyle.Symbol;
            NumberFormat = "#,##0.##";
            FormatProvider = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets or sets the desired naming style.
        /// </summary>
        /// <value>The desired naming style.</value>
        public NamingStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the desired number format.
        /// </summary>
        /// <value>The desired number format.</value>
        public string NumberFormat { get; set; }
    }
}