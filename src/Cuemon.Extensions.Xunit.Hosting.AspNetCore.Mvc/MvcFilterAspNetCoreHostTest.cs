using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    internal sealed class MvcFilterAspNetCoreHostTest : IMvcFilterTest
    {
        private readonly IMiddlewareTest _middlewareTest;

        internal MvcFilterAspNetCoreHostTest(Action<IApplicationBuilder> pipelineConfigurator, Action<IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator)
        {
            _middlewareTest = MiddlewareTestFactory.CreateMiddlewareTest(pipelineConfigurator, serviceConfigurator, hostConfigurator);
        }

        public IServiceProvider ServiceProvider => _middlewareTest.ServiceProvider;

        public IApplicationBuilder Application => _middlewareTest.Application;

        public IConfiguration Configuration => _middlewareTest.Configuration;

        public IHostEnvironment HostingEnvironment => _middlewareTest.HostingEnvironment;

        public void Dispose()
        {
            _middlewareTest.Dispose();
        }

        public IHost Host => ((AspNetCoreHostTest<AspNetCoreHostFixture>)_middlewareTest).Host;
    }
}