using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Specifies options that is related to <see cref="KeyedHashAlgorithm"/> operations.
    /// </summary>
    public class KeyedHashOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedHashOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="KeyedHashOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AlgorithmType"/></term>
        ///         <description><see cref="HmacAlgorithmType.SHA1"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public KeyedHashOptions()
        {
            AlgorithmType = HmacAlgorithmType.SHA1;
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the computation.
        /// </summary>
        /// <value>The hash algorithm to use for the computation.</value>
        public HmacAlgorithmType AlgorithmType { get; set; }
    }
}