using System;
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
    }
}