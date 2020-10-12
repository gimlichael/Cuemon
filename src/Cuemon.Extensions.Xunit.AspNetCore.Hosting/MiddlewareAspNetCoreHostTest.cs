using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    internal class MiddlewareAspNetCoreHostTest : AspNetCoreHostTest<AspNetCoreHostFixture>, IMiddlewareTest
    {
        private readonly Action<IApplicationBuilder> _pipelineConfigurator;
        private readonly Action<IServiceCollection> _serviceConfigurator;

        internal MiddlewareAspNetCoreHostTest(Action<IApplicationBuilder> pipelineConfigurator, Action<IServiceCollection> serviceConfigurator, AspNetCoreHostFixture hostFixture) : base(hostFixture)
        {
            _pipelineConfigurator = pipelineConfigurator;
            _serviceConfigurator = serviceConfigurator;
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureApplicationCallback = ConfigureApplication;
                hostFixture.ConfigureHost(this);
            }
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.ServiceProvider;
            Application = hostFixture.Application;
        }

        protected override void InitializeHostFixture(AspNetCoreHostFixture hostFixture)
        {
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            _pipelineConfigurator(app);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _serviceConfigurator(services);
        }
    }
}