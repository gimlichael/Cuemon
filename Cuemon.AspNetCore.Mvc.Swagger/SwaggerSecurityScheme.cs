using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Allows the definition of a security scheme that can be used by the operations.
    /// </summary>
    public class SwaggerSecurityScheme
    {
        /// <summary>
        /// Gets the type of the security scheme. Default is <see cref="SwaggerSecurityType.Basic"/>.
        /// </summary>
        /// <value>The type of the security scheme.</value>
        public virtual SwaggerSecurityType Type { get; } = SwaggerSecurityType.Basic;

        /// <summary>
        /// Gets or sets the short description for security scheme.
        /// </summary>
        /// <value>The short description for security scheme.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}