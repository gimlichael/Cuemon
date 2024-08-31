using Cuemon.Extensions.Xunit.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Hosting
{
    public class HostEnvironmentExtensionsTest : HostTest<HostFixture>
    {
        public HostEnvironmentExtensionsTest(HostFixture hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
        {
        }

        protected override void ConfigureHost(IHostBuilder hb)
        {
            hb.UseEnvironment(Environments.LocalDevelopment);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
        }

        [Fact]
        public void IsLocalDevelopment_VerifyEnvironmentEqualsLocalDevelopment()
        {
#if NET6_0_OR_GREATER
            Assert.True(HostingEnvironment.IsLocalDevelopment());
            Assert.False(HostingEnvironment.IsProduction());
            Assert.False(HostingEnvironment.IsStaging());
            Assert.False(HostingEnvironment.IsDevelopment());
#else
            Assert.True(HostingEnvironment.IsLocalDevelopment());
            Assert.False(HostingEnvironment.IsProduction());
            Assert.False(HostingEnvironment.IsStaging());
            Assert.False(HostingEnvironment.IsDevelopment());
#endif
            TestOutput.WriteLine(HostingEnvironment.EnvironmentName);
        }

        [Fact]
        public void IsLocalDevelopment_VerifyEnvironmentIsNonProduction()
        {
#if NET6_0_OR_GREATER
            Assert.True(HostingEnvironment.IsNonProduction());
            Assert.False(HostingEnvironment.IsProduction());
#else
            Assert.True(HostingEnvironment.IsNonProduction());
            Assert.False(HostingEnvironment.IsProduction());
#endif
            TestOutput.WriteLine(HostingEnvironment.EnvironmentName);
        }
    }
}