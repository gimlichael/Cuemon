using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Provides a User-Agent field to the generated <see cref="OpenApiDocument"/>.
    /// </summary>
    /// <seealso cref="DocumentFilter{T}" />
    /// <seealso cref="UserAgentDocumentOptions"/>
    /// <remarks>Inspiration for this class was borrowed from: https://github.com/tgstation/tgstation-server/blob/dev/src/Tgstation.Server.Host/Core/SwaggerConfiguration.cs</remarks>
    public class UserAgentDocumentFilter : DocumentFilter<UserAgentDocumentOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentDocumentFilter"/> class.
        /// </summary>
        /// <param name="options">The configured options of this instance.</param>
        public UserAgentDocumentFilter(UserAgentDocumentOptions options) : base(options)
        {
        }

        /// <summary>
        /// Applies post-processing to the <paramref name="swaggerDoc" />.
        /// </summary>
        /// <param name="swaggerDoc">The <see cref="OpenApiDocument" /> to modify.</param>
        /// <param name="context">The <see cref="DocumentFilterContext" /> that provides additional context.</param>
        /// <remarks>Once an <seealso cref="OpenApiDocument" /> has been generated you have full control to modify the document however you see fit.</remarks>
        public override void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Components.Parameters.Add(HeaderNames.UserAgent, new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = HeaderNames.UserAgent,
                Description = Options.Description,
                Style = ParameterStyle.Simple,
                Example = new OpenApiString(Options.Example),
                Required = Options.Required
            });
            
            foreach (var operation in swaggerDoc.Paths.SelectMany(path => path.Value.Operations.Select(x => x.Value)))
            {
                operation.Parameters.Insert(0, new OpenApiParameter
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Parameter,
                        Id = HeaderNames.UserAgent
                    }
                });
            }
        }
    }
}
