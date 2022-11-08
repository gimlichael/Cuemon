using System;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Hmac;

namespace Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4
{
    /// <summary>
    /// Provides a representation of a HTTP AWS4-HMAC-SHA256 Authentication header.
    /// </summary>
    /// <seealso cref="HmacAuthorizationHeader" />
    public class Aws4HmacAuthorizationHeader : HmacAuthorizationHeader
    {
        /// <summary>
        /// Creates an instance of <see cref="Aws4HmacAuthorizationHeader"/> from the specified parameters.
        /// </summary>
        /// <param name="authorizationHeader">The raw HTTP authorization header.</param>
        /// <param name="setup">The <see cref="AuthorizationHeaderOptions" /> which may be configured.</param>
        /// <returns>An instance of <see cref="Aws4HmacAuthorizationHeader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="authorizationHeader"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="authorizationHeader"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static HmacAuthorizationHeader Create(string authorizationHeader, Action<AuthorizationHeaderOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(authorizationHeader);
            return Create(Aws4HmacFields.Scheme, authorizationHeader, setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Aws4HmacAuthorizationHeader"/> class.
        /// </summary>
        /// <param name="clientId">The client identifier that is the public key of the signing process.</param>
        /// <param name="credentialScope">The credential scope that defines the remote resource.</param>
        /// <param name="signedHeaders">The headers that will be part of the signing process.</param>
        /// <param name="signature">The signature that represents the integrity of this header.</param>
        public Aws4HmacAuthorizationHeader(string clientId, string credentialScope, string signedHeaders, string signature) : base(clientId, credentialScope, signedHeaders, signature, Aws4HmacFields.Scheme)
        {
        }
    }
}
