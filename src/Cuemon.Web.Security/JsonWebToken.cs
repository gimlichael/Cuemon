using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace Cuemon.Web.Security
{
    /// <summary>
    /// Represents a simple implementation of JSON Web Token that is based on the standard RFC 7519 method for communicating claims securely between two parties. This class cannot be inherited.
    /// </summary>
    public sealed class JsonWebToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebToken"/> class that is .
        /// </summary>
        /// <param name="header">The first part of the JSON Web Token; the header information.</param>
        /// <param name="payload">The second part of the JSON Web Token; the payload information.</param>
        public JsonWebToken(JsonWebTokenHeader header, JsonWebTokenPayload payload) : this(header, payload, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebToken"/> class.
        /// </summary>
        /// <param name="header">The first part of the JSON Web Token; the header information.</param>
        /// <param name="payload">The second part of the JSON Web Token; the payload information.</param>
        /// <param name="secret">The optional secret that is used in the last part of the JSON Web Token; the signature.</param>
        public JsonWebToken(JsonWebTokenHeader header, JsonWebTokenPayload payload, byte[] secret)
        {
            Validator.ThrowIfNull(header, nameof(header));
            Validator.ThrowIfNull(payload, nameof(payload));
            if (header.Algorithm != JsonWebTokenHashAlgorithm.None && secret == null) { throw new ArgumentException("You must specify a secret when JsonWebTokenHashAlgorithm is specified to other than None.", nameof(secret)); }
            Header = header;
            Payload = payload;
            Secret = secret;
        }

        /// <summary>
        /// Gets the secret (if any) of this JWT.
        /// </summary>
        /// <value>The secret (if any) of this JWT.</value>
        public byte[] Secret { get; }

        /// <summary>
        /// Gets the header information of this JWT.
        /// </summary>
        /// <value>The header information of this JWT.</value>
        public JsonWebTokenHeader Header { get; }

        /// <summary>
        /// Gets the payload information of this JWT.
        /// </summary>
        /// <value>The payload information of this JWT.</value>
        public JsonWebTokenPayload Payload { get; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <remarks>Calling this method provides the actual JSON Web Token.</remarks>
        public override string ToString()
        {
            string base64UrlEncodedHeader = Encoding.UTF8.GetBytes(Header.ToString()).ToUrlEncodedBase64();
            string base64UrlEncodedPayload = Encoding.UTF8.GetBytes(Payload.ToString()).ToUrlEncodedBase64();
            StringBuilder token = new StringBuilder();
            token.Append(base64UrlEncodedHeader);
            token.Append(".");
            token.Append(base64UrlEncodedPayload);
            var tokenString = token.ToString();
            return Header.Algorithm == JsonWebTokenHashAlgorithm.SHA256 ? "{0}.{1}".FormatWith(tokenString, tokenString.ComputeKeyedHash(Secret, HmacAlgorithmType.SHA256, Encoding.UTF8).ToUrlEncodedBase64()) : tokenString;
        }
    }
}