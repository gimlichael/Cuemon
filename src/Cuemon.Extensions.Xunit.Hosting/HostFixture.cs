using System;
using System.IO;
using Cuemon.Collections.Generic;
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
            Validator.ThrowIfNull(hostTest);
            Validator.ThrowIfNotContainsType(hostTest, Arguments.ToArrayOf(typeof(HostTest<>)), $"{nameof(hostTest)} is not assignable from HostTest<T>.");

            var hb = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((context, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();

                    ConfigureCallback(config.Build(), context.HostingEnvironment);
                })
                .ConfigureServices((context, services) =>
                {
                    Configuration = context.Configuration;
                    HostingEnvironment = context.HostingEnvironment;
                    ConfigureServicesCallback(services);
                });

            ConfigureHostCallback(hb);

#if NET9_0_OR_GREATER
            hb.UseDefaultServiceProvider(o => o.ValidateScopes = false); // this is by intent
#endif

            Host = hb.Build();
        }

#if NETSTANDARD2_0_OR_GREATER
        /// <summary>
        /// Gets or sets the delegate that initializes the test class.
        /// </summary>
        /// <value>The delegate that initializes the test class.</value>
        /// <remarks>Mimics the Startup convention.</remarks>
        public Action<IConfiguration, IHostingEnvironment> ConfigureCallback { get; set; }
#else
        /// <summary>
        /// Gets or sets the delegate that initializes the test class.
        /// </summary>
        /// <value>The delegate that initializes the test class.</value>
        /// <remarks>Mimics the Startup convention.</remarks>
        public Action<IConfiguration, IHostEnvironment> ConfigureCallback { get; set; }
#endif

        /// <summary>
        /// Gets or sets the delegate that initializes the host builder.
        /// </summary>
        /// <value>The delegate that initializes the host builder.</value>
        public Action<IHostBuilder> ConfigureHostCallback { get; set; }

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
        public IServiceProvider ServiceProvider => Host?.Services;

        /// <summary>
        /// Gets the <see cref="IConfiguration" /> initialized by this instance.
        /// </summary>
        /// <value>The <see cref="IConfiguration" /> initialized by this instance.</value>
        public IConfiguration Configuration { get; protected set; }

#if NETSTANDARD2_0_OR_GREATER
        /// <summary>
        /// Gets the <see cref="IHostingEnvironment"/> initialized by this instance.
        /// </summary>
        /// <value>The <see cref="IHostingEnvironment"/> initialized by this instance.</value>
        public IHostingEnvironment HostingEnvironment { get; protected set; }
#else
        /// <summary>
        /// Gets the <see cref="IHostEnvironment"/> initialized by this instance.
        /// </summary>
        /// <value>The <see cref="IHostEnvironment"/> initialized by this instance.</value>
        public IHostEnvironment HostingEnvironment { get; protected set; }
#endif

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
