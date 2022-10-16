using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
            using (var app = WebApplicationTestFactory.Create(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                    .AddJsonFormatters();
                services.AddRestfulApiVersioning(o =>
                {
                    o.UseProblemDetailsFactory<DefaultProblemDetailsFactory>();
                });
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
        public async Task GetRequest_ShouldFailWithUnsupportedMediaType_AsWeAreSpecifyingV2_WhenOnlyV1Exists()
        {
            using (var app = WebApplicationTestFactory.Create(app =>
                   {
                       app.UseFaultDescriptorExceptionHandler();
                       app.UseRestfulApiVersioning();
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }, services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.UseProblemDetailsFactory<DefaultProblemDetailsFactory>();
                       });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;v=2.0");
                var sut = await client.GetAsync("/fake/");

                Assert.Equal(HttpStatusCode.UnsupportedMediaType, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.Equal(@"Error: 
  Status: 415
  Code: UnsupportedMediaType
  Message: The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.
", await sut.Content.ReadAsStringAsync(), ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsJsonResponse_As_d3_IsAnUnknownVersion_CorreclySetOnApplicationJsonAccept()
        {
            using (var app = WebApplicationTestFactory.Create(app =>
            {
                app.UseFaultDescriptorExceptionHandler(o =>
                {
                    o.NonMvcResponseHandler = (context, descriptor) =>
                    {
                        return new List<HttpExceptionDescriptorResponseHandler>()
                            .AddJsonResponseHandler(descriptor, Patterns.ConfigureRevertExchange<JsonFormatterOptions, ExceptionDescriptorOptions>(context.RequestServices.GetService<IOptions<JsonFormatterOptions>>()?.Value ?? new JsonFormatterOptions(), (s, r) =>
                            {
                                r.IncludeStackTrace = s.IncludeExceptionStackTrace;
                                r.IncludeEvidence = s.IncludeExceptionDescriptorEvidence;
                                r.IncludeFailure = s.IncludeExceptionDescriptorFailure;
                            }))
                            .AddXmlResponseHandler(descriptor, Patterns.ConfigureRevertExchange<XmlFormatterOptions, ExceptionDescriptorOptions>(context.RequestServices.GetService<IOptions<XmlFormatterOptions>>()?.Value ?? new XmlFormatterOptions(), (s, r) =>
                            {
                                r.IncludeStackTrace = s.IncludeExceptionStackTrace;
                                r.IncludeEvidence = s.IncludeExceptionDescriptorEvidence;
                                r.IncludeFailure = s.IncludeExceptionDescriptorFailure;
                            }));
                    };
                });
                app.UseRestfulApiVersioning();
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });

            }, services =>
            {
                services.AddControllers(o => o.Filters.AddFaultDescriptor())
                    .AddApplicationPart(typeof(FakeController).Assembly)
                    .AddJsonFormatters();
                services.AddHttpContextAccessor();
                services.AddRestfulApiVersioning();
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
        public async Task GetRequest_ShouldThrowUnsupportedMediaTypeException_As_2dot0_IsAnUnknownVersion()
        {
            using (var app = WebApplicationTestFactory.Create(app =>
            {
                app.UseRestfulApiVersioning();

                app.UseRouting();

                app.UseEndpoints(routes =>
                {
                    routes.MapControllers();
                });
            }, services =>
            {
                services.AddControllers()
                    .AddApplicationPart(typeof(FakeController).Assembly);
                services.AddRestfulApiVersioning();
            }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/xml;v=2.0");

                await Assert.ThrowsAsync<UnsupportedMediaTypeException>(() => client.GetAsync("/fake/"));
            }
        }

        [Fact]
        public async Task GetRequest_ShouldFailSilentlyAsOriginallyIntendedByApiVersionAuthor()
        {
            using (var app = WebApplicationTestFactory.Create(app =>
                   {
                       app.UseRouting();

                       app.UseEndpoints(routes =>
                       {
                           routes.MapControllers();
                       });
                   }, services =>
                   {
                       services.AddControllers()
                           .AddApplicationPart(typeof(FakeController).Assembly);
                       services.AddRestfulApiVersioning();
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/xml;v=2.0");

                var sut = await client.GetAsync("/fake/");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.UnsupportedMediaType, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.Empty(await sut.Content.ReadAsStringAsync());
            }
        }
    }
}
