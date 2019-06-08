using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="MultipleTable"/>.
    /// </summary>
    /// <seealso cref="BitMultipleTable"/>
    /// <seealso cref="ByteMultipleTable"/>
    public sealed class MultipleTableOptions : FormattingOptions<CultureInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleTableOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="MultipleTableOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="UseCompoundName"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NumberFormat"/></term>
        ///         <description><c>#,##0.##</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FormattingOptions{T}.FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Prefix"/></term>
        ///         <description><see cref="UnitPrefix.Binary"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public MultipleTableOptions()
        {
            UseCompoundName = false;
            NumberFormat = "#,##0.##";
            FormatProvider = CultureInfo.InvariantCulture;
            Prefix = UnitPrefix.Binary;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use compound naming style (eg. 1 Gigabyte / 0.93 Gibibyte) or symbol naming style (eg. 1 GB / 0.93 GiB).
        /// </summary>
        /// <value><c>true</c> to use compound naming style; otherwise, <c>false</c>.</value>
        public bool UseCompoundName { get; set; }

        /// <summary>
        /// Gets or sets the desired number format when using ToString().
        /// </summary>
        /// <value>The desired number format when using ToString().</value>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets the proffered <see cref="UnitPrefix"/> when calling ToString().
        /// </summary>
        /// <value>The proffered <see cref="UnitPrefix"/> when calling ToString().</value>
        public UnitPrefix Prefix { get; set; }
    }
}
