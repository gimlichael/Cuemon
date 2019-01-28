using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.AspNetCore.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a memory-based throttling cache service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddMemoryThrottling(this IServiceCollection services)
        {
            services.AddSingleton<IThrottlingCache, MemoryThrottlingCache>();
            return services;
        }
    }
}