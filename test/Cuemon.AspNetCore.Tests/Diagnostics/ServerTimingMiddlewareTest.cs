using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Diagnostics
{
    public class ServerTimingMiddlewareTest : Test
    {
        public ServerTimingMiddlewareTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task InvokeAsync_ShouldProviderServerTimingHeaderWithMetrics()
        {
            using (var middleware = MiddlewareTestFactory.Create(app =>
            {
                app.Use(async (context, next) =>
                {
                    var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                    serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                    serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                    await next();
                });
                app.UseServerTiming();
            }, services =>
            {
                services.AddServerTiming();
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                var sut0 = context.Response.Headers[ServerTiming.HeaderName];
                await pipeline(context);
                var sut1 = context.Response.Headers[ServerTiming.HeaderName];

                TestOutput.WriteLine(sut1);

                Assert.Empty(sut0.ToString());
                Assert.Equal("redis", sut1[0].Split(';').First());
                Assert.Equal("restApi", sut1[1].Split(';').First());
            }
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotProviderServerTimingHeaderWithMetrics()
        {
            using (var middleware = MiddlewareTestFactory.Create(app =>
            {
                app.Use(async (context, next) =>
                {
                    var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                    serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                    serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                    await next();
                });
                //app.UseServerTiming(); intentionally removed
            }, services =>
            {
                services.AddServerTiming();
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                var sut0 = context.Response.Headers[ServerTiming.HeaderName];
                await pipeline(context);
                var sut1 = context.Response.Headers[ServerTiming.HeaderName];

                Assert.Empty(sut0.ToString());
                Assert.Empty(sut1.ToString());
            }
        }
    }
}