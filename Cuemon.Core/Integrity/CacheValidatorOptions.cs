using Cuemon.Security.Cryptography;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Configuration options for <see cref="CacheValidator"/>.
    /// </summary>
    public class CacheValidatorOptions : ChecksumBuilderOptions
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
        ///         <term><see cref="ChecksumBuilderOptions.Algorithm"/></term>
        ///         <description><see cref="CryptoAlgorithm.Md5"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Method"/></term>
        ///         <description><see cref="ChecksumMethod.Default"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public CacheValidatorOptions()
        {
            Algorithm = CryptoAlgorithm.Md5;
            Method = ChecksumMethod.Default;
        }

        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumMethod"/> indicating how a checksum is generated.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumMethod"/> that indicates how a checksum is generated.</value>
        public ChecksumMethod Method { get; set; }
    }
}