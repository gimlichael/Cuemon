using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Cuemon.Configuration;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Provides programmatic configuration for the <see cref="ServiceCollectionExtensions.AddRestfulSwagger" /> method.
    /// </summary>
    public class RestfulSwaggerOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulSwaggerOptions"/> class.
        /// </summary>
        public RestfulSwaggerOptions()
        {
        }

        /// <summary>
        /// Gets or sets the open API information that may be configured.
        /// </summary>
        /// <value>The open API information that may be configured.</value>
        public OpenApiInfoOptions OpenApiInfo { get; set; } = new();

        /// <summary>
        /// Gets or sets a list of XML documentations to apply to a Swagger document.
        /// </summary>
        /// <value>The list of XML documentations to apply to a Swagger document.</value>
        public IList<XPathDocument> XmlDocumentations { get; set; } = new List<XPathDocument>();

        /// <summary>
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </summary>
        /// <value><c>true</c> if controller XML comments (i.e. summary) should be used to assign Tag descriptions; otherwise, <c>false</c>.</value>
        public bool IncludeControllerXmlComments { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <see cref="OpenApiInfo"/> cannot be null - or -
        /// <see cref="XmlDocumentations"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfNull(OpenApiInfo, nameof(OpenApiInfo), $"{nameof(OpenApiInfo)} cannot be null.");
            Validator.ThrowIfNull(XmlDocumentations, nameof(XmlDocumentations), $"{nameof(XmlDocumentations)} cannot be null.");
        }
    }
}
