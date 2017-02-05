using Cuemon.AspNetCore.Mvc.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.AspNetCore.Mvc
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
            return AddCacheBusting(services, new AssemblyCacheBusting());
        }

        /// <summary>
        /// Adds a cache-busting service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="cacheBusting">The <see cref="CacheBusting"/> service implementation of your choice.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddCacheBusting<T>(this IServiceCollection services, T cacheBusting = null)
            where T : CacheBusting
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(cacheBusting, nameof(cacheBusting));

            services.AddSingleton(cacheBusting as CacheBusting);
            return services;
        }
    }
}