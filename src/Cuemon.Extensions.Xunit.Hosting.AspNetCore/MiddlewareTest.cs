using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    internal sealed class MiddlewareTest : AspNetCoreHostTest, IMiddlewareTest
    {
        private readonly Action<IApplicationBuilder> _pipelineConfigurator;
        private readonly Action<IServiceCollection> _serviceConfigurator;
        private readonly Action<HostBuilderContext, IApplicationBuilder> _pipelineConfiguratorWithContext;
        private readonly Action<HostBuilderContext, IServiceCollection> _serviceConfiguratorWithContext;
        private readonly Action<IHostBuilder> _hostConfigurator;
        private HostBuilderContext _hostBuilderContext;

        internal MiddlewareTest(Action<IApplicationBuilder> pipelineConfigurator, Action<IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator, AspNetCoreHostFixture hostFixture) : base(callerType: pipelineConfigurator?.Target?.GetType() ?? serviceConfigurator?.Target?.GetType() ?? hostConfigurator?.Target?.GetType())
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
            HostFixture = hostFixture;
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.Host.Services;
            Application = hostFixture.Application;
            Configure(hostFixture.Configuration, hostFixture.HostingEnvironment);
        }

        internal MiddlewareTest(Action<HostBuilderContext, IApplicationBuilder> pipelineConfigurator, Action<HostBuilderContext, IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator, AspNetCoreHostFixture hostFixture) : base(callerType: pipelineConfigurator?.Target?.GetType() ?? serviceConfigurator?.Target?.GetType() ?? hostConfigurator?.Target?.GetType())
        {
            _pipelineConfiguratorWithContext = pipelineConfigurator;
            _serviceConfiguratorWithContext = serviceConfigurator;
            _hostConfigurator = hostConfigurator;
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureHostCallback = ConfigureHost;
                hostFixture.ConfigureCallback = Configure;
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureApplicationCallback = ConfigureApplication;
                hostFixture.ConfigureHost(this);
            }
            HostFixture = hostFixture;
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.Host.Services;
            Application = hostFixture.Application;
            Configure(hostFixture.Configuration, hostFixture.HostingEnvironment);
        }

        internal AspNetCoreHostFixture HostFixture { get; }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            _pipelineConfigurator?.Invoke(app);
            _pipelineConfiguratorWithContext?.Invoke(Tweaker.Adjust(_hostBuilderContext, hbc =>
            {
                hbc.Configuration = Configuration;
                hbc.HostingEnvironment = HostingEnvironment;
                return hbc;
            }), app);
        }

        protected override void ConfigureHost(IHostBuilder hb)
        {
            _hostBuilderContext = new HostBuilderContext(hb.Properties);
            _hostConfigurator?.Invoke(hb);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _serviceConfigurator?.Invoke(services);
            _serviceConfiguratorWithContext?.Invoke(Tweaker.Adjust(_hostBuilderContext, hbc =>
            {
                hbc.Configuration = Configuration;
                hbc.HostingEnvironment = HostingEnvironment;
                return hbc;
            }), services);
            services.AddFakeHttpContextAccessor(ServiceLifetime.Scoped);
        }
    }

    internal sealed class MiddlewareTestDecorator : AspNetCoreHostTest<AspNetCoreHostFixture>, IMiddlewareTest
    {
        private readonly MiddlewareTest _middleware;

        internal MiddlewareTestDecorator(MiddlewareTest middleware) : base(middleware.HostFixture, callerType: middleware.CallerType)
        {
            _middleware = middleware;
            Application = middleware.Application;
            Host = middleware.Host;
            ServiceProvider = middleware.Host.Services;
            Application = middleware.Application;
            Configure(middleware.Configuration, middleware.HostingEnvironment);
        }

        protected override void InitializeHostFixture(AspNetCoreHostFixture hostFixture)
        {
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            _middleware.ConfigureApplication(app);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _middleware.ConfigureServices(services);
        }
    }
}
