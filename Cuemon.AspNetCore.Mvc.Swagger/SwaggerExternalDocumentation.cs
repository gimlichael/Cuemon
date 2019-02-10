using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    public class SwaggerExternalDocumentation
    {
        /// <summary>
        /// Gets or sets a short description of the target documentation. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>The short description of the target documentation.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL for the target documentation.
        /// </summary>
        /// <value>The URL for the target documentation.</value>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}