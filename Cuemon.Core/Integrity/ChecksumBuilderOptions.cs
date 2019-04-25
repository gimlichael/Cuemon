using Cuemon.Security.Cryptography;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Specifies options that is related to <see cref="ChecksumBuilder"/> operations.
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
        ///         <term><see cref="AlgorithmType"/></term>
        ///         <description><see cref="HashAlgorithmType.MD5"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ChecksumBuilderOptions()
        {
            AlgorithmType = HashAlgorithmType.MD5;
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the checksum computation.
        /// </summary>
        /// <value>The hash algorithm to use for the checksum computation.</value>
        public HashAlgorithmType AlgorithmType { get; set; }
    }
}