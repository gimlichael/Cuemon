using Cuemon.Security.Cryptography;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Specifies options that is related to <see cref="CacheValidator"/> operations.
    /// </summary>
    public class CacheValidatorOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidatorOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="CacheValidatorOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AlgorithmType"/></term>
        ///         <description><see cref="HashAlgorithmType.MD5"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Method"/></term>
        ///         <description><see cref="ChecksumMethod.Default"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public CacheValidatorOptions()
        {
            AlgorithmType = HashAlgorithmType.MD5;
            Method = ChecksumMethod.Default;
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the checksum computation.
        /// </summary>
        /// <value>The hash algorithm to use for the checksum computation.</value>
        public HashAlgorithmType AlgorithmType { get; set; }

        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumMethod"/> indicating how a checksum is generated.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumMethod"/> that indicates how a checksum is generated.</value>
        public ChecksumMethod Method { get; set; }
    }
}