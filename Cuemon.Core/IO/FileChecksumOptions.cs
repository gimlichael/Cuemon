using Cuemon.Integrity;

namespace Cuemon.IO
{
    public class FileChecksumOptions : FileInfoOptions
    {
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