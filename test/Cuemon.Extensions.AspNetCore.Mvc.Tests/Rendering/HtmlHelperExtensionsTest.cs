using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Extensions.AspNetCore.Mvc.Models;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Mvc.Rendering
{
    public class HtmlHelperExtensionsTest : Test
    {
        public HtmlHelperExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task EnsureThatBothRazorPagesAndControllerViewAreWorking()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseStaticFiles();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}");
                });
            }, services =>
            {
                services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddAssemblyCacheBusting();
                services.AddControllersWithViews();
                services.AddRazorPages(o => o.Conventions.AddPageRoute("/", "/"));
            }))
            {
                var client = filter.Host.GetTestClient();
                var page = await client.GetAsync("/regions");
                var view = await client.GetAsync("/home");

                Assert.Equal(HttpStatusCode.OK, page.StatusCode);
                Assert.Equal(HttpStatusCode.OK, view.StatusCode);
            }
        }

        [Fact]
        public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnRegionsAnchorTag_AsControllerIsRegionAndActionIsRegion()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseStaticFiles();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}");
                });
            }, services =>
            {
                services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddAssemblyCacheBusting();
                services.AddControllersWithViews();
            }))
            {
                var client = filter.Host.GetTestClient();
                var view = await client.GetAsync("/regions/da-dk/denmark");
                var sut = await view.Content.ReadAsStringAsync();

                TestOutput.WriteLine(sut);

                Assert.Equal(HttpStatusCode.OK, view.StatusCode);
                Assert.Contains("<li class=\"active\"><a href=\"/regions\">Regions</a></li>", sut);
                Assert.Contains("<li><a href=\"/\">Home</a></li>", sut);
            }
        }

        [Fact]
        public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnHomeAnchorTag_AsControllerIsHomeAndActionIsIndex()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseStaticFiles();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}");
                });
            }, services =>
            {
                services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddAssemblyCacheBusting();
                services.AddControllersWithViews();
            }))
            {
                var client = filter.Host.GetTestClient();
                var view = await client.GetAsync("/");
                var sut = await view.Content.ReadAsStringAsync();

                TestOutput.WriteLine(sut);

                Assert.Equal(HttpStatusCode.OK, view.StatusCode);
                Assert.Contains("<li><a href=\"/regions\">Regions</a></li>", sut);
                Assert.Contains("<li class=\"active\"><a href=\"/\">Home</a></li>", sut);
            }
        }

        [Fact]
        public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnRegionsAnchorTag_AsPerPageStructure()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseStaticFiles();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                });
            }, services =>
            {
                services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddAssemblyCacheBusting();
                services.AddRazorPages(o =>
                {
                    o.Conventions.AddPageRoute("/Regions/Index", "regions");
                    o.Conventions.AddPageRoute("/Regions/CultureCollection", "regions/{regionName}/{regionDisplayName}");
                });
            }))
            {
                var client = filter.Host.GetTestClient();
                var page = await client.GetAsync("/regions/da-dk/denmark");
                var sut = await page.Content.ReadAsStringAsync();

                TestOutput.WriteLine(sut);

                Assert.Equal(HttpStatusCode.OK, page.StatusCode);
                Assert.Contains("<li class=\"active\"><a>Regions</a></li>", sut);
                Assert.Contains("<li><a>Home</a></li>", sut);
            }
        }

        [Fact]
        public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnHomeAnchorTag_AsPerPageStructure()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseStaticFiles();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                });
            }, services =>
            {
                services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddAssemblyCacheBusting();
                services.AddRazorPages(o =>
                {
                    o.Conventions.AddPageRoute("/", "/");
                    o.Conventions.AddPageRoute("/Regions/Index", "regions");
                    o.Conventions.AddPageRoute("/Regions/CultureCollection", "/regions/{regionName}/{regionDisplayName}");
                });
            }))
            {
                var client = filter.Host.GetTestClient();
                var page = await client.GetAsync("/");
                var sut = await page.Content.ReadAsStringAsync();

                TestOutput.WriteLine(sut);

                Assert.Equal(HttpStatusCode.OK, page.StatusCode);
                Assert.Contains("<li><a>Regions</a></li>", sut);
                Assert.Contains("<li class=\"active\"><a>Home</a></li>", sut);
            }
        }
    }
}