using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            using (var middleware = MiddlewareTestFactory.Create(services =>
            {
                services.AddServerTiming();
            }, app =>
                   {
                       app.Use(async (context, next) =>
                       {
                           var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                           serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                           serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                           await next();
                       });
                       app.UseServerTiming();
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
        public async Task InvokeAsync_ShouldExcludeServerTimingHeaderWithMetrics_ButIncludeLogLevelDebug()
        {
	        using (var middleware = MiddlewareTestFactory.Create(services =>
	               {
		               services.AddServerTiming();
		               services.AddXunitTestLogging(TestOutput);
	               }, app =>
	               {
		               app.Use(async (context, next) =>
		               {
			               var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

			               serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
			               serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

			               await next();
		               });
		               app.UseServerTiming(o =>
		               {
			               o.SuppressHeaderPredicate = _ => true;
		               });
	               }))
	        {
		        var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var logger = middleware.ServiceProvider.GetRequiredService<ILogger<ServerTimingMiddleware>>();
                var loggerStore = logger.GetTestStore();
				var pipeline = middleware.Application.Build();

		        await pipeline(context);

		        var serverTimingHeader = context.Response.Headers[ServerTiming.HeaderName];

                Assert.Empty(serverTimingHeader.ToString());
                Assert.Collection(loggerStore.Query(),
	                entry => Assert.Equal("Debug: ServerTimingMetric { Name: redis, Duration: 22.0ms, Description: \"Redis Cache\" }", entry.ToString()),
	                entry => Assert.Equal("Debug: ServerTimingMetric { Name: restApi, Duration: 1700.0ms, Description: \"Some REST API integration\" }", entry.ToString()));
	        }
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotProviderServerTimingHeaderWithMetrics()
        {
            using (var middleware = MiddlewareTestFactory.Create(services =>
            {
                services.AddServerTiming();
            }, app =>
                   {
                       app.Use(async (context, next) =>
                       {
                           var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                           serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                           serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                           await next();
                       });
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