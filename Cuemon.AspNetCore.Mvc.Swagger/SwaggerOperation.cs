using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Describes a single API operation on a path.
    /// </summary>
    public class SwaggerOperation
    {
        /// <summary>
        /// Gets the list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </summary>
        /// <value>The list of tags for API documentation control.</value>
        public IList<string> Tags { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a short summary of what the operation does. For maximum readability in the swagger-ui, this field SHOULD be less than 120 characters
        /// </summary>
        /// <value>The short summary of what the operation does.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets a verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>The verbose explanation of the operation behavior.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional external documentation for this operation.
        /// </summary>
        /// <value>The additional external documentation for this operation.</value>
        public SwaggerExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets the string used to identify the operation. The id MUST be unique among all operations described in the API. Tools and libraries MAY use the operationId to uniquely identify an operation, therefore, it is recommended to follow common programming naming conventions.
        /// </summary>
        /// <value>The string used to identify the operation.</value>
        public string OperationId { get; set; }

        /// <summary>
        /// Gets the list of MIME types the operation can consume. This overrides the consumes definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        /// <value>The list of MIME types the operation can consume.</value>
        public IList<string> Consumes { get; } = new List<string>();

        /// <summary>
        /// Gets the list of MIME types the operation can produce. This overrides the produces definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        /// <value>The list of MIME types the operation can produce.</value>
        public IList<string> Produces { get; } = new List<string>();

        /// <summary>
        /// Gets the list of parameters that are applicable for this operation. If a parameter is already defined at the Path Item, the new definition will override it, but can never remove it.
        /// </summary>
        /// <value>The list of parameters that are applicable for this operation.</value>
        public IDictionary<string, SwaggerParameter> Parameters { get; } = new Dictionary<string, SwaggerParameter>();

        /// <summary>
        /// Gets the list of possible responses as they are returned from executing this operation.
        /// </summary>
        /// <value>The list of possible responses as they are returned from executing this operation.</value>
        public IDictionary<string, SwaggerResponse> Responses { get; } = new Dictionary<string, SwaggerResponse>();

        /// <summary>
        /// Gets the transfer protocol for the operation. Values MUST be from the list: "http", "https", "ws", "wss". The value overrides the Swagger Object schemes definition.
        /// </summary>
        /// <value>The transfer protocol for the operation.</value>
        public IList<string> Schemes { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether this operation is deprecated. Usage of the declared operation should be refrained. Default value is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> if this operation is deprecated; otherwise, <c>false</c>.</value>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Gets the security schemes are applied for this operation.
        /// </summary>
        /// <value>The security schemes are applied for this operation.</value>
        /// <remarks>The list of values describes alternative security schemes that can be used (that is, there is a logical OR between the security requirements). This definition overrides any declared top-level security. To remove a top-level security declaration, an empty array can be used.</remarks>
        public IDictionary<string, IList<string>> Security { get; } = new Dictionary<string, IList<string>>();

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}