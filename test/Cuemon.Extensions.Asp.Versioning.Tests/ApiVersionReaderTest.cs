using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Asp.Versioning.Assets;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
            using (var app = WebHostTestFactory.Create(services =>
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
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
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
                Assert.Equal("The HTTP resource that matches the request URI 'http://localhost/fake/' does not support the API version '2.0'.", stringResult, ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public async Task PostRequest_ShouldFailWitUnsupportedMediaType_AsWeAreSpecifyingV2_WhenOnlyV1Exists()
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
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
                Assert.Equal("The HTTP resource that matches the request URI 'http://localhost/fake/' does not support the API version '2.0'.", stringResult, ignoreLineEndingDifferences: true);
            }
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("text/json")]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsJsonResponse_As_d3_IsAnUnknownVersion_CorrectlySetOnJsonAccept(string jsonAccept)
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions();
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters(o => o.Settings.Encoder = JavaScriptEncoder.Default);
                       services.AddHttpContextAccessor();
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
                client.DefaultRequestHeaders.Add("Accept", $"text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,{jsonAccept};q=10.0;v=d3");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith(jsonAccept, sut.Content.Headers.ContentType.ToString());
                Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/fake/throw"",
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The HTTP resource that matches the request URI \u0027http://localhost/fake/throw\u0027 does not support the API version \u0027d3\u0027.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), (await sut.Content.ReadAsStringAsync()).ReplaceLineEndings()));
            }
        }

        [Theory]
        [InlineData("application/xml")]
        [InlineData("text/xml")]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsXmlResponse_As_d3_IsAnUnknownVersion_CorrectlySetOnXmlAccept(string xmlAccept)
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions();
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters(o => o.Settings.Encoder = JavaScriptEncoder.Default)
                           .AddXmlFormatters();
                       services.AddHttpContextAccessor();
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
                client.DefaultRequestHeaders.Add("Accept", $"application/json,text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,{xmlAccept};q=10.0;v=d3");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith(xmlAccept, sut.Content.Headers.ContentType.ToString());
                Assert.True(Match(@"<?xml version=""1.0"" encoding=""utf-8""?><HttpExceptionDescriptor><Error><Instance>http://localhost/fake/throw</Instance><Status>400</Status><Code>BadRequest</Code><Message>The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'd3'.</Message></Error><TraceId>*</TraceId></HttpExceptionDescriptor>", await sut.Content.ReadAsStringAsync()));
            }
        }

        [Theory]
        [InlineData("text/plain")]
        [InlineData("text/yaml")]
        [InlineData("application/yaml")]
        [InlineData("*/*")]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsXmlResponse_As_d3_IsAnUnknownVersion_CorrectlySetOnYamlAccept(string yamlAccept)
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions();
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters()
                           .AddXmlFormatters()
                           .AddYamlFormatters();
                       services.AddHttpContextAccessor();
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
                client.DefaultRequestHeaders.Add("Accept", $"application/json,text/html,application/xhtml+xml,image/avif,image/webp,image/apng,application/signed-exchange;v=b3;q=0.9,{yamlAccept};q=10.0;v=d3");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith(yamlAccept, sut.Content.Headers.ContentType.ToString());
                Assert.Equal("""
                             Error:
                               Status: 400
                               Code: BadRequest
                               Message: The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'd3'.

                             """, await sut.Content.ReadAsStringAsync(), ignoreLineEndingDifferences: true);
            }
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("text/json")]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsNewtonsoftJsonResponse_As_d3_IsAnUnknownVersion_CorrectlySetOnJsonAccept(string jsonAccept)
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.Evidence);
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly)
                           .AddNewtonsoftJsonFormatters(o => o.SensitivityDetails = FaultSensitivityDetails.Evidence);
                       services.AddHttpContextAccessor();
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
                client.DefaultRequestHeaders.Add("Accept", $"text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,{jsonAccept};q=10.0;v=d3");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith(jsonAccept, sut.Content.Headers.ContentType.ToString());
                Assert.True(Match($$"""
                             {
                               "error": {
                                 "instance": "http://localhost/fake/throw",
                                 "status": 400,
                                 "code": "BadRequest",
                                 "message": "The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'd3'."
                               },
                               "evidence": {
                                 "request": {
                                   "location": "http://localhost/fake/throw",
                                   "method": "GET",
                                   "headers": {
                                     "accept": [
                                       "text/html",
                                       "application/xhtml+xml",
                                       "image/avif",
                                       "image/webp",
                                       "image/apng",
                                       "*/*; q=0.8",
                                       "application/signed-exchange; v=b3; q=0.9",
                                       "{{jsonAccept}}; q=10.0; v=d3"
                                     ],
                                     "host": "localhost"
                                   },
                                   "query": [],
                                   "cookies": [],
                                   "body": ""
                                 }
                               },
                               "traceId": "*"
                             }
                             """.ReplaceLineEndings(), (await sut.Content.ReadAsStringAsync()).ReplaceLineEndings()));
            }
        }

        [Fact]
        public async Task GetRequest_ShouldThrowNotAcceptableException_As_2dot0_IsAnUnknownVersion()
        {
            using (var app = WebHostTestFactory.Create(services =>
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
            using (var app = WebHostTestFactory.Create(services =>
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
