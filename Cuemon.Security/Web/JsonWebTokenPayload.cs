using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections;
using Cuemon.Runtime.Serialization;

namespace Cuemon.Security.Web
{
    /// <summary>
    /// Represents the payload information of JSON Web Token that is based on the standard RFC 7519. This class cannot be inherited.
    /// </summary>
    public sealed class JsonWebTokenPayload
    {
        private static readonly string[] ReservedClaims = { "iss", "sub", "aud", "jti", "exp", "nbf", "iat" };

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebTokenPayload"/> class.
        /// </summary>
        public JsonWebTokenPayload() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebTokenPayload"/> class.
        /// </summary>
        /// <param name="claims">The public and/or private claims of the token.</param>
        public JsonWebTokenPayload(params DataPair[] claims)
        {
            Claims = new DataPairCollection();
            if (claims != null && claims.Length > 0)
            {
                foreach (var claim in claims)
                {
                    Claims.Add(claim);
                }
            }
        }

        /// <summary>
        /// Gets the public and/or private claims of the token.
        /// </summary>
        /// <value>The public and/or private claims of the token.</value>
        public DataPairCollection Claims { get; }

        /// <summary>
        /// Gets or sets the issuer (iss) of the token.
        /// </summary>
        /// <value>The issuer (iss) of the token.</value>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the subject (sub) of the token.
        /// </summary>
        /// <value>The subject (sub) of the token.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the audience (aud) of the token.
        /// </summary>
        /// <value>The audience (aud) of the token.</value>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the expiration time (exp) of the token.
        /// </summary>
        /// <value>The expiration time (exp) of the token.</value>
        public DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the not before (nbf) of the token.
        /// </summary>
        /// <value>The not before (nbf) of the token.</value>
        public DateTime? NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the issued at (iat) of the token.
        /// </summary>
        /// <value>The issued at (iat) of the token.</value>
        public DateTime? IssuedAt { get; set; }

        /// <summary>
        /// Gets or sets the JWT unique identifier (jti) of the token.
        /// </summary>
        /// <value>The JWT unique identifier (jti) of the token.</value>
        public string JwtId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <remarks>Calling this method provides the raw payload information of the JSON Web Token.</remarks>
        public override string ToString()
        {
            List<string> payload = new List<string>();
            if (!string.IsNullOrEmpty(Issuer)) { payload.Add("\"iss\": {0}".FormatWith(JsonConverter.ToString(Issuer))); }
            if (!string.IsNullOrEmpty(Subject)) { payload.Add("\"sub\": {0}".FormatWith(JsonConverter.ToString(Subject))); }
            if (!string.IsNullOrEmpty(Audience)) { payload.Add("\"aud\": {0}".FormatWith(JsonConverter.ToString(Audience))); }
            if (!string.IsNullOrEmpty(JwtId)) { payload.Add("\"jti\": {0}".FormatWith(JsonConverter.ToString(JwtId))); }
            if (ExpirationTime.HasValue) { payload.Add("\"exp\": {0}".FormatWith(JsonConverter.ToString(ExpirationTime.Value.ToEpochTime()))); }
            if (NotBefore.HasValue) { payload.Add("\"nbf\": {0}".FormatWith(JsonConverter.ToString(NotBefore.Value.ToEpochTime()))); }
            if (IssuedAt.HasValue) { payload.Add("\"iat\": {0}".FormatWith(JsonConverter.ToString(IssuedAt.Value.ToEpochTime()))); }
            foreach (var claim in Claims)
            {
                if (ReservedClaims.Contains(claim.Name, StringComparer.OrdinalIgnoreCase)) { continue; }
                payload.Add("\"{0}\": {1}".FormatWith(claim.Name, JsonConverter.ToString(claim.Value, claim.Type)));
            }
            return "{{ {0} }}".FormatWith(payload.ToDelimitedString(", "));
        }
    }
}