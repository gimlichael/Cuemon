using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class SwaggerLicense
    {
        /// <summary>
        /// Gets or sets the license name used for the API.
        /// </summary>
        /// <value>The license name used for the API.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        /// <value>The URL to the license used for the API. MUST be in the format of a URL.</value>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}