using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    public class SwaggerResponse
    {
        /// <summary>
        /// Gets or sets a short description of the response. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>The short description of the response.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the definition of the response structure.
        /// </summary>
        /// <value>The definition of the response structure.</value>
        public SwaggerSchema Schema { get; set; }

        /// <summary>
        /// Gets the headers that are sent with the response.
        /// </summary>
        /// <value>The headers that are sent with the response.</value>
        public IDictionary<string, SwaggerHeader> Headers { get; } = new Dictionary<string, SwaggerHeader>();

        /// <summary>
        /// Gets the examples for operation responses.
        /// </summary>
        /// <value>The examples for operation responses.</value>
        public IDictionary<SwaggerMimeType, object> Examples { get; } = new Dictionary<SwaggerMimeType, object>();

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}