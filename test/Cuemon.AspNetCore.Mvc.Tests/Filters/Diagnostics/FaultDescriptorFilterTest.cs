using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    public class FaultDescriptorFilterTest : Test
    {
        public FaultDescriptorFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task OnException_ShouldIncludeFailure_DifferentiateOnUseBaseException(bool useBaseException)
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.Configure<MvcFaultDescriptorOptions>(o =>
                {
                    o.IncludeFailure = true; // TODO: find a way make FaultDescriptorOptions enough in terms of Exception controlling (e.g., do not rely on AddNewtonsoftJsonFormatters .. -> .. IncludeExceptionDescriptorFailure
                    o.UseBaseException = useBaseException;
                });
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormatters(o => o.IncludeExceptionDescriptorFailure = true);
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/400");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Condition.FlipFlop(useBaseException,
                    () => Assert.Equal(@"{
  ""error"": {
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""Value cannot be null."",
    ""failure"": {
      ""type"": ""System.ArgumentNullException"",
      ""message"": ""Value cannot be null.""
    }
  }
}", body),
                    () => Assert.Equal(@"{
  ""error"": {
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The request could not be understood by the server due to malformed syntax."",
    ""failure"": {
      ""type"": ""Cuemon.AspNetCore.Http.BadRequestException"",
      ""source"": ""Cuemon.AspNetCore.Mvc.Tests"",
      ""message"": ""The request could not be understood by the server due to malformed syntax."",
      ""statusCode"": 400,
      ""reasonPhrase"": ""Bad Request"",
      ""inner"": {
        ""type"": ""System.ArgumentNullException"",
        ""message"": ""Value cannot be null.""
      }
    }
  }
}", body));

                Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureUserAgentException_BadRequestMessage()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }, (context, services) =>
                   {
                       services.AddControllers(o =>
                           {
                               o.Filters.AddFaultDescriptor();
                               o.Filters.AddUserAgentSentinel();
                           }).AddApplicationPart(typeof(FakeController).Assembly)
                           .AddNewtonsoftJson()
                           .AddNewtonsoftJsonFormattersOptions(o =>
                           {
                               o.IncludeExceptionDescriptorFailure = false;
                           })
                           .AddNewtonsoftJsonFormatters();
                       services.Configure<UserAgentSentinelOptions>(o => o.RequireUserAgentHeader = true);
                   }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/fake/it");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The requirements of the request was not met.""
  }
}", body);
                Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(400), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureThrottlingException_TooManyRequests()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }, (context, services) =>
                   {
                       services.AddControllers(o =>
                           {
                               o.Filters.AddFaultDescriptor();
                               o.Filters.AddThrottlingSentinel();
                           }).AddApplicationPart(typeof(FakeController).Assembly)
                           .AddNewtonsoftJson()
                           .AddNewtonsoftJsonFormattersOptions(o =>
                           {
                               o.IncludeExceptionDescriptorFailure = false;
                           })
                           .AddNewtonsoftJsonFormatters();
                       services.Configure<ThrottlingSentinelOptions>(o =>
                       {
                           o.ContextResolver = _ => "dummy";
                           o.Quota = new ThrottleQuota(0, TimeSpan.Zero);
                       });
                       services.AddMemoryThrottlingCache();
                   }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/fake/it");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 429,
    ""code"": ""TooManyRequests"",
    ""message"": ""Throttling rate limit quota violation. Quota limit exceeded.""
  }
}", body);
                Assert.Equal(StatusCodes.Status429TooManyRequests, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(429), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureBadRequest()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o =>
                    {
                        o.IncludeExceptionDescriptorFailure = false;
                    })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/400");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The request could not be understood by the server due to malformed syntax.""
  }
}", body);
                Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(400), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureConflict()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o =>
                    {
                        o.IncludeExceptionDescriptorFailure = false;
                    })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/409");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 409,
    ""code"": ""Conflict"",
    ""message"": ""The request could not be completed due to a conflict with the current state of the resource.""
  }
}", body);
                Assert.Equal(StatusCodes.Status409Conflict, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(409), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureForbidden()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o =>
                    {
                        o.IncludeExceptionDescriptorFailure = false;
                    })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/403");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 403,
    ""code"": ""Forbidden"",
    ""message"": ""The server understood the request, but is refusing to fulfill it.""
  }
}", body);
                Assert.Equal(StatusCodes.Status403Forbidden, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(403), result.ReasonPhrase);
            }
        }
        
        [Fact]
        public async Task OnException_ShouldCaptureGone()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o =>
                    {
                        o.IncludeExceptionDescriptorFailure = false;
                    })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/410");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 410,
    ""code"": ""Gone"",
    ""message"": ""The requested resource is no longer available at the server and no forwarding address is known.""
  }
}", body);
                Assert.Equal(StatusCodes.Status410Gone, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(410), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureNotFound()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o =>
                    {
                        o.IncludeExceptionDescriptorFailure = false;
                    })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/404");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 404,
    ""code"": ""NotFound"",
    ""message"": ""The server has not found anything matching the request URI.""
  }
}", body);
                Assert.Equal(StatusCodes.Status404NotFound, (int)result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(404), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCapturePayloadTooLarge()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/413");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 413,
    ""code"": ""PayloadTooLarge"",
    ""message"": ""The server is refusing to process a request because the request entity is larger than the server is willing or able to process.""
  }
}", body);
                Assert.Equal(StatusCodes.Status413PayloadTooLarge, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(413), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCapturePreconditionFailed()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/412");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 412,
    ""code"": ""PreconditionFailed"",
    ""message"": ""The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.""
  }
}", body);
                Assert.Equal(StatusCodes.Status412PreconditionFailed, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(412), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCapturePreconditionRequired()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/428");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 428,
    ""code"": ""PreconditionRequired"",
    ""message"": ""No conditional request-header fields was supplied to the server.""
  }
}", body);
                Assert.Equal(StatusCodes.Status428PreconditionRequired, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(428), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureTooManyRequests()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/429");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 429,
    ""code"": ""TooManyRequests"",
    ""message"": ""The allowed number of requests has been exceeded.""
  }
}", body);
                Assert.Equal(StatusCodes.Status429TooManyRequests, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(429), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureUnauthorized()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/401");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 401,
    ""code"": ""Unauthorized"",
    ""message"": ""The request requires user authentication.""
  }
}", body);
                Assert.Equal(StatusCodes.Status401Unauthorized, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(401), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureMethodNotAllowed()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/405");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 405,
    ""code"": ""MethodNotAllowed"",
    ""message"": ""The method specified in the request is not allowed for the resource identified by the request URI.""
  }
}", body);
                Assert.Equal(StatusCodes.Status405MethodNotAllowed, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(405), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureNotAcceptable()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/406");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 406,
    ""code"": ""NotAcceptable"",
    ""message"": ""The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.""
  }
}", body);
                Assert.Equal(StatusCodes.Status406NotAcceptable, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(406), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureGeneric()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/XXX");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Contains(@"{
  ""error"": {
    ""status"": 500,
    ""code"": ""InternalServerError"",
    ""message"": ""An unhandled exception was raised by ", body);
                Assert.Equal(StatusCodes.Status500InternalServerError, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(500), result.ReasonPhrase);
            }
        }

        [Fact]
        public async Task OnException_ShouldCaptureUnsupportedMediaType()
        {
            using (var filter = WebApplicationTestFactory.CreateWithHostBuilderContext((context, app) =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, (context, services) =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormattersOptions(o => { o.IncludeExceptionDescriptorFailure = false; })
                    .AddNewtonsoftJsonFormatters();
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/statuscodes/415");
                var body = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(body);

                Assert.Equal(@"{
  ""error"": {
    ""status"": 415,
    ""code"": ""UnsupportedMediaType"",
    ""message"": ""The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.""
  }
}", body);
                Assert.Equal(StatusCodes.Status415UnsupportedMediaType, (int) result.StatusCode);
                Assert.Equal(HttpStatusDescription.Get(415), result.ReasonPhrase);
            }
        }
    }
}