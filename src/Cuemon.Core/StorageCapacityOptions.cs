using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="StorageCapacity"/>.
    /// </summary>
    /// <seealso cref="BitStorageCapacity"/>
    /// <seealso cref="ByteStorageCapacity"/>
    public sealed class StorageCapacityOptions : FormattingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageCapacityOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="StorageCapacityOptions"/>.
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
        ///         <term><see cref="FormattingOptions.FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Prefix"/></term>
        ///         <description><see cref="UnitPrefix.Binary"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public StorageCapacityOptions()
        {
            Style = NamingStyle.Symbol;
            NumberFormat = "#,##0.##";
            Prefix = UnitPrefix.Binary;
        }

        /// <summary>
        /// Gets or sets the desired naming style.
        /// </summary>
        /// <value>The desired naming style.</value>
        public NamingStyle Style { get; set; }

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
