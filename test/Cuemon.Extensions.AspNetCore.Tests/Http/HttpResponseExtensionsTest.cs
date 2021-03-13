using System;
using System.Threading.Tasks;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Http
{
    public class HttpResponseExtensionsTest : AspNetCoreHostTest<AspNetCoreHostFixture>
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationBuilder _pipeline;

        public HttpResponseExtensionsTest(AspNetCoreHostFixture hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
        {
            _pipeline = hostFixture.Application;
            _provider = hostFixture.ServiceProvider;
        }

        [Fact]
        public async Task AddOrUpdateEntityTagHeader_ShouldHaveNoEntityTagHeader()
        {
            var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var pipeline = _pipeline.Build();

            await pipeline(context);

            Assert.Empty(context.Response.Headers[HeaderNames.ETag]);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }

        [Fact]
        public async Task AddOrUpdateEntityTagHeader_ShouldAddEntityTagHeader()
        {
            var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var pipeline = _pipeline.Build();

            await pipeline(context);

            context.Response.AddOrUpdateEntityTagHeader(context.Request, new ChecksumBuilder(() => HashFactory.CreateFnv128()));

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            Assert.Collection(context.Response.Headers[HeaderNames.ETag], s => Assert.Equal("\"648e5c529d659a6882387b946cc37a83\"", s));
        }

        [Fact]
        public async Task AddOrUpdateEntityTagHeader_ShouldAddEntityTagHeader_ChangingStatusCodeToNotModified()
        {
            var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var pipeline = _pipeline.Build();

            await pipeline(context);

            context.Request.Headers.Add(HeaderNames.IfNoneMatch, "\"648e5c529d659a6882387b946cc37a83\"");
            context.Response.AddOrUpdateEntityTagHeader(context.Request, new ChecksumBuilder(() => HashFactory.CreateFnv128()));

            Assert.Equal(StatusCodes.Status304NotModified, context.Response.StatusCode);
            Assert.Collection(context.Request.Headers[HeaderNames.IfNoneMatch], s => Assert.Equal("\"648e5c529d659a6882387b946cc37a83\"", s));
            Assert.Collection(context.Response.Headers[HeaderNames.ETag], s => Assert.Equal("\"648e5c529d659a6882387b946cc37a83\"", s));
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddTransient<IHttpContextAccessor, FakeHttpContextAccessor>();
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(routes => routes.MapGet("/", context =>
            {
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            }));
        }
    }
}