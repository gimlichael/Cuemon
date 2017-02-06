using System.Reflection;
using Cuemon.Security.Cryptography;

namespace Cuemon.AspNetCore.Mvc.Configuration
{
    /// <summary>
    /// Specifies options that is related to <see cref="AssemblyCacheBustingOptions"/> operations.
    /// </summary>
    /// <seealso cref="CacheBustingOptions" />
    public class AssemblyCacheBustingOptions : CacheBustingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCacheBustingOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DynamicCacheBustingOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Assembly"/></term>
        ///         <description><see cref="System.Reflection.Assembly.GetEntryAssembly"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AlgorithmType"/></term>
        ///         <description><see cref="HashAlgorithmType.CRC32"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ReadByteForByteChecksum"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AssemblyCacheBustingOptions()
        {
            Assembly = Assembly.GetEntryAssembly();
            AlgorithmType = HashAlgorithmType.CRC32;
            ReadByteForByteChecksum = false;
        }

        /// <summary>
        /// Gets or sets the assembly that should be used as reference for the cache-busting.
        /// </summary>
        /// <value>The assembly that should be used as reference for the cache-busting.</value>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the computation of <see cref="Assembly"/>.
        /// </summary>
        /// <value>The hash algorithm to use for the computation of <see cref="Assembly"/>.</value>
        public HashAlgorithmType AlgorithmType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Assembly"/> will be read byte-for-byte when computing the checksum.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Assembly"/> will be read byte-for-byte when computing the checksum; otherwise, <c>false</c>.</value>
        public bool ReadByteForByteChecksum { get; set; }
    }
}