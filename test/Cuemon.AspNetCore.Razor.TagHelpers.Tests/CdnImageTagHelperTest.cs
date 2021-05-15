using System.Threading.Tasks;
using Cuemon.AspNetCore.Razor.TagHelpers.Assets;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    public class CdnImageTagHelperTest : Test
    {
        public CdnImageTagHelperTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Page_RenderImageTagForCdnRole()
        {
            using (var filter = WebApplicationTestFactory.CreateWebApplicationTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
            }, services =>
            {
                services.AddRazorPages();
                services.Configure<CdnTagHelperOptions>(o =>
                {
                    o.Scheme = ProtocolUriScheme.Https;
                    o.BaseUrl = "nblcdn.net";
                });
                services.Configure<AppTagHelperOptions>(o =>
                {
                    o.Scheme = ProtocolUriScheme.Relative;
                    o.BaseUrl = "static.cuemon.net";
                });
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/CdnImageTagHelper");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"<img src=""https://nblcdn.net/devops.svg"" alt=""DevOps Factory"">", body, ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task Page_RenderImageTagForCdnRole_WithCacheBusting()
        {
            using (var filter = WebApplicationTestFactory.CreateWebApplicationTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
            }, services =>
            {
                services.AddCacheBusting<FakeCacheBusting>();
                services.AddRazorPages();
                services.Configure<CdnTagHelperOptions>(o =>
                {
                    o.Scheme = ProtocolUriScheme.Https;
                    o.BaseUrl = "nblcdn.net";
                });
                services.Configure<AppTagHelperOptions>(o =>
                {
                    o.Scheme = ProtocolUriScheme.Relative;
                    o.BaseUrl = "static.cuemon.net";
                });
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/CdnImageTagHelper");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"<img src=""https://nblcdn.net/devops.svg?v=00000000000000000000000000000000"" alt=""DevOps Factory"">", body, ignoreLineEndingDifferences: true);
            }
        }
    }
}
