using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    public class HttpCacheableFilterTest : Test
    {
        public HttpCacheableFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetEtag_ShouldReturnOkWithEtagAndSubsequentlyNotModified()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.Configure<HttpCacheableOptions>(o =>
                {
                    o.Filters.Add(new HttpEntityTagHeaderFilter());
                });
                services.AddControllers(o => { o.Filters.Add<HttpCacheableFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/fake/getCacheByEtag");
                var etag = result.Headers.ETag.ToString();

                Assert.Equal(StatusCodes.Status200OK, (int) result.StatusCode);
                TestOutput.WriteLine(etag);

                client.DefaultRequestHeaders.Add(HeaderNames.IfNoneMatch, etag);
                
                result = await client.GetAsync("/fake/getCacheByEtag");
                Assert.Equal(StatusCodes.Status304NotModified, (int) result.StatusCode);
            }
        }

        [Fact]
        public async Task GetLastModified_ShouldReturnOkWithLastModifiedAndSubsequentlyNotModified()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.Configure<HttpCacheableOptions>(o =>
                {
                    o.Filters.Add(new HttpLastModifiedHeaderFilter());
                });
                services.AddControllers(o => { o.Filters.Add<HttpCacheableFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/fake/getCacheByLastModified");
                var lastModified = result.Content.Headers.LastModified.Value;

                Assert.Equal(StatusCodes.Status200OK, (int) result.StatusCode);
                TestOutput.WriteLine(lastModified.ToString("O"));

                client.DefaultRequestHeaders.Add(HeaderNames.IfModifiedSince, lastModified.ToString("R"));
                
                result = await client.GetAsync("/fake/getCacheByLastModified");
                Assert.Equal(StatusCodes.Status304NotModified, (int) result.StatusCode);
            }
        }
    }
}