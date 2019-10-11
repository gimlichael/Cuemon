using System;
using Cuemon.Integrity;

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
            Algorithm = CryptoAlgorithm.Md5;
            OpaqueGenerator = DigestAuthenticationUtility.DefaultOpaqueGenerator;
            NonceExpiredParser = DigestAuthenticationUtility.DefaultNonceExpiredParser;
            NonceGenerator = DigestAuthenticationUtility.DefaultNonceGenerator;
            NonceSecret = () => DigestAuthenticationUtility.DefaultPrivateKey;
            DigestAccessSigner = parameters =>
            {
                var ha1 = DigestAuthenticationUtility.ComputeHash1(parameters.Credentials, parameters.Password, parameters.Algorithm);
                var ha2 = DigestAuthenticationUtility.ComputeHash2(parameters.Credentials, parameters.HttpMethod, parameters.Algorithm);
                return DigestAuthenticationUtility.ComputeResponse(parameters.Credentials, ha1, ha2, parameters.Algorithm);
            };
        }

        /// <summary>
        /// Gets or sets the function delegate that will perform the authentication from the specified <c>username</c>.
        /// </summary>
        /// <value>The function delegate that will perform the authentication.</value>
        public DigestAccessAuthenticator Authenticator { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will sign a message retrieved from a HTTP request.
        /// </summary>
        /// <value>The function delegate that will sign a message.</value>
        public Func<DigestAccessAuthenticationParameters, byte[]> DigestAccessSigner { get; set; }

        /// <summary>
        /// Gets or sets the algorithm of the HTTP Digest Access Authentication. Default is <see cref="CryptoAlgorithm.Md5"/>.
        /// </summary>
        /// <value>The algorithm of the HTTP Digest Access Authentication.</value>
        public CryptoAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets the realm that defines the protection space.
        /// </summary>
        /// <value>The realm that defines the protection space.</value>
        public string Realm { get; set; }

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