using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Asp.Versioning;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Asp.Versioning.Assets;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Asp.Versioning
{
    public class ApiVersionReaderTest : Test
    {
        public ApiVersionReaderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetRequest_ShouldReturnOkay_AsWeAreFilteringAwayInvalidAcceptHeaders()
        {
            using (var app = WebApplicationTestFactory.Create(services =>
            {
                services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                    .AddJsonFormatters();
                services.AddRestfulApiVersioning();
            }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;v=1.0");
                var sut = await client.GetAsync("/fake/");

                TestOutput.WriteLine(sut.Content.Headers.ContentType.ToString());
                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith("application/json; charset=utf-8; v=1.0", sut.Content.Headers.ContentType.ToString());
                Assert.Equal(@"""Unit Test""", await sut.Content.ReadAsStringAsync(), ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task GetRequest_ShouldFailWitNotAcceptable_AsWeAreSpecifyingV2_WhenOnlyV1Exists()
        {
            using (var app = WebApplicationTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters();
                       services.AddRestfulApiVersioning();
                   }, app =>
                   {
                       app.UseFaultDescriptorExceptionHandler();
                       app.UseRestfulApiVersioning();
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;v=2.0");
                var sut = await client.GetAsync("/fake/");

                var stringResult = await sut.Content.ReadAsStringAsync();

                TestOutput.WriteLine(stringResult);

                Assert.Equal(HttpStatusCode.NotAcceptable, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.Equal(@"Error: 
  Status: 406
  Code: NotAcceptable
  Message: The HTTP resource that matches the request URI 'http://localhost/fake/' does not support the API version '2.0'.",stringResult, ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task PostRequest_ShouldFailWitUnsupportedMediaType_AsWeAreSpecifyingV2_WhenOnlyV1Exists()
        {
            using (var app = WebApplicationTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters();
                       services.AddRestfulApiVersioning();
                   }, app =>
                   {
                       app.UseFaultDescriptorExceptionHandler();
                       app.UseRestfulApiVersioning();
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;v=2.0");
                var sut = await client.PostAsync("/fake/", new StringContent(Generate.RandomString(100)));

                var stringResult = await sut.Content.ReadAsStringAsync();

                TestOutput.WriteLine(stringResult);

                Assert.Equal(HttpStatusCode.UnsupportedMediaType, sut.StatusCode);
                Assert.Equal(HttpMethod.Post, sut.RequestMessage.Method);
                Assert.Equal(@"Error: 
  Status: 415
  Code: UnsupportedMediaType
  Message: The HTTP resource that matches the request URI 'http://localhost/fake/' does not support the API version '2.0'.", stringResult, ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsJsonResponse_As_d3_IsAnUnknownVersion_CorreclySetOnApplicationJsonAccept()
        {
            using (var app = WebApplicationTestFactory.Create(services =>
            {
                services.AddControllers(o => o.Filters.AddFaultDescriptor())
                    .AddApplicationPart(typeof(FakeController).Assembly)
                    .AddJsonFormatters(o => o.Settings.Encoder = JavaScriptEncoder.Default);
                services.AddHttpContextAccessor();
                services.AddRestfulApiVersioning();
            }, app =>
                   { 
                       app.UseFaultDescriptorExceptionHandler(o =>
                       {
                           o.NonMvcResponseHandlers
                               .AddJsonResponseHandler(app.ApplicationServices.GetRequiredService<IOptions<JsonFormatterOptions>>())
                               .AddXmlResponseHandler(app.ApplicationServices.GetRequiredService<IOptions<XmlFormatterOptions>>());
                       });
                       app.UseRestfulApiVersioning();
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });

                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;q=10.0;v=d3");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith("application/json", sut.Content.Headers.ContentType.ToString());
                Assert.Equal(@"{
  ""error"": {
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The HTTP resource that matches the request URI \u0027http://localhost/fake/throw\u0027 does not support the API version \u0027d3\u0027.""
  }
}", await sut.Content.ReadAsStringAsync(), ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task GetRequest_ShouldThrowNotAcceptableException_As_2dot0_IsAnUnknownVersion()
        {
            using (var app = WebApplicationTestFactory.Create(services =>
            {
                services.AddControllers()
                    .AddApplicationPart(typeof(FakeController).Assembly);
                services.AddRestfulApiVersioning();
            }, app =>
                   {
                       app.UseRestfulApiVersioning();

                       app.UseRouting();

                       app.UseEndpoints(routes =>
                       {
                           routes.MapControllers();
                       });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/xml;v=2.0");

                await Assert.ThrowsAsync<NotAcceptableException>(() => client.GetAsync("/fake/"));
            }
        }

        [Fact]
        public async Task PostRequest_ShouldThrowUnsupportedMediaTypeException_As_2dot0_IsAnUnknownVersion()
        {
            using (var app = WebApplicationTestFactory.Create(services =>
                   {
                       services.AddControllers()
                           .AddApplicationPart(typeof(FakeController).Assembly);
                       services.AddRestfulApiVersioning();
                   }, app =>
                   {
                       app.UseRestfulApiVersioning();

                       app.UseRouting();

                       app.UseEndpoints(routes =>
                       {
                           routes.MapControllers();
                       });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/xml;v=2.0");

                await Assert.ThrowsAsync<UnsupportedMediaTypeException>(() => client.PostAsync("/fake/", new StringContent(Generate.RandomString(100))));
            }
        }
    }
}
