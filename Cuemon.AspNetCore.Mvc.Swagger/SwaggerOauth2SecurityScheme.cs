using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Defines OAuth2's common flows (implicit, password, application and access code).
    /// </summary>
    /// <seealso cref="SwaggerSecurityScheme" />
    public class SwaggerOauth2SecurityScheme : SwaggerSecurityScheme
    {
        /// <summary>
        /// Gets the type of the security scheme.
        /// </summary>
        /// <value>The type of the security scheme.</value>
        public override SwaggerSecurityType Type { get; } = SwaggerSecurityType.Oauth2;

        /// <summary>
        /// Gets or sets the flow used by the OAuth2 security scheme.
        /// </summary>
        /// <value>The flow used by the OAuth2 security scheme.</value>
        public SwaggerSecurityFlow Flow { get; set; }

        /// <summary>
        /// Gets or sets the authorization URL to be used for this flow.
        /// </summary>
        /// <value>The authorization URL to be used for this flow.</value>
        public Uri AuthorizationUrl { get; set; }

        /// <summary>
        /// Gets or sets the token URL to be used for this flow.
        /// </summary>
        /// <value>The token URL to be used for this flow.</value>
        public Uri TokenUrl { get; set; }

        /// <summary>
        /// Gets the available scopes for the OAuth2 security scheme.
        /// </summary>
        /// <value>The available scopes for the OAuth2 security scheme.</value>
        public IDictionary<string, string> Scopes { get; } = new Dictionary<string, string>();
    }
}