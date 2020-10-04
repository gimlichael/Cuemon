using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="MemoryThrottlingCache"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddMemoryThrottlingCache(this IServiceCollection services)
        {
            return services.AddThrottlingCache<MemoryThrottlingCache>();
        }

        /// <summary>
        /// Adds a throttling cache service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddThrottlingCache<T>(this IServiceCollection services) where T : class, IThrottlingCache
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddSingleton<IThrottlingCache, T>();
            return services;
        }
    }
}