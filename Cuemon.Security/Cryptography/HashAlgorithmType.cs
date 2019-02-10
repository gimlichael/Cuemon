namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Specifies the algorithm used for generating hash values.
    /// </summary>
    public enum HashAlgorithmType
    {
        /// <summary>
        /// The Message Digest 5 (MD5) algorithm (128 bits).
        /// </summary>
        MD5 = 0,
        /// <summary>
        /// The Secure Hashing Algorithm (SHA1) algorithm (160 bits).
        /// </summary>
        SHA1 = 1,
        /// <summary>
        /// The Secure Hashing Algorithm (SHA256) algorithm (256 bits).
        /// </summary>
        SHA256 = 2,
        /// <summary>
        /// The Secure Hashing Algorithm (SHA384) algorithm (384 bits).
        /// </summary>
        SHA384 = 3,
        /// <summary>
        /// The Secure Hashing Algorithm (SHA512) algorithm (512 bits).
        /// </summary>
        SHA512 = 4,
        /// <summary>
        /// The Cyclic Redundancy Check 32 (CRC32) algorithm (32 bits), reversed for broader compatibility (0xEDB88320).
        /// </summary>
        CRC32 = 5
    }
}