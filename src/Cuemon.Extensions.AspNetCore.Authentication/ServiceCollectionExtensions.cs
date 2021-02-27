using Cuemon.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Authentication
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="MemoryNonceTracker"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddDigestAccessAuthenticationNonceTracker(this IServiceCollection services)
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddSingleton<INonceTracker, MemoryNonceTracker>();
            return services;
        }
    }
}