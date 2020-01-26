using System.IO;
using Cuemon.Integrity;

namespace Cuemon.IO
{
    /// <summary>
    /// Configuration options for <see cref="FileInfo"/>.
    /// </summary>
    public class FileChecksumOptions : FileInfoOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileChecksumOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FileChecksumOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Algorithm"/></term>
        ///         <description><see cref="CryptoAlgorithm.Md5"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Method"/></term>
        ///         <description><see cref="ChecksumMethod.Default"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FileChecksumOptions()
        {
            Algorithm = CryptoAlgorithm.Md5;
            Method = ChecksumMethod.Default;
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use for the checksum computation.
        /// </summary>
        /// <value>The hash algorithm to use for the checksum computation.</value>
        public CryptoAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumMethod"/> indicating how a checksum is generated.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumMethod"/> that indicates how a checksum is generated.</value>
        public ChecksumMethod Method { get; set; }
    }
}