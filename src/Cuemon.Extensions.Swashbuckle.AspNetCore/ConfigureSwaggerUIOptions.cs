using System.Linq;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Represents something that configures the <see cref="SwaggerUIOptions"/> type.
    /// Note: These are run before all <see cref="IPostConfigureOptions{TOptions}"/>.
    /// </summary>
    /// <seealso cref="IConfigureOptions{SwaggerUIOptions}" />
    public class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerUIOptions"/> class.
        /// </summary>
        /// <param name="provider">The behavior of a provider that discovers and describes API version information within an application.</param>
        public ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Invoked to configure a <see cref="SwaggerUIOptions"/> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(SwaggerUIOptions options)
        {
            foreach (var groupName in _provider.ApiVersionDescriptions.Select(description => description.GroupName))
            {
                options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
            }
        }
    }
}
