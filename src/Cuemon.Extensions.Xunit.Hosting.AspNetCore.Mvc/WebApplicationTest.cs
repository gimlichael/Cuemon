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
        private readonly bool _useClassFixture;

        internal WebApplicationTest(bool useClassFixture, Action<IApplicationBuilder> pipelineConfigurator, Action<IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator)
        {
            _useClassFixture = useClassFixture;
            _middlewareTest = MiddlewareTestFactory.Create(useClassFixture, pipelineConfigurator, serviceConfigurator, hostConfigurator);
        }

        internal WebApplicationTest(bool useClassFixture, Action<HostBuilderContext, IApplicationBuilder> pipelineConfigurator, Action<HostBuilderContext, IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator)
        {
            _useClassFixture = useClassFixture;
            _middlewareTest = MiddlewareTestFactory.CreateWithHostBuilderContext(useClassFixture, pipelineConfigurator, serviceConfigurator, hostConfigurator);
        }

        public IServiceProvider ServiceProvider => _middlewareTest.ServiceProvider;

        public IApplicationBuilder Application => _middlewareTest.Application;

        public IConfiguration Configuration => _middlewareTest.Configuration;

        public IHostEnvironment HostingEnvironment => _middlewareTest.HostingEnvironment;

        public void Dispose()
        {
            _middlewareTest.Dispose();
        }

        public IHost Host => _useClassFixture ? ((AspNetCoreHostTest<AspNetCoreHostFixture>)_middlewareTest).Host : ((AspNetCoreHostTest)_middlewareTest).Host;

        public Type CallerType => _middlewareTest.CallerType;
    }
}
