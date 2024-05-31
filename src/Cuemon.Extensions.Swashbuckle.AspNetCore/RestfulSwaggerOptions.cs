using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.XPath;
using Cuemon.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="RestfulSwaggerOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="XmlDocumentations"/></term>
        ///         <description><c>new List&lt;XPathDocument&gt;();</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="OpenApiInfo"/></term>
        ///         <description><c>new();</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IncludeControllerXmlComments"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
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
        /// Gets or sets the <see cref="SwaggerGenOptions"/> used to configure Swagger.
        /// </summary>
        /// <value>The <see cref="SwaggerGenOptions"/> used to configure Swagger.</value>
        public SwaggerGenOptions Settings { get; set; } = new();

        /// <summary>
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </summary>
        /// <value><c>true</c> if controller XML comments (i.e. summary) should be used to assign Tag descriptions; otherwise, <c>false</c>.</value>
        public bool IncludeControllerXmlComments { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will resolve a <see cref="JsonSerializerOptions"/> instance.
        /// </summary>
        /// <value>The function delegate that will resolve a <see cref="JsonSerializerOptions"/> instance.</value>
        /// <remarks>The Swagger team decided to opt-in full hearted to System.Text.Json using the MVC variant of JsonOption to retrieve an instance of <see cref="JsonSerializerOptions"/>. Weird design choice IMO; leave it to the implementor to decide which configured instance of <see cref="JsonSerializerOptions"/> to use and avoid redundant configuration that may or may not be a mismatch. More info: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1269</remarks>
        public Func<IServiceProvider, JsonSerializerOptions> JsonSerializerOptionsFactory { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="OpenApiInfo"/> cannot be null - or -
        /// <see cref="XmlDocumentations"/> cannot be null - or -
        /// <see cref="Settings"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(OpenApiInfo == null);
            Validator.ThrowIfInvalidState(XmlDocumentations == null);
            Validator.ThrowIfInvalidState(Settings == null);
        }
    }
}
