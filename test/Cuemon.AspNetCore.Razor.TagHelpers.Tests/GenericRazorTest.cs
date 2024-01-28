using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    public class GenericRazorTest : Test
    {
        public GenericRazorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Index_VerifyCachingWorks()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
                   {
                       services.AddRazorPages();
                       services.Configure<HttpCacheableOptions>(o =>
                       {
                           o.CacheControl.MaxAge = TimeSpan.FromHours(1);
                           o.CacheControl.NoTransform = true;
                           o.CacheControl.Public = true;
                           o.CacheControl.Private = false;
                           o.Filters.AddEntityTagHeader(eo => eo.UseEntityTagResponseParser = true);
                       });
                       services.AddControllersWithViews(o =>
                       {
                           o.Filters.Add<HttpCacheableFilter>();
                       });
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
                   }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/Index");
                var body = await result.Content.ReadAsStringAsync();
                var etag = result.Headers.ETag!.ToString();

                TestOutput.WriteLine(body);

                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);

                client.DefaultRequestHeaders.Add(HeaderNames.IfNoneMatch, etag);

                result = await client.GetAsync("/Index");

                Assert.Equal(StatusCodes.Status304NotModified, (int)result.StatusCode);

                body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);
            }
        }
    }
}
