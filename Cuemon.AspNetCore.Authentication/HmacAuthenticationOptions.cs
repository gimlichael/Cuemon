using System;
using System.Linq;
using System.Text;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
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
            AuthenticationScheme = "HMAC";
            Algorithm = HmacAlgorithmType.SHA1;
            MessageDescriptor = context => "{0}:{1}:{2}:{3}:{4}:{5}".FormatWith(
                context.Request.Method,
                context.Request.GetDisplayUrl(),
                context.Request.Headers[HeaderNames.ContentMD5].FirstOrDefault(),
                context.Request.Headers[HeaderNames.ContentType].FirstOrDefault(),
                context.Request.Headers[HeaderNames.Date].FirstOrDefault(),
                context.Request.Headers[HeaderNames.UserAgent].FirstOrDefault());
            HmacSigner = parameters => HmacUtility.ComputeKeyedHash(parameters.Message, parameters.PrivateKey, o =>
            {
                o.AlgorithmType = parameters.Algorithm;
                o.Encoding = Encoding.UTF8;
            }).Value;
        }

        /// <summary>
        /// Gets the name of the authentication scheme. Default is "HMAC".
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationScheme { get; set; }

        /// <summary>
        /// Gets or sets the algorithm of the HMAC Authentication. Default is <see cref="HmacAlgorithmType.SHA1"/>.
        /// </summary>
        /// <value>The algorithm of the HMAC Authentication.</value>
        public HmacAlgorithmType Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that provides information about the message to be signed.
        /// </summary>
        /// <value>The function delegate that provides information about the message to be signed.</value>
        public Func<HttpContext, string> MessageDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will perform the authentication from the specified <c>publicKey</c>.
        /// </summary>
        /// <value>The function delegate that will perform the authentication.</value>
        public HmacAuthenticator Authenticator { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will sign a message retrieved by <see cref="MessageDescriptor"/>.
        /// </summary>
        /// <value>The function delegate that will sign a message.</value>
        public Func<HmacAuthenticationParameters, byte[]> HmacSigner { get; set; }
    }
}