using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Represents a proxy for configuring an Open API Info Object that provides metadata about an Open API endpoint.
    /// </summary>
    public class OpenApiInfoOptions
    {
        /// <summary>
        /// The title of the application.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the application.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A URL to the Terms of Service for the API.
        /// </summary>
        public Uri TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        public OpenApiContact Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        public OpenApiLicense License { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
