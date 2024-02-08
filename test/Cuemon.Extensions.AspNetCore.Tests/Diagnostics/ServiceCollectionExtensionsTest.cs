using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddServerTiming_ShouldAddToServiceCollection_HavingLifetimeOfScope()
        {
            var sut1 = new ServiceCollection().AddServerTiming();
            var sut2 = sut1.Single(sd => sd.ServiceType == typeof(IServerTiming));

            Assert.True(sut2.Lifetime == ServiceLifetime.Scoped);
            Assert.True(sut2.ImplementationType == typeof(ServerTiming));
        }
    }
}