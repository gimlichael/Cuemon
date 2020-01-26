using Cuemon.AspNetCore.Mvc.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.AspNetCore.Mvc.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a cache-busting service to the specified <see cref="IServiceCollection"/> based on a default instance of <see cref="AssemblyCacheBusting"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddCacheBusting(this IServiceCollection services)
        {
            return services.AddCacheBusting<AssemblyCacheBusting>();
        }

        /// <summary>
        /// Adds a cache-busting service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddCacheBusting<T>(this IServiceCollection services) where T : CacheBusting
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddSingleton<ICacheBusting, T>();
            return services;
        }
    }
}