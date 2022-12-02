using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    internal sealed class GenericHostTest : HostTest, IGenericHostTest
    {
        private readonly Action<IServiceCollection> _serviceConfigurator;
        private readonly Action<HostBuilderContext, IServiceCollection> _serviceConfiguratorWithContext;
        private readonly Action<IHostBuilder> _hostConfigurator;
        private HostBuilderContext _hostBuilderContext;

        internal GenericHostTest(Action<IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator, HostFixture hostFixture) : base(callerType: serviceConfigurator?.Target?.GetType() ?? hostConfigurator?.Target?.GetType())
        {
            _serviceConfigurator = serviceConfigurator;
            _hostConfigurator = hostConfigurator;
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureHostCallback = ConfigureHost;
                hostFixture.ConfigureCallback = Configure;
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureHost(this);
            }
            HostFixture = hostFixture;
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.Host.Services;
            Configure(hostFixture.Configuration, hostFixture.HostingEnvironment);
        }

        internal GenericHostTest(Action<HostBuilderContext, IServiceCollection> serviceConfigurator, Action<IHostBuilder> hostConfigurator, HostFixture hostFixture) : base(callerType: serviceConfigurator?.Target?.GetType() ?? hostConfigurator?.Target?.GetType())
        {
            _serviceConfiguratorWithContext = serviceConfigurator;
            _hostConfigurator = hostConfigurator;
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureHostCallback = ConfigureHost;
                hostFixture.ConfigureCallback = Configure;
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureHost(this);
            }
            HostFixture = hostFixture;
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.Host.Services;
            Configure(hostFixture.Configuration, hostFixture.HostingEnvironment);
        }

        internal HostFixture HostFixture { get; }

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
        }
    }

    internal sealed class GenericHostTestDecorator : HostTest<HostFixture>, IGenericHostTest
    {
        private readonly GenericHostTest _genericHost;

        internal GenericHostTestDecorator(GenericHostTest genericHost) : base(genericHost.HostFixture, callerType: genericHost.CallerType)
        {
            _genericHost = genericHost;
        }

        protected override void InitializeHostFixture(HostFixture hostFixture)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _genericHost.ConfigureServices(services);
        }
    }
}
