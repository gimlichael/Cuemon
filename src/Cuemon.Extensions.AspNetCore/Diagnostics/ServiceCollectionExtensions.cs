using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="ServerTiming"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddServerTiming(this IServiceCollection services)
        {
            return services.AddServerTiming<ServerTiming>();
        }

        /// <summary>
        /// Adds an implementation of <see cref="IServerTiming"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        public static IServiceCollection AddServerTiming<T>(this IServiceCollection services) where T : class, IServerTiming
        {
            Validator.ThrowIfNull(services);
            services.AddScoped<IServerTiming, T>();
            return services;
        }

		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="FaultDescriptorOptions"/> in the <paramref name="services"/> collection.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
		/// <param name="setup">The <see cref="FaultDescriptorOptions"/> that may be configured.</param>
		/// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="FaultDescriptorOptions"/> in a valid state.
		/// </exception>
		public static IServiceCollection AddNonMvcFaultDescriptor(this IServiceCollection services, Action<FaultDescriptorOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            services.Configure(setup ?? (o =>
            {
                o.SensitivityDetails = options.SensitivityDetails;
                o.ExceptionCallback = options.ExceptionCallback;
                o.ExceptionDescriptorResolver = options.ExceptionDescriptorResolver;
                o.HttpFaultResolvers = options.HttpFaultResolvers;
                o.RequestEvidenceProvider = options.RequestEvidenceProvider;
                o.RootHelpLink = options.RootHelpLink;
                o.UseBaseException = options.UseBaseException;
                o.CancellationToken = options.CancellationToken;
			}));
            services.TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = options.SensitivityDetails);
            return services;
        }
    }
}
