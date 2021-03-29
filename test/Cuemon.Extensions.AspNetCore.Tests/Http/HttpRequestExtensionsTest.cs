using System;
using System.Threading.Tasks;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Http
{
    public class HttpRequestExtensionsTest : Test
    {
        public HttpRequestExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("HEAD")]
        [InlineData("get")]
        [InlineData("head")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("TRACE")]
        public async Task IsGetOrHeadMethod_ShouldRecognizeGetOrHeadMethod(string method)
        {
            bool sut = false;
            await MiddlewareTestFactory.RunMiddlewareTest(app =>
            {
                app.Run(context =>
                {
                    context.Request.Method = method;
                    sut = context.Request.IsGetOrHeadMethod();
                    return Task.CompletedTask;
                });
            });
            Condition.FlipFlop(method.Equals("GET", StringComparison.OrdinalIgnoreCase) || method.Equals("HEAD", StringComparison.OrdinalIgnoreCase), () => Assert.True(sut), () => Assert.False(sut));
        }

        [Theory]
        [InlineData("fcad9155c895f17a")]
        [InlineData("xxxxxxxxxxxxxxxx")]
        public async Task IsClientSideResourceCached_ShouldCompareIfNoneMatchToChecksumBuilder(string checksum)
        {
            var sut = CacheValidatorFactory.CreateValidator(typeof(HttpRequestExtensionsTest).Assembly);
            bool isClientSideResourceCached = false;
            int statusCdoe = 100;
            await MiddlewareTestFactory.RunMiddlewareTest(app =>
            {
                app.Use(async (context, next) =>
                {
                    context.Request.Headers.AddOrUpdateHeader(HeaderNames.IfNoneMatch, $"\"{checksum}\"");
                    isClientSideResourceCached = context.Request.IsClientSideResourceCached(sut);
                    context.Response.StatusCode = isClientSideResourceCached ? 304 : 200;
                    await next();
                });
                app.Run(context =>
                {
                    statusCdoe = context.Response.StatusCode;
                    return Task.CompletedTask;
                });
            });

            Condition.FlipFlop(checksum.Equals("xxxxxxxxxxxxxxxx"), () =>
            {
                Assert.False(isClientSideResourceCached);
                Assert.Equal(200, statusCdoe);
            }, () =>
            {
                Assert.True(isClientSideResourceCached);
                Assert.Equal(304, statusCdoe);
            });
        }

        [Theory]
        [InlineData("Thu, 01 Jan 1970 00:00:00 GMT")]
        [InlineData("Mon, 29 Mar 2021 00:00:00 GMT")]
        public async Task IsClientSideResourceCached_ShouldCompareIfModifiedSinceToDateTime(string modified)
        {
            var sut = DateTime.Parse("Mon, 29 Mar 2021 00:00:00 GMT").AddDays(-1);
            bool isClientSideResourceCached = false;
            int statusCdoe = 100;
            await MiddlewareTestFactory.RunMiddlewareTest(app =>
            {
                app.Use(async (context, next) =>
                {
                    context.Request.Headers.AddOrUpdateHeader(HeaderNames.IfModifiedSince, $"{modified}");
                    isClientSideResourceCached = context.Request.IsClientSideResourceCached(sut);
                    context.Response.StatusCode = isClientSideResourceCached ? 304 : 200;
                    await next();
                });
                app.Run(context =>
                {
                    statusCdoe = context.Response.StatusCode;
                    return Task.CompletedTask;
                });
            });

            Condition.FlipFlop(modified.Equals("Thu, 01 Jan 1970 00:00:00 GMT"), () =>
            {
                Assert.False(isClientSideResourceCached);
                Assert.Equal(200, statusCdoe);
            }, () =>
            {
                Assert.True(isClientSideResourceCached);
                Assert.Equal(304, statusCdoe);
            });
        }
    }
}