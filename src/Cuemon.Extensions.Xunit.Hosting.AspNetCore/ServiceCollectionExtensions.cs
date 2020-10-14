using Cuemon.Extensions.DependencyInjection;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a unit test optimized implementation for the <see cref="IHttpContextAccessor"/> service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to extend.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection AddHttpContextAccessor(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.TryAdd<IHttpContextAccessor, FakeHttpContextAccessor>(provider =>
            {
                var contextAccessor = new FakeHttpContextAccessor
                {
                    HttpContext =
                    {
                        RequestServices = provider
                    }
                };
                return contextAccessor;
            }, lifetime);
            return services;
        }
    }
}