using Asp.Versioning.ApiExplorer;
using Cuemon.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Represents something that configures the <see cref="SwaggerGenOptions"/> type.
    /// Note: These are run before all <see cref="IPostConfigureOptions{TOptions}"/>.
    /// </summary>
    /// <seealso cref="IConfigureOptions{SwaggerUIOptions}" />
    public class ConfigureSwaggerGenOptions : Configurable<RestfulSwaggerOptions>, IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerGenOptions"/> class.
        /// </summary>
        /// <param name="provider">The behavior of a provider that discovers and describes API version information within an application.</param>
        /// <param name="restfulSwaggerOptions">The options for configuring the <see cref="SwaggerGenOptions"/>.</param>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider, IOptions<RestfulSwaggerOptions> restfulSwaggerOptions) : base(restfulSwaggerOptions.Value)
        {
            _provider = provider;
        }

        /// <summary>
        /// Invoked to configure a <see cref="SwaggerGenOptions"/> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = Options.OpenApiInfo.Title ?? $"API {description.ApiVersion}",
                        Description = Options.OpenApiInfo.Description,
                        Contact = Options.OpenApiInfo.Contact,
                        License = Options.OpenApiInfo.License,
                        TermsOfService = Options.OpenApiInfo.TermsOfService,
                        Version = description.ApiVersion.ToString(),
                        Extensions = Options.OpenApiInfo.Extensions
                    });

                foreach (var xmldoc in Options.XmlDocumentations)
                {
                    options.IncludeXmlComments(() => xmldoc, Options.IncludeControllerXmlComments);
                }
            }
        }
    }
}
