using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Describes the operations available on a single path. A Path Item may be empty, due to ACL constraints. The path itself is still exposed to the documentation viewer but they will not know which operations and parameters are available.
    /// </summary>
    public class SwaggerPathItem
    {
        /// <summary>
        /// Gets or sets the value to reference another value in a JSON document.
        /// </summary>
        /// <value>The value to reference another value in a JSON document.</value>
        public string Ref { get; set; }

        /// <summary>
        /// Gets or sets the definition of a GET operation on this path.
        /// </summary>
        /// <value>The definition of a GET operation on this path.</value>
        public SwaggerOperation Get { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PUT operation on this path.
        /// </summary>
        /// <value>The definition of a PUT operation on this path.</value>
        public SwaggerOperation Put { get; set; }

        /// <summary>
        /// Gets or sets the definition of a POST operation on this path.
        /// </summary>
        /// <value>The definition of a POST operation on this path.</value>
        public SwaggerOperation Post { get; set; }

        /// <summary>
        /// Gets or sets the definition of a DELETE operation on this path.
        /// </summary>
        /// <value>The definition of a DELETE operation on this path.</value>
        public SwaggerOperation Delete { get; set; }

        /// <summary>
        /// Gets or sets the definition of a OPTIONS operation on this path.
        /// </summary>
        /// <value>The definition of a OPTIONS operation on this path.</value>
        public SwaggerOperation Options { get; set; }

        /// <summary>
        /// Gets or sets the definition of a HEAD operation on this path.
        /// </summary>
        /// <value>The definition of a HEAD operation on this path.</value>
        public SwaggerOperation Head { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PATCH operation on this path.
        /// </summary>
        /// <value>The definition of a PATCH operation on this path.</value>
        public SwaggerOperation Patch { get; set; }

        /// <summary>
        /// Gets the list of parameters that are applicable for all the operations described under this path. These parameters can be overridden at the operation level, but cannot be removed there.
        /// </summary>
        /// <value>The list of parameters that are applicable for all the operations described under this path.</value>
        public IDictionary<string, SwaggerParameter> Parameters { get; } = new Dictionary<string, SwaggerParameter>();

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}