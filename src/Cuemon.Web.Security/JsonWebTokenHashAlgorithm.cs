namespace Cuemon.Web.Security
{
    /// <summary>
    /// Specifies the algorithm used for generating JWT hash values.
    /// </summary>
    public enum JsonWebTokenHashAlgorithm
    {
        /// <summary>
        /// Defines no hashing algorithm.
        /// </summary>
        None,
        /// <summary>
        /// Defines the Secure Hashing Algorithm (SHA256) algorithm (256 bits).
        /// </summary>
        SHA256
    }
}