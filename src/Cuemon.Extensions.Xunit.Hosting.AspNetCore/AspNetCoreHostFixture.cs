using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IAspNetCoreHostFixture"/> interface.
    /// </summary>
    /// <seealso cref="HostFixture" />
    /// <seealso cref="IAspNetCoreHostFixture" />
    public class AspNetCoreHostFixture : HostFixture, IAspNetCoreHostFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreHostFixture"/> class.
        /// </summary>
        public AspNetCoreHostFixture()
        {
        }

        /// <summary>
        /// Creates and configures the <see cref="IWebHost" /> of this instance.
        /// </summary>
        /// <param name="hostTest">The object that inherits from <see cref="AspNetCoreHostTest{T}" />.</param>
        /// <remarks><paramref name="hostTest" /> was added to support those cases where the caller is required in the host configuration.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostTest"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hostTest"/> is not assignable from <see cref="AspNetCoreHostTest{T}"/>.
        /// </exception>
        public override void ConfigureHost(Test hostTest)
        {
            var hostTestType = hostTest?.GetType();
            Validator.ThrowIfNull(hostTest, nameof(hostTest));
            Validator.ThrowIfNotContainsType(hostTestType, nameof(hostTestType), $"{nameof(hostTest)} is not assignable from AspNetCoreHostTest<T>.", typeof(AspNetCoreHostTest<>));

            Host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseEnvironment("Development")
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            config.AddJsonFile("appsettings.json", true, true)
                                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                                .AddEnvironmentVariables();

                            ConfigureCallback(config.Build(), context.HostingEnvironment);
                        })
                        .ConfigureLogging((context, logging) =>
                        {
                            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                            logging.AddConsole();
                            logging.AddDebug();
                            logging.AddEventSourceLogger();
                        })
                        .ConfigureServices((context, services) =>
                        {
                            Configuration = context.Configuration;
                            HostingEnvironment = context.HostingEnvironment;
                            ConfigureServicesCallback(services);
                            ServiceProvider = services.BuildServiceProvider();
                        })
                        .Configure(app =>
                            {
                                ConfigureApplicationCallback(app);
                                Application = app;
                            }
                        );
                }).Start();
        }

        /// <summary>
        /// Gets or sets the delegate that configures the HTTP request pipeline.
        /// </summary>
        /// <value>The delegate that configures the HTTP request pipeline.</value>
        public Action<IApplicationBuilder> ConfigureApplicationCallback { get; set; }

        /// <summary>
        /// Gets the <see cref="IApplicationBuilder" /> initialized by the <see cref="IHost" />.
        /// </summary>
        /// <value>The <see cref="IApplicationBuilder" /> initialized by the <see cref="IHost" />.</value>
        public IApplicationBuilder Application { get; protected set; }
    }
}