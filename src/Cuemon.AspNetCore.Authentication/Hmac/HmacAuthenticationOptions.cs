using Cuemon.Security.Cryptography;

namespace Cuemon.AspNetCore.Authentication.Hmac
{
    /// <summary>
    /// Configuration options for <see cref="HmacAuthenticationMiddleware"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="AuthenticationOptions" />
    public sealed class HmacAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HmacAuthenticationOptions"/> class.
        /// </summary>
        public HmacAuthenticationOptions()
        {
            AuthenticationScheme = HmacAuthorizationHeader.Scheme;
            Algorithm = KeyedCryptoAlgorithm.HmacSha256;
        }

        /// <summary>
        /// Gets the name of the authentication scheme. Default is <see cref="HmacAuthorizationHeader.Scheme"/>.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationScheme { get; set; }

        /// <summary>
        /// Gets or sets the algorithm of the HMAC Authentication. Default is <see cref="KeyedCryptoAlgorithm.HmacSha256"/>.
        /// </summary>
        /// <value>The algorithm of the HMAC Authentication.</value>
        public KeyedCryptoAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will perform the authentication from the specified <c>publicKey</c>.
        /// </summary>
        /// <value>The function delegate that will perform the authentication.</value>
        public HmacAuthenticator Authenticator { get; set; }
    }
}