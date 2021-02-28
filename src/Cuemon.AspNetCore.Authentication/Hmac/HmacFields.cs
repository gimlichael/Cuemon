namespace Cuemon.AspNetCore.Authentication.Hmac
{
    /// <summary>
    /// A collection of constants for <see cref="HmacAuthorizationHeaderBuilder"/>.
    /// </summary>
    public static class HmacFields
    {
        /// <summary>
        /// The HTTP request method.
        /// </summary>
        public const string HttpMethod = "httpRequestMethod";

        /// <summary>
        /// The canonical URI that is the URI-encoded version of the absolute path component of an URI.
        /// </summary>
        public const string UriPath = "canonicalUri";

        /// <summary>
        /// The canonical query string.
        /// </summary>
        public const string UriQuery = "canonicalQueryString";

        /// <summary>
        /// The canonical headers.
        /// </summary>
        public const string HttpHeaders = "canonicalHeaders";

        /// <summary>
        /// The headers that must be part of the signing process.
        /// </summary>
        public const string SignedHeaders = "signedHeaders";

        /// <summary>
        /// The request payload.
        /// </summary>
        public const string Payload = "requestPayload";

        /// <summary>
        /// The server date time expressed in ISO 8601 format.
        /// </summary>
        public const string ServerDateTime = "serverDateTime";

        /// <summary>
        /// The public key of the signing process.
        /// </summary>
        public const string ClientId = "clientId";

        /// <summary>
        /// The private key of the signing process.
        /// </summary>
        public const string ClientSecret = "clientSecret";

        /// <summary>
        /// The credential scope that defines the remote resource.
        /// </summary>
        public const string CredentialScope = "credentialScope";
    }
}