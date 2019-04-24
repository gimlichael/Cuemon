using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Specifies options that is related to <see cref="HashAlgorithm"/> operations.
    /// </summary>
    public class HashOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="HashOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AlgorithmType"/></term>
        ///         <description><see cref="HashAlgorithmType.SHA256"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public HashOptions()
        {
            AlgorithmType = HashAlgorithmType.SHA256;
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the computation.
        /// </summary>
        /// <value>The hash algorithm to use for the computation.</value>
        public HashAlgorithmType AlgorithmType { get; set; }
    }
}