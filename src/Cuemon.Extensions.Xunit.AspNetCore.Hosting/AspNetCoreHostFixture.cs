using System;
using System.IO;
using System.Linq;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddEnvironmentVariables("ASPNETCORE_");
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    var flags = new MemberReflection(excludeStatic: true, excludePublic: true).Flags;
                    var hostTestTypeBase = Decorator.Enclose(hostTestType).GetInheritedTypes().Single(t => t.BaseType == typeof(Test));
                    hostTestTypeBase.GetField("_configuration", flags).SetValue(hostTest, context.Configuration);
                    hostTestTypeBase.GetField("_hostingEnvironment", flags).SetValue(hostTest, context.HostingEnvironment);

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
                ));

            var host = server.Host;

            host.Start();
            
            Host = host;
        }

        /// <summary>
        /// Gets the <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.
        /// </summary>
        /// <value>The <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.</value>
        public new IWebHost Host { get; private set; }

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