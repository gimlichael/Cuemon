using System;
using System.Security.Claims;
using Cuemon.Security.Cryptography;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Configuration options for <see cref="DigestAccessAuthenticationMiddleware"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="AuthenticationOptions" />
    public sealed class DigestAccessAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAccessAuthenticationOptions"/> class.
        /// </summary>
        public DigestAccessAuthenticationOptions()
        {
            Algorithm = HashAlgorithmType.MD5;
            OpaqueGenerator = DigestAuthenticationUtility.DefaultOpaqueGenerator;
            NonceExpiredParser = DigestAuthenticationUtility.DefaultNonceExpiredParser;
            NonceGenerator = DigestAuthenticationUtility.DefaultNonceGenerator;
            NonceSecret = () => DigestAuthenticationUtility.DefaultPrivateKey;
        }

        /// <summary>
        /// Gets or sets the function delegate for credentials validation.
        /// </summary>
        /// <value>The function delegate for credentials validation.</value>
        public TesterFunc<string, string, ClaimsPrincipal> CredentialsValidator { get; set; }

        /// <summary>
        /// Gets or sets the algorithm of the HTTP Digest Access Authentication. Default is <see cref="HashAlgorithmType.MD5"/>.
        /// </summary>
        /// <value>The algorithm of the HTTP Digest Access Authentication.</value>
        public HashAlgorithmType Algorithm { get; set; }


        /// <summary>
        /// Gets or sets the function delegate for generating opaque string values.
        /// </summary>
        /// <value>The function delegate for generating opaque string values.</value>
        public Func<string> OpaqueGenerator { get; set; }

        /// <summary>
        /// Gets or sets the function delegate for retrieving the cryptographic secret used in nonce string values.
        /// </summary>
        /// <value>The function delegate for retrieving the cryptographic secret used in nonce string values.</value>
        public Func<byte[]> NonceSecret { get; set; }

        /// <summary>
        /// Gets or sets the function delegate for generating nonce string values.
        /// </summary>
        /// <value>The function delegate for generating nonce string values.</value>
        public Func<DateTime, string, byte[], string> NonceGenerator { get; set; }

        /// <summary>
        /// Gets or sets the function delegate for parsing nonce string values for expiration.
        /// </summary>
        /// <value>The function delegate for parsing nonce string values for expiration.</value>
        public Func<string, TimeSpan, bool> NonceExpiredParser { get; set; }
    }
}