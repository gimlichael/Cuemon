using Cuemon.Security.Cryptography;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Represents a set of parameters that is needed for creating a keyed-hash message authentication code (HMAC).
    /// </summary>
    public class HmacAuthenticationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HmacAuthenticationParameters"/> class.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use for the computation.</param>
        /// <param name="privateKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="message">The <see cref="string"/> value to compute a hash code for.</param>
        internal HmacAuthenticationParameters(HmacAlgorithmType algorithm, byte[] privateKey, string message)
        {
            Algorithm = algorithm;
            Message = message;
            PrivateKey = privateKey;
        }

        /// <summary>
        /// Gets the algorithm of the HMAC. Default is <see cref="HmacAlgorithmType.SHA1"/>.
        /// </summary>
        /// <value>The algorithm of the HMAC.</value>
        public HmacAlgorithmType Algorithm { get; }

        /// <summary>
        /// Gets the secret key for the hashed encryption.
        /// </summary>
        /// <value>The secret key for the hashed encryption.</value>
        public byte[] PrivateKey { get; }

        /// <summary>
        /// Gets the message to compute a hash code for.
        /// </summary>
        /// <value>The message to compute a hash code for.</value>
        public string Message { get; }
    }
}