using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace Cuemon.Security.Web
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
            Base64UrlEncodedHeader = StringConverter.ToUrlEncodedBase64(Encoding.UTF8.GetBytes(header.ToString()));
            Base64UrlEncodedPayload = StringConverter.ToUrlEncodedBase64(Encoding.UTF8.GetBytes(payload.ToString()));
            Algorithm = header.Algorithm;
            Secret = secret;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebToken" /> class.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="algorithm">The <see cref="JsonWebTokenHashAlgorithm"/> to use when signing the JSON Web Token.</param>
        /// <param name="secret">The secret that is used when signing the JSON Web Token.</param>
        public JsonWebToken(string base64UrlEncodedHeader, string base64UrlEncodedPayload, JsonWebTokenHashAlgorithm algorithm, byte[] secret)
        {
            ValidateJwtParameters(base64UrlEncodedHeader, base64UrlEncodedPayload, algorithm, secret);
            Base64UrlEncodedHeader = base64UrlEncodedHeader;
            Base64UrlEncodedPayload = base64UrlEncodedPayload;
            Algorithm = algorithm;
            Secret = secret;
        }

        /// <summary>
        /// Gets the <see cref="JsonWebTokenHashAlgorithm"/> of this JWT.
        /// </summary>
        /// <value>The <see cref="JsonWebTokenHashAlgorithm"/> of this JWT.</value>
        public JsonWebTokenHashAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the secret (if any) of this JWT.
        /// </summary>
        /// <value>The secret (if any) of this JWT.</value>
        public byte[] Secret { get; }

        /// <summary>
        /// Gets the header information of this JWT.
        /// </summary>
        /// <value>The header information of this JWT.</value>
        public string Base64UrlEncodedHeader { get; }

        /// <summary>
        /// Gets the payload information of this JWT.
        /// </summary>
        /// <value>The payload information of this JWT.</value>
        public string Base64UrlEncodedPayload { get; }

        /// <summary>
        /// Computes the signature of the JSON Web Token.
        /// </summary>
        /// <returns>A <see cref="string"/> that represent the signature of a JSON Web Token.</returns>
        public string ComputeSignature()
        {
            return ComputeSignature(Base64UrlEncodedHeader);
        }

        /// <summary>
        /// Computes the signature of the JSON Web Token.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <returns>A <see cref="string"/> that represent the signature of a JSON Web Token.</returns>
        public string ComputeSignature(string base64UrlEncodedHeader)
        {
            return ComputeSignature(base64UrlEncodedHeader, Base64UrlEncodedPayload);
        }

        /// <summary>
        /// Computes the signature of the JSON Web Token.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <returns>A <see cref="string"/> that represent the signature of a JSON Web Token.</returns>
        public string ComputeSignature(string base64UrlEncodedHeader, string base64UrlEncodedPayload)
        {
            return ComputeSignature(base64UrlEncodedHeader, base64UrlEncodedPayload, Algorithm);
        }

        /// <summary>
        /// Computes the signature of the JSON Web Token.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="algorithm">The <see cref="JsonWebTokenHashAlgorithm"/> to use when signing the JSON Web Token.</param>
        /// <returns>A <see cref="string"/> that represent the signature of a JSON Web Token.</returns>
        public string ComputeSignature(string base64UrlEncodedHeader, string base64UrlEncodedPayload, JsonWebTokenHashAlgorithm algorithm)
        {
            return ComputeSignature(base64UrlEncodedHeader, base64UrlEncodedPayload, algorithm, Secret);
        }

        /// <summary>
        /// Computes the signature of the JSON Web Token.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="algorithm">The <see cref="JsonWebTokenHashAlgorithm"/> to use when signing the JSON Web Token.</param>
        /// <param name="secret">The secret that is used when signing the JSON Web Token.</param>
        /// <returns>A <see cref="string"/> that represent the signature of a JSON Web Token.</returns>
        public string ComputeSignature(string base64UrlEncodedHeader, string base64UrlEncodedPayload, JsonWebTokenHashAlgorithm algorithm, byte[] secret)
        {
            ValidateJwtParameters(base64UrlEncodedHeader, base64UrlEncodedPayload, algorithm, secret);
            return Tokenize(base64UrlEncodedHeader, base64UrlEncodedPayload).ComputeKeyedHash(secret, o =>
            {
                o.AlgorithmType = JsonWebTokenHashAlgorithmConverter.ToHmacAlgorithm(algorithm);
                o.Encoding = Encoding.UTF8;
            }).ToUrlEncodedBase64();
        }

        /// <summary>
        /// Assemble the specified <see cref="Base64UrlEncodedHeader"/> and <see cref="Base64UrlEncodedPayload"/> to represent the first and second part of the JSON Web Token.
        /// </summary>
        /// <returns>A <see cref="string"/> that represent a partial JSON Web Token that does not include the signature information.</returns>
        public string Tokenize()
        {
            return Tokenize(Base64UrlEncodedHeader, Base64UrlEncodedPayload);
        }

        /// <summary>
        /// Assemble the specified <paramref name="base64UrlEncodedHeader"/> and <paramref name="base64UrlEncodedPayload"/> to represent the first and second part of the JSON Web Token.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <returns>A <see cref="string"/> that represent a partial JSON Web Token that does not include the signature information.</returns>
        public string Tokenize(string base64UrlEncodedHeader, string base64UrlEncodedPayload)
        {
            StringBuilder token = new StringBuilder();
            token.Append(base64UrlEncodedHeader);
            token.Append(".");
            token.Append(base64UrlEncodedPayload);
            return token.ToString();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        /// <remarks>Calling this method provides the actual JSON Web Token.</remarks>
        public override string ToString()
        {
            return ToString(Base64UrlEncodedHeader);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public string ToString(string base64UrlEncodedHeader)
        {
            return ToString(base64UrlEncodedHeader, Base64UrlEncodedPayload);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public string ToString(string base64UrlEncodedHeader, string base64UrlEncodedPayload)
        {
            return ToString(base64UrlEncodedHeader, base64UrlEncodedPayload, Algorithm);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="algorithm">The <see cref="JsonWebTokenHashAlgorithm"/> to use when signing the JSON Web Token.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public string ToString(string base64UrlEncodedHeader, string base64UrlEncodedPayload, JsonWebTokenHashAlgorithm algorithm)
        {
            return ToString(base64UrlEncodedHeader, base64UrlEncodedPayload, algorithm, Secret);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="base64UrlEncodedHeader">The header information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="base64UrlEncodedPayload">The payload information of the JSON Web Token; encoded as a Base64 structure usable for transmission on the URL.</param>
        /// <param name="algorithm">The <see cref="JsonWebTokenHashAlgorithm"/> to use when signing the JSON Web Token.</param>
        /// <param name="secret">The secret that is used when signing the JSON Web Token.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public string ToString(string base64UrlEncodedHeader, string base64UrlEncodedPayload, JsonWebTokenHashAlgorithm algorithm, byte[] secret)
        {
            ValidateJwtParameters(base64UrlEncodedHeader, base64UrlEncodedPayload, algorithm, secret);
            string tokenString = Tokenize(base64UrlEncodedHeader, base64UrlEncodedPayload);
            return algorithm == JsonWebTokenHashAlgorithm.None ? tokenString : $"{tokenString}.{ComputeSignature(base64UrlEncodedHeader, base64UrlEncodedPayload, algorithm, secret)}";
        }

        private static void ValidateJwtParameters(string base64UrlEncodedHeader, string base64UrlEncodedPayload, JsonWebTokenHashAlgorithm algorithm, byte[] secret)
        {
            Validator.ThrowIfNullOrEmpty(base64UrlEncodedHeader, nameof(base64UrlEncodedHeader));
            Validator.ThrowIfNullOrEmpty(base64UrlEncodedPayload, nameof(base64UrlEncodedPayload));
            if (algorithm != JsonWebTokenHashAlgorithm.None && secret == null) { throw new ArgumentException("You must specify a secret when JsonWebTokenHashAlgorithm is specified to other than None.", nameof(secret)); }
        }
    }
}