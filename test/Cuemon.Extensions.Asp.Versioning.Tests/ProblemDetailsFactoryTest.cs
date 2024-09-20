using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Asp.Versioning.Assets;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
//using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Extensions.DependencyInjection;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Asp.Versioning
{
    public class ProblemDetailsFactoryTest : Test
    {
        public ProblemDetailsFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

#if NET8_0_OR_GREATER
        [Fact]
        public async Task GetRequest_ShouldFailWithBadRequest_FormattedAsRfc7807_As_b3_IsAnUnknownVersion()
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.UseBuiltInRfc7807 = true;
                           o.ValidAcceptHeaders.Clear();
                       });
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                var sut = await client.GetAsync("/fake/");

                TestOutput.WriteLine(sut.Content.Headers.ContentType.ToString());
                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith("application/problem+json", sut.Content.Headers.ContentType.ToString());

#if NET9_0
                var expected = """{"type":"https://docs.api-versioning.org/problems#invalid","title":"Invalid API version","status":400,"detail":"The HTTP resource that matches the request URI 'http://localhost/fake/' does not support the API version 'b3'.","traceId":"*"}""";
                Assert.True(Match(expected, await sut.Content.ReadAsStringAsync()));
#else
                var expected = @"{""type"":""https://docs.api-versioning.org/problems#invalid"",""title"":""Invalid API version"",""status"":400,""detail"":""The HTTP resource that matches the request URI 'http://localhost/fake/' does not support the API version 'b3'.""}";
                Assert.Equal(expected, await sut.Content.ReadAsStringAsync());
#endif



                // sadly Microsoft does not use the formatter we feed into the pipeline .. they use their own horrid WriteJsonAsync implementation .. 
            }
        }
#endif

        [Fact]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsXmlResponse_As_b3_IsAnUnknownVersion()
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions();
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters()
                           .AddXmlFormatters();
                       services.AddHttpContextAccessor();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.ValidAcceptHeaders.Clear();
                       });
                   }, app =>
                          {
                              app.UseFaultDescriptorExceptionHandler();
                              app.UseRestfulApiVersioning();
                              app.UseRouting();
                              app.UseEndpoints(routes => { routes.MapControllers(); });

                          }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/xml;q=0.9");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith("application/xml", sut.Content.Headers.ContentType.ToString());
                Assert.True(Match(@"<?xml version=""1.0"" encoding=""utf-8""?><HttpExceptionDescriptor><Error><Instance>http://localhost/fake/throw</Instance><Status>400</Status><Code>BadRequest</Code><Message>The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'b3'.</Message></Error><TraceId>*</TraceId></HttpExceptionDescriptor>", await sut.Content.ReadAsStringAsync()));
            }
        }

        [Fact]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsJsonResponse_As_b3_IsAnUnknownVersion()
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions();
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters(o => o.Settings.Encoder = JavaScriptEncoder.Default);
                       services.AddHttpContextAccessor();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.ValidAcceptHeaders.Clear();
                       });
                   }, app =>
                   {
                       app.UseFaultDescriptorExceptionHandler();
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });

                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;q=10.0");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith("application/json", sut.Content.Headers.ContentType.ToString());
                Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/fake/throw"",
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The HTTP resource that matches the request URI \u0027http://localhost/fake/throw\u0027 does not support the API version \u0027b3\u0027.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), (await sut.Content.ReadAsStringAsync()).ReplaceLineEndings()));
            }
        }

        // TODO: MOVE TO aspversioning repo
        //        [Fact]
        //        public async Task GetRequest_ShouldFailWithBadRequestFormattedAsYamlResponse_As_b3_IsAnUnknownVersion()
        //        {
        //            using (var app = WebHostTestFactory.Create(services =>
        //                   {
        //                       services.AddFaultDescriptorOptions();
        //                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
        //                           .AddApplicationPart(typeof(FakeController).Assembly)
        //                           .AddYamlFormatters();
        //                       services.AddHttpContextAccessor()
        //                           .AddRestfulApiVersioning(o =>
        //                           {
        //                               o.ValidAcceptHeaders.Clear();
        //                           })
        //                           .PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All);
        //                   }, app =>
        //                          {
        //                              app.UseFaultDescriptorExceptionHandler();
        //                              app.UseRouting();
        //                              app.UseEndpoints(routes => { routes.MapControllers(); });

        //                          }))
        //            {
        //                var client = app.Host.GetTestClient();
        //                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;q=10.0");
        //                var sut = await client.GetAsync("/fake/throw");

        //                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

        //                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
        //                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
        //                Assert.EndsWith("*/*", sut.Content.Headers.ContentType.ToString());
        //                Assert.StartsWith(@"Error:
        //  Status: 400
        //  Code: BadRequest
        //  Message: The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'b3'.
        //  Failure:
        //    Type: Cuemon.AspNetCore.Http.BadRequestException
        //    Source: Cuemon.Extensions.Asp.Versioning
        //    Message: The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'b3'.
        //    Stack:
        //".ReplaceLineEndings(), await sut.Content.ReadAsStringAsync());
        //                Assert.EndsWith(@"
        //Evidence:
        //  Request:
        //    Location: http://localhost/fake/throw
        //    Method: GET
        //    Headers:
        //      Accept:
        //        - text/html
        //        - application/xhtml+xml
        //        - image/avif
        //        - image/webp
        //        - image/apng
        //        - '*/*; q=0.8'
        //        - application/signed-exchange; v=b3; q=0.9
        //        - application/json; q=10.0
        //      Host:
        //        - localhost
        //    Query: []
        //    Form: 
        //    Cookies: []
        //    Body: ''
        //".ReplaceLineEndings(), await sut.Content.ReadAsStringAsync());
        //            }
        //        }

        [Fact]
        public async Task GetRequest_ShouldFailWithBadRequestFormattedWithFallbackResponse_As_b3_IsAnUnknownVersion()
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddFaultDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.All);
                       services.AddControllers(o => o.Filters.AddFaultDescriptor())
                           .AddApplicationPart(typeof(FakeController).Assembly);
                       services.AddHttpContextAccessor();
                       services.AddRestfulApiVersioning(o => o.ValidAcceptHeaders.Clear());
                   }, app =>
                          {
                              app.UseFaultDescriptorExceptionHandler();
                              app.UseRouting();
                              app.UseEndpoints(routes => { routes.MapControllers(); });

                          }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9,application/json;q=10.0");
                var sut = await client.GetAsync("/fake/throw");

                TestOutput.WriteLine(await sut.Content.ReadAsStringAsync());

                Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
                Assert.Equal(HttpMethod.Get, sut.RequestMessage.Method);
                Assert.EndsWith("text/plain", sut.Content.Headers.ContentType.ToString());
                Assert.StartsWith("Cuemon.AspNetCore.Http.BadRequestException: The HTTP resource that matches the request URI 'http://localhost/fake/throw' does not support the API version 'b3'.".ReplaceLineEndings(), await sut.Content.ReadAsStringAsync());
                Assert.Contains(@"Additional Information:
	Headers: Microsoft.AspNetCore.Http.HeaderDictionary
	StatusCode: 400
	ReasonPhrase: Bad Request".ReplaceLineEndings(), await sut.Content.ReadAsStringAsync());
            }
        }

        [Fact]
        public async Task GetRequest_ShouldThrowABadRequestException_As_b3_IsAnUnknownVersion()
        {
            using (var app = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.ValidAcceptHeaders.Clear();
                       });
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var client = app.Host.GetTestClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                await Assert.ThrowsAsync<BadRequestException>(() => client.GetAsync("/fake/"));
            }
        }
    }
}
