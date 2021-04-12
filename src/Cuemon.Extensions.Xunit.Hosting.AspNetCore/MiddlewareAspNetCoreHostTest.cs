using System;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    internal sealed class MiddlewareAspNetCoreHostTest : AspNetCoreHostTest<AspNetCoreHostFixture>, IMiddlewareTest
    {
        private readonly Action<IApplicationBuilder> _pipelineConfigurator;
        private readonly Action<IServiceCollection> _serviceConfigurator;
        private readonly Action<IHostBuilder> _hostConfigurator;

        internal MiddlewareAspNetCoreHostTest(Action<IApplicationBuilder> pipelineConfigurator, Action<IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator, AspNetCoreHostFixture hostFixture) : base(hostFixture, callerType: pipelineConfigurator?.Target?.GetType() ?? serviceConfigurator?.Target?.GetType() ?? hostConfigurator?.Target?.GetType())
        {
            _pipelineConfigurator = pipelineConfigurator;
            _serviceConfigurator = serviceConfigurator;
            _hostConfigurator = hostConfigurator;
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureHostCallback = ConfigureHost;
                hostFixture.ConfigureCallback = Configure;
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureApplicationCallback = ConfigureApplication;
                hostFixture.ConfigureHost(this);
            }
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.Host.Services;
            Application = hostFixture.Application;
            Configure(hostFixture.Configuration, hostFixture.HostingEnvironment);
        }

        protected override void InitializeHostFixture(AspNetCoreHostFixture hostFixture)
        {
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            _pipelineConfigurator?.Invoke(app);
        }

        protected override void ConfigureHost(IHostBuilder hb)
        {
            _hostConfigurator?.Invoke(hb);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _serviceConfigurator?.Invoke(services);
            services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
        }
    }
}