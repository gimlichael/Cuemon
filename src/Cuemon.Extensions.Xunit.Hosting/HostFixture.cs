using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IHostFixture"/> interface.
    /// </summary>
    /// <seealso cref="Disposable" />
    /// <seealso cref="IHostFixture" />
    public class HostFixture : Disposable, IHostFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostFixture"/> class.
        /// </summary>
        public HostFixture()
        {
        }

        /// <summary>
        /// Creates and configures the <see cref="IHost" /> of this instance.
        /// </summary>
        /// <param name="hostTest">The object that inherits from <see cref="HostTest{T}"/>.</param>
        /// <remarks><paramref name="hostTest"/> was added to support those cases where the caller is required in the host configuration.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostTest"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hostTest"/> is not assignable from <see cref="HostTest{T}"/>.
        /// </exception>
        public virtual void ConfigureHost(Test hostTest)
        {
            var hostTestType = hostTest?.GetType();
            Validator.ThrowIfNull(hostTest, nameof(hostTest));
            Validator.ThrowIfNotContainsType(hostTestType, nameof(hostTestType), $"{nameof(hostTest)} is not assignable from HostTest<T>.", typeof(HostTest<>));

            Host = new HostBuilder()
                .ConfigureHostConfiguration(config => config.AddEnvironmentVariables("DOTNET_"))
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    hostTestType.GetField("_configuration", BindingFlags.NonPublic).SetValue(hostTest, context.Configuration);
                    hostTestType.GetField("_hostingEnvironment", BindingFlags.NonPublic).SetValue(hostTest, context.HostingEnvironment);
                    ConfigureServicesCallback(services);
                    ServiceProvider = services.BuildServiceProvider();
                }).Build();
        }

        /// <summary>
        /// Gets or sets the delegate that adds services to the container.
        /// </summary>
        /// <value>The delegate that adds services to the container.</value>
        public Action<IServiceCollection> ConfigureServicesCallback { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IHost" /> initialized by this instance.
        /// </summary>
        /// <value>The <see cref="IHost" /> initialized by this instance.</value>
        public IHost Host { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider" /> initialized by this instance.
        /// </summary>
        /// <value>The <see cref="IServiceProvider" /> initialized by this instance.</value>
        public IServiceProvider ServiceProvider { get; protected set; }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="M:Cuemon.Disposable.Dispose" /> or <see cref="M:Cuemon.Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="P:Cuemon.Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            if (ServiceProvider is ServiceProvider sp)
            {
                sp.Dispose();
            }
            Host?.Dispose();
        }
    }
}