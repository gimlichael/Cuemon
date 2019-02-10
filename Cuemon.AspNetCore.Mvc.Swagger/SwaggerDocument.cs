using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Represents the root document object for the API specification.
    /// </summary>
    public class SwaggerDocument
    {
        /// <summary>
        /// Gets the Swagger Specification version being used.
        /// </summary>
        /// <value>The Swagger Specification version being used.</value>
        public string Swagger { get; } = "2.0";

        /// <summary>
        /// Gets or sets the metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        /// <value>The metadata about the API.</value>
        public SwaggerInfo Info { get; set; }

        /// <summary>
        /// Gets or sets the host (name or ip) serving the API. This MUST be the host only and does not include the scheme nor sub-paths. It MAY include a port. If the host is not included, the host serving the documentation is to be used (including the port).
        /// </summary>
        /// <value>The host (name or ip) serving the API.</value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the base path on which the API is served, which is relative to the host. If it is not included, the API is served directly under the host. The value MUST start with a leading slash (/).
        /// </summary>
        /// <value>The base path on which the API is served, which is relative to the host.</value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets the transfer protocol of the API. Values MUST be from the list: "http", "https", "ws", "wss". If the schemes is not included, the default scheme to be used is the one used to access the Swagger definition itself.
        /// </summary>
        /// <value>The transfer protocol of the API.</value>
        public IList<string> Schemes { get; } = new List<string>();

        /// <summary>
        /// Gets a list of MIME types the APIs can consume. This is global to all APIs but can be overridden on specific API calls.
        /// </summary>
        /// <value>The list of MIME types the APIs can consume.</value>
        public IList<string> Consumes { get; } = new List<string>();

        /// <summary>
        /// Gets a list of MIME types the APIs can produce. This is global to all APIs but can be overridden on specific API calls.
        /// </summary>
        /// <value>The list of MIME types the APIs can produce.</value>
        public IList<string> Produces { get; } = new List<string>();

        /// <summary>
        /// Gets the available paths and operations for the API.
        /// </summary>
        /// <value>The paths and operations for the API.</value>
        public IDictionary<SwaggerPath, SwaggerPathItem> Paths { get; } = new Dictionary<SwaggerPath, SwaggerPathItem>();

        /// <summary>
        /// Gets the object to hold data types produced and consumed by operations.
        /// </summary>
        /// <value>The object to hold data types produced and consumed by operations.</value>
        public IDictionary<string, SwaggerSchema> Definitions { get; } = new Dictionary<string, SwaggerSchema>();

        /// <summary>
        /// Gets the object to hold parameters to be reused across operations. Parameter definitions can be referenced to the ones defined here.
        /// </summary>
        /// <value>The object to hold parameters to be reused across operations.</value>
        public IDictionary<string, SwaggerParameter> Parameters { get; } = new Dictionary<string, SwaggerParameter>();

        /// <summary>
        /// Gets the object to hold responses to be reused across operations. Response definitions can be referenced to the ones defined here.
        /// </summary>
        /// <value>The object to hold responses to be reused across operations.</value>
        public IDictionary<string, SwaggerResponse> Responses { get; } = new Dictionary<string, SwaggerResponse>();

        /// <summary>
        /// Gets the security scheme definitions that can be used across the specification.
        /// </summary>
        /// <value>The security scheme definitions that can be used across the specification.</value>
        /// <remarks>A declaration of the security schemes available to be used in the specification. This does not enforce the security schemes on the operations and only serves to provide the relevant details for each scheme.</remarks>
        public IDictionary<string, SwaggerSecurityScheme> SecurityDefinitions { get; } = new Dictionary<string, SwaggerSecurityScheme>();

        /// <summary>
        /// Gets a declaration of which security schemes are applied for the API as a whole. The list of values describes alternative security schemes that can be used (that is, there is a logical OR between the security requirements). Individual operations can override this definition.
        /// </summary>
        /// <value>The declaration of which security schemes are applied for the API as a whole.</value>
        /// <remarks>The object can have multiple security schemes declared in it which are all required (that is, there is a logical AND between the schemes). The name used for each property MUST correspond to a security scheme declared in the <see cref="SecurityDefinitions"/>.</remarks>
        public IDictionary<string, IList<string>> Security { get; } = new Dictionary<string, IList<string>>();

        /// <summary>
        /// Gets a list of tags used by the specification with additional metadata. The order of the tags can be used to reflect on their order by the parsing tools. Not all tags that are used by the Operation Object must be declared.
        /// </summary>
        /// <value>The list of tags used by the specification with additional metadata.</value>
        public IList<SwaggerTag> Tags { get; } = new List<SwaggerTag>();

        /// <summary>
        /// Gets or sets the additional external documentation.
        /// </summary>
        /// <value>The additional external documentation.</value>
        public SwaggerExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}