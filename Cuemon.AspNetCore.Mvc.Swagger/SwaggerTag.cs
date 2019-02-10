using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Allows adding meta data to a single tag that is used by the Operation Object. It is not mandatory to have a Tag Object per tag used there.
    /// </summary>
    public class SwaggerTag
    {
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>The name of the tag.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a short description for the tag. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>The short description for the tag.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional external documentation for this tag.
        /// </summary>
        /// <value>The additional external documentation for this tag.</value>
        public SwaggerExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}