using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Hosting;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Hosting
{
    public class HostingEnvironmentMiddlewareTest : AspNetCoreHostTest<AspNetCoreHostFixture>
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationBuilder _pipeline;

        public HostingEnvironmentMiddlewareTest(AspNetCoreHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
        {
            _pipeline = hostFixture.Application;
            _provider = hostFixture.ServiceProvider;
        }

        [Fact]
        public async Task InvokeAsync_ShouldHaveHostingEnvironmentHeader_ConfiguredByIOptions()
        {
            var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = _provider.GetRequiredService<IOptions<HostingEnvironmentOptions>>();
            var pipeline = _pipeline.Build();

            await pipeline(context);
            
            Assert.True(context.Response.Headers.TryGetValue(options.Value.HeaderName, out var xHostingEnvironmentHeader));
            Assert.Equal(HostingEnvironment.EnvironmentName, xHostingEnvironmentHeader.Single());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddFakeHttpContextAccessor(ServiceLifetime.Transient);
            services.Configure<HostingEnvironmentOptions>(o => o.HeaderName = "X-Environment");
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseHostingEnvironment();
        }
    }
}