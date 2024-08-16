using System;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceProvider"/> interface.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets a required scoped service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service from.</param>
        /// <returns>The required service of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">There is no service of type <typeparamref name="T"/>.</exception>
        public static T GetRequiredScopedService<T>(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
