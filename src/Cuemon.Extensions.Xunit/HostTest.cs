using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit
{
    public abstract class HostTest : Test
    {
        protected HostTest(ITestOutputHelper output = null) : base(output)
        {
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
                    ConfigureServices(services);
                    Configuration = context.Configuration;
                    HostingEnvironment = context.HostingEnvironment;
                    ServiceProvider = services.BuildServiceProvider();
                }).Build();
        }

        public IHost Host { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        #if NETSTANDARD
        public IHostingEnvironment HostingEnvironment { get; private set; }
        #elif NETCOREAPP
        public IHostEnvironment HostingEnvironment { get; private set; }
        #endif

        public abstract void ConfigureServices(IServiceCollection services);

        protected override void OnDisposeManagedResources()
        {
            if (ServiceProvider is ServiceProvider sp)
            {
                sp.Dispose();
            }
            Host.Dispose();
        }
    }
}