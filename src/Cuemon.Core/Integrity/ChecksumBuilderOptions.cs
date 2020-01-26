namespace Cuemon.Integrity
{
    /// <summary>
    /// Configuration options for <see cref="ChecksumBuilder"/>.
    /// </summary>
    public class ChecksumBuilderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilderOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ChecksumBuilderOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Algorithm"/></term>
        ///         <description><see cref="CryptoAlgorithm.Md5"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ChecksumBuilderOptions()
        {
            Algorithm = CryptoAlgorithm.Md5;
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the checksum computation.
        /// </summary>
        /// <value>The hash algorithm to use for the checksum computation.</value>
        public CryptoAlgorithm Algorithm { get; set; }
    }
}