namespace Cuemon.Text
{
    /// <summary>
    /// Configuration options for <see cref="EnumParser"/>.
    /// </summary>
    public class EnumOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EnumOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="IgnoreCase"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EnumOptions()
        {
            IgnoreCase = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore or regard the case of the string being parsed.
        /// </summary>
        /// <value><c>true</c> to ignore the case of the string being parsed; otherwise, <c>false</c>.</value>
        public bool IgnoreCase { get; set; }
    }
}