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
            _pipelineConfigurator(app);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _serviceConfigurator(services);
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="M:Cuemon.Disposable.Dispose" /> or <see cref="M:Cuemon.Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="P:Cuemon.Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            Host?.Dispose();
        }
    }
}