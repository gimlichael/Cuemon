namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Specifies the algorithm used for generating hash-based message authentication code values.
    /// </summary>
    public enum HmacAlgorithmType
    {
        /// <summary>
        /// The Hash-based Message Authentication Code using (SHA1) algorithm (160 bits).
        /// </summary>
        SHA1 = 0,
        /// <summary>
        /// The Hash-based Message Authentication Code using (SHA256) algorithm (256 bits).
        /// </summary>
        SHA256 = 1,
        /// <summary>
        /// The Hash-based Message Authentication Code using (SHA384) algorithm (384 bits).
        /// </summary>
        SHA384 = 2,
        /// <summary>
        /// The Hash-based Message Authentication Code using (SHA512) algorithm (512 bits).
        /// </summary>
        SHA512 = 3,
    }
}