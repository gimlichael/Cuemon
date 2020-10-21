using System;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Assets;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    public class AspNetCoreHostTestTest : AspNetCoreHostTest<AspNetCoreHostFixture>
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationBuilder _pipeline;

        public AspNetCoreHostTestTest(AspNetCoreHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
        {
            _pipeline = hostFixture.Application;
            _provider = hostFixture.ServiceProvider;
        }

        [Fact]
        public async Task ShouldHaveResultOfBoolMiddlewareInBody()
        {
            var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = _provider.GetRequiredService<IOptions<BoolOptions>>();
            var pipeline = _pipeline.Build();

            Assert.Equal("Hello awesome developers!", context.Response.Body.ToEncodedString(o => o.LeaveOpen = true));

            await pipeline(context);

            Assert.Equal("A:True, B:False, C:True, D:False, E:True, F:False", context.Response.Body.ToEncodedString());

            Assert.True(options.Value.A);
            Assert.False(options.Value.B);
            Assert.True(options.Value.C);
            Assert.False(options.Value.D);
            Assert.True(options.Value.E);
            Assert.False(options.Value.F);
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseMiddleware<BoolMiddleware>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, FakeHttpContextAccessor>();
            services.Configure<BoolOptions>(o =>
            {
                o.A = true;
                o.C = true;
                o.E = true;
            });
        }
    }
}