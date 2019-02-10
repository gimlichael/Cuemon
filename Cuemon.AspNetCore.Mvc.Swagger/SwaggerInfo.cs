using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// The object provides metadata about the API. The metadata can be used by the clients if needed, and can be presented in the Swagger-UI for convenience.
    /// </summary>
    public class SwaggerInfo
    {
        /// <summary>
        /// Gets or sets the title of the application.
        /// </summary>
        /// <value>The title of the application.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a short description of the application. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>The short description of the application.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Terms of Service for the API.
        /// </summary>
        /// <value>The Terms of Service for the API.</value>
        public string TermsOfService { get; set; }

        /// <summary>
        /// Gets or sets the version of the application API (not to be confused with the specification version).
        /// </summary>
        /// <value>The version of the application API (not to be confused with the specification version).</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the exposed API.
        /// </summary>
        /// <value>The contact information for the exposed API.</value>
        public SwaggerContact Contact { get; set; }

        /// <summary>
        /// Gets or sets the license information for the exposed API.
        /// </summary>
        /// <value>The license information for the exposed API.</value>
        public SwaggerLicense License { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}