using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a unit test optimized implementation of output logging to the <paramref name="services"/> collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="output">The <see cref="ITestOutputHelper"/> that provides the output for the logging.</param>
        /// <param name="minimumLevel">The <see cref="LogLevel"/> that specifies the minimum level to include for the logging.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null -or-
        /// <paramref name="output"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddXunitTestLogging(this IServiceCollection services, ITestOutputHelper output, LogLevel minimumLevel = LogLevel.Trace)
        {
            Validator.ThrowIfNull(services);
            Validator.ThrowIfNull(output);
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(minimumLevel);
                builder.AddProvider(new XunitTestLoggerProvider(output));
            });
            return services;
        }
    }
}
