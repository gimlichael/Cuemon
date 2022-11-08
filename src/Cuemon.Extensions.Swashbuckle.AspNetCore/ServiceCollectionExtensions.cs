using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds complementary configurations for both <see cref="SwaggerGenOptions"/> and <see cref="SwaggerUIOptions"/> - optimized for RESTful APIs.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="RestfulSwaggerOptions"/> that may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRestfulSwagger(this IServiceCollection services, Action<RestfulSwaggerOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            services.AddSwaggerGen(Patterns.ConfigureRevert(options.Settings));
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
            services.Configure(setup ?? Patterns.ConfigureRevert(options));
            return services;
        }
    }
}
