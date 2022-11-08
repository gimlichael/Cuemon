using System;
using Cuemon.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="SwaggerGenOptions"/> class.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Adds an <see cref="UserAgentDocumentFilter"/> to the <see cref="SwaggerGenOptions.DocumentFilterDescriptors"/>.
        /// </summary>
        /// <param name="options">The <see cref="SwaggerGenOptions"/> to extend.</param>
        /// <param name="setup">The <see cref="UserAgentDocumentOptions"/> that may be configured.</param>
        /// <returns>A reference to <paramref name="options" /> so that additional calls can be chained.</returns>
        public static SwaggerGenOptions AddUserAgent(this SwaggerGenOptions options, Action<UserAgentDocumentOptions> setup = null)
        {
            Validator.ThrowIfNull(options);
            options.DocumentFilter<UserAgentDocumentFilter>(Patterns.Configure(setup));
            return options;
        }

        /// <summary>
        /// Adds support for first line of defense using security based HTTP X-Api-Key header.
        /// </summary>
        /// <param name="options">The <see cref="SwaggerGenOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options" /> so that additional calls can be chained.</returns>
        public static SwaggerGenOptions AddXApiKeySecurity(this SwaggerGenOptions options)
        {
            Validator.ThrowIfNull(options);
            options.AddSecurityDefinition(HttpHeaderNames.XApiKey, new OpenApiSecurityScheme
            {
                Description = $"Protects an API by adding a first line of defense {HttpHeaderNames.XApiKey} header.",
                In = ParameterLocation.Header,
                Name = HttpHeaderNames.XApiKey,
                Type = SecuritySchemeType.ApiKey
            });

            var apiKeyRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = HttpHeaderNames.XApiKey
                            },
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                }
            };

            options.AddSecurityRequirement(apiKeyRequirement);
            return options;
        }

        /// <summary>
        /// Adds support for AuthN/AuthZ using the Bearer security scheme in JWT format.
        /// </summary>
        /// <param name="options">The <see cref="SwaggerGenOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options" /> so that additional calls can be chained.</returns>
        public static SwaggerGenOptions AddJwtBearerSecurity(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(HttpAuthenticationSchemes.Bearer, new OpenApiSecurityScheme
            {
                Description = $"Protects an API by adding an {HttpHeaderNames.Authorization} header using the {HttpAuthenticationSchemes.Bearer} scheme in JWT format.",
                Name = HttpHeaderNames.Authorization,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = HttpAuthenticationSchemes.Bearer.ToLowerInvariant()
            });

            var jwtRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = HttpAuthenticationSchemes.Bearer
                        },
                        In = ParameterLocation.Header
                    },
                    new string[] {}
                }
            };

            options.AddSecurityRequirement(jwtRequirement);
            return options;
        }
    }
}
