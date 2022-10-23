using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cuemon.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            return services.Add(typeof(TService), typeof(TImplementation), lifetime);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add<TService, TImplementation, TOptions>(this IServiceCollection services, ServiceLifetime lifetime, Action<TOptions> setup)
            where TService : class
            where TImplementation : class, TService
            where TOptions : class, new()
        {
            return services.Add(typeof(TService), typeof(TImplementation), lifetime, setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementation" /> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The implementation type of the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add(this IServiceCollection services, Type service, Type implementation, ServiceLifetime lifetime)
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddServices(service, implementation, lifetime, false);
            return services;
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementation" /> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The implementation type of the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add<TOptions>(this IServiceCollection services, Type service, Type implementation, ServiceLifetime lifetime, Action<TOptions> setup)
            where TOptions : class, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(setup, nameof(setup));
            services.AddServices(service, implementation, lifetime, false);
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            return services.Add(typeof(TService), implementationFactory, lifetime);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add<TService, TImplementation, TOptions>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, ServiceLifetime lifetime, Action<TOptions> setup)
            where TService : class
            where TImplementation : class, TService
            where TOptions : class, new()
        {
            return services.Add(typeof(TService), implementationFactory, lifetime, setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementationFactory" /> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddServices(service, implementationFactory, lifetime, false);
            return services;
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementationFactory" /> and <paramref name="lifetime"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection Add<TOptions>(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime, Action<TOptions> setup)
            where TOptions : class, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(setup, nameof(setup));
            services.AddServices(service, implementationFactory, lifetime, false);
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the configured <paramref name="setup"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection Add<TService>(this IServiceCollection services, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
        {
            return Add<TService, TService>(services, setup);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and configured <paramref name="setup"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
            where TImplementation : class, TService
        {
            return Add(services, typeof(TService), typeof(TImplementation), setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementation" /> and configured <paramref name="setup"/> to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The implementation type of the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection Add(this IServiceCollection services, Type service, Type implementation, Action<TypeForwardServiceOptions> setup = null)
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(service, nameof(service));
            Validator.ThrowIfNull(implementation, nameof(implementation));
            return AddServicesWithNestedTypeForwarding(services, service, implementation, setup, false);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the configured <paramref name="setup"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection Add<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
        {
            return Add<TService, TService>(services, implementationFactory, setup);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and configured <paramref name="setup"/> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
            where TImplementation : class, TService
        {
            return Add(services, typeof(TService), implementationFactory, setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementationFactory" /> and configured <paramref name="setup"/> to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection Add(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, Action<TypeForwardServiceOptions> setup = null)
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(implementationFactory, nameof(implementationFactory));
            return AddServicesWithNestedTypeForwarding(services, service, implementationFactory, setup, false);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the configured <paramref name="setup"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection TryAdd<TService>(this IServiceCollection services, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
        {
            return TryAdd<TService, TService>(services, setup);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and configured <paramref name="setup"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection TryAdd<TService, TImplementation>(this IServiceCollection services, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
            where TImplementation : class, TService
        {
            return TryAdd(services, typeof(TService), typeof(TImplementation), setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementation" /> and configured <paramref name="setup"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The implementation type of the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection TryAdd(this IServiceCollection services, Type service, Type implementation, Action<TypeForwardServiceOptions> setup = null)
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(service, nameof(service));
            Validator.ThrowIfNull(implementation, nameof(implementation));
            return AddServicesWithNestedTypeForwarding(services, service, implementation, setup, true);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the configured <paramref name="setup"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection TryAdd<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
        {
            return TryAdd<TService, TService>(services, implementationFactory, setup);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and configured <paramref name="setup"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection TryAdd<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, Action<TypeForwardServiceOptions> setup = null)
            where TService : class
            where TImplementation : class, TService
        {
            return TryAdd(services, typeof(TService), implementationFactory, setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementationFactory" /> and configured <paramref name="setup"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="setup">The <see cref="TypeForwardServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection TryAdd(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, Action<TypeForwardServiceOptions> setup = null)
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(implementationFactory, nameof(implementationFactory));
            AddServicesWithNestedTypeForwarding(services, service, implementationFactory, setup, true);
            return services;
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            Validator.ThrowIfNull(services, nameof(services));
            return services.TryAdd(typeof(TService), typeof(TImplementation), lifetime);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd<TService, TImplementation, TOptions>(this IServiceCollection services, ServiceLifetime lifetime, Action<TOptions> setup)
            where TService : class
            where TImplementation : class, TService
            where TOptions : class, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(setup, nameof(setup));
            return services.TryAdd(typeof(TService), typeof(TImplementation), lifetime, setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementation" /> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The implementation type of the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd(this IServiceCollection services, Type service, Type implementation, ServiceLifetime lifetime)
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddServices(service, implementation, lifetime, true);
            return services;
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementation" /> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The implementation type of the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd<TOptions>(this IServiceCollection services, Type service, Type implementation, ServiceLifetime lifetime, Action<TOptions> setup)
            where TOptions : class, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(setup, nameof(setup));
            services.AddServices(service, implementation, lifetime, true);
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(implementationFactory, nameof(implementationFactory));
            return services.TryAdd(typeof(TService), implementationFactory, lifetime);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd<TService, TImplementation, TOptions>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, ServiceLifetime lifetime, Action<TOptions> setup)
            where TService : class
            where TImplementation : class, TService
            where TOptions : class, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(implementationFactory, nameof(implementationFactory));
            Validator.ThrowIfNull(setup, nameof(setup));
            return services.TryAdd(typeof(TService), implementationFactory, lifetime, setup);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementationFactory" /> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(implementationFactory, nameof(implementationFactory));
            services.AddServices(service, implementationFactory, lifetime, true);
            return services;
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> with the <paramref name="implementationFactory" /> and <paramref name="lifetime"/> to the <paramref name="services" /> if the service type has not already been registered.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configured options.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The function delegate that creates the service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> after the operation has completed.</returns>
        public static IServiceCollection TryAdd<TOptions>(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime, Action<TOptions> setup)
            where TOptions : class, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            Validator.ThrowIfNull(implementationFactory, nameof(implementationFactory));
            Validator.ThrowIfNull(setup, nameof(setup));
            services.AddServices(service, implementationFactory, lifetime, true);
            services.Configure(setup);
            return services;
        }

        private static void AddServices(this IServiceCollection services, Type service, Type implementation, ServiceLifetime lifetime, bool useTesterDoerPattern)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    Condition.FlipFlop(useTesterDoerPattern, () => services.TryAddScoped(service, implementation), () => services.AddScoped(service, implementation));
                    break;
                case ServiceLifetime.Singleton:
                    Condition.FlipFlop(useTesterDoerPattern, () => services.TryAddSingleton(service, implementation), () => services.AddSingleton(service, implementation));
                    break;
                case ServiceLifetime.Transient:
                    Condition.FlipFlop(useTesterDoerPattern, () => services.TryAddTransient(service, implementation), () => services.AddTransient(service, implementation));
                    break;
            }
        }

        private static void AddServices(this IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime, bool useTesterDoerPattern)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    Condition.FlipFlop(useTesterDoerPattern, () => services.TryAddScoped(service, implementationFactory), () => services.AddScoped(service, implementationFactory));
                    break;
                case ServiceLifetime.Singleton:
                    Condition.FlipFlop(useTesterDoerPattern, () => services.TryAddSingleton(service, implementationFactory), () => services.AddSingleton(service, implementationFactory));
                    break;
                case ServiceLifetime.Transient:
                    Condition.FlipFlop(useTesterDoerPattern, () => services.TryAddTransient(service, implementationFactory), () => services.AddTransient(service, implementationFactory));
                    break;
            }
        }

        private static IServiceCollection AddServicesWithNestedTypeForwarding(IServiceCollection services, Type service, Type implementation, Action<TypeForwardServiceOptions> setup, bool useTesterDoerPattern)
        {
            var options = Patterns.Configure(setup);
            Condition.FlipFlop(useTesterDoerPattern, () => services.TryAdd(service, implementation, options.Lifetime), () => services.Add(service, implementation, options.Lifetime));
            if (options.UseNestedTypeForwarding) { AddServicesWithNestedTypeForwarding(services, service, options, useTesterDoerPattern); }
            return services;
        }

        private static IServiceCollection AddServicesWithNestedTypeForwarding(IServiceCollection services, Type service, Func<IServiceProvider, object> implementationFactory, Action<TypeForwardServiceOptions> setup, bool useTesterDoerPattern)
        {
            var options = Patterns.Configure(setup);
            Condition.FlipFlop(useTesterDoerPattern, () => services.TryAdd(service, implementationFactory, options.Lifetime), () => services.Add(service, implementationFactory, options.Lifetime));
            if (options.UseNestedTypeForwarding) { AddServicesWithNestedTypeForwarding(services, service, options, useTesterDoerPattern); }
            return services;
        }

        private static void AddServicesWithNestedTypeForwarding(IServiceCollection services, Type service, TypeForwardServiceOptions options, bool useTesterDoerPattern)
        {
            var hasMarkerType = service.TryGetDependencyInjectionMarker(out _);
            foreach (var groupingTypes in options.NestedTypeSelector(service).Where(type => options.NestedTypePredicate(type)).GroupBy(type => Decorator.Enclose(type).ToFriendlyName(o => o.ExcludeGenericArguments = true)))
            {
                var orderedGroupingTypes = groupingTypes.OrderBy(type => type.Name, StringComparer.InvariantCulture);
                if (useTesterDoerPattern)
                {
                    TryAdd(services, hasMarkerType ? orderedGroupingTypes.Last() : orderedGroupingTypes.First(), p => p.GetRequiredService(service), options.Lifetime);
                }
                else
                {
                    Add(services, hasMarkerType ? orderedGroupingTypes.Last() : orderedGroupingTypes.First(), p => p.GetRequiredService(service), options.Lifetime);
                }
            }
        }
    }
}
