using System.Collections.Generic;
using System.Xml.XPath;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Provides programmatic configuration for the <see cref="ServiceCollectionExtensions.AddRestfulSwagger" /> method.
    /// </summary>
    public class RestfulSwaggerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulSwaggerOptions"/> class.
        /// </summary>
        public RestfulSwaggerOptions()
        {
        }

        /// <summary>
        /// Gets the open API information that may be configured.
        /// </summary>
        /// <value>The open API information that may be configured.</value>
        public OpenApiInfoOptions OpenApiInfo { get; } = new();

        /// <summary>
        /// Gets a list of XML documentations to apply to a Swagger document.
        /// </summary>
        /// <value>The list of XML documentations to apply to a Swagger document.</value>
        public IList<XPathDocument> XmlDocumentations { get; } = new List<XPathDocument>();

        /// <summary>
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </summary>
        /// <value><c>true</c> if controller XML comments (i.e. summary) should be used to assign Tag descriptions; otherwise, <c>false</c>.</value>
        public bool IncludeControllerXmlComments { get; set; }
    }
}
