using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    internal sealed class WebApplicationTest : IWebApplicationTest
    {
        private readonly IMiddlewareTest _middlewareTest;

        internal WebApplicationTest(Action<IServiceCollection> serviceConfigurator, Action<IApplicationBuilder> pipelineConfigurator, Action<IHostBuilder> hostConfigurator)
        {
            _middlewareTest = MiddlewareTestFactory.Create(serviceConfigurator, pipelineConfigurator, hostConfigurator);
        }

        internal WebApplicationTest(Action<HostBuilderContext, IServiceCollection> serviceConfigurator, Action<HostBuilderContext, IApplicationBuilder> pipelineConfigurator, Action<IHostBuilder> hostConfigurator)
        {
            _middlewareTest = MiddlewareTestFactory.CreateWithHostBuilderContext(serviceConfigurator, pipelineConfigurator, hostConfigurator);
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

        public Type CallerType => _middlewareTest.CallerType;
    }
}
