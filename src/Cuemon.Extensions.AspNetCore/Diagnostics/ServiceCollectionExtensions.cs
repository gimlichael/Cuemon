using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
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
        public static IServiceCollection AddServerTiming(this IServiceCollection services)
        {
            return services.AddServerTiming<ServerTiming>();
        }

        /// <summary>
        /// Adds a throttling cache service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddServerTiming<T>(this IServiceCollection services) where T : class, IServerTiming
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddScoped<IServerTiming, T>();
            return services;
        }
    }
}