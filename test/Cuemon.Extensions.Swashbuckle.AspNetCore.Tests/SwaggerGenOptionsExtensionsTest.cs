using System.Threading.Tasks;
using Asp.Versioning;
using Cuemon.Extensions.Asp.Versioning;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Swashbuckle.AspNetCore.Assets.V1;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    public class SwaggerGenOptionsExtensionsTest : Test
    {
        public SwaggerGenOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AddUserAgent_ShouldIncludeUserAgentDefaults()
        {
            using (var filter = WebApplicationTestFactory.Create(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
                app.UseSwagger();
                app.UseSwaggerUI();
            }, services =>
            {
                services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                    .AddJsonFormatters(o => o.IncludeExceptionDescriptorFailure = false);
                services.AddEndpointsApiExplorer();
                services.AddRestfulApiVersioning(o =>
                {
                    o.Conventions.Controller<FakeController>().HasApiVersion(new ApiVersion(1, 0));
                    o.Conventions.Controller<Assets.V2.FakeController>().HasApiVersion(new ApiVersion(2, 0));
                });
                services.AddSwaggerGen(o =>
                {
                    o.AddUserAgent();
                });
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetStringAsync("/swagger/v1/swagger.json");

                TestOutput.WriteLine(result);

              
                Assert.Equal(@"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""Cuemon.Extensions.Swashbuckle.AspNetCore.Tests"",
    ""version"": ""1.0""
  },
  ""paths"": {
    ""/Fake"": {
      ""get"": {
        ""tags"": [
          ""Fake""
        ],
        ""parameters"": [
          {
            ""$ref"": ""#/components/parameters/User-Agent""
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""Success""
          }
        }
      }
    }
  },
  ""components"": {
    ""parameters"": {
      ""User-Agent"": {
        ""name"": ""User-Agent"",
        ""in"": ""header"",
        ""description"": ""The identifier of the calling client."",
        ""style"": ""simple"",
        ""example"": ""Your-Awesome-Client/1.0.0""
      }
    }
  }
}", result, ignoreLineEndingDifferences: true);

            }
        }

        [Fact]
        public async Task AddJwtBearerSecurity_ShouldIncludeJwtBearerSecurityDefaults()
        {
            using (var filter = WebApplicationTestFactory.Create(app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                       app.UseSwagger();
                       app.UseSwaggerUI();
                   }, services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters(o => o.IncludeExceptionDescriptorFailure = false);
                       services.AddEndpointsApiExplorer();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.Conventions.Controller<FakeController>().HasApiVersion(new ApiVersion(1, 0));
                           o.Conventions.Controller<Assets.V2.FakeController>().HasApiVersion(new ApiVersion(2, 0));
                       });
                       services.AddSwaggerGen(o =>
                       {
                           o.AddJwtBearerSecurity();
                       });
                   }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetStringAsync("/swagger/v1/swagger.json");

                TestOutput.WriteLine(result);


                Assert.Equal(@"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""Cuemon.Extensions.Swashbuckle.AspNetCore.Tests"",
    ""version"": ""1.0""
  },
  ""paths"": {
    ""/Fake"": {
      ""get"": {
        ""tags"": [
          ""Fake""
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""Success""
          }
        }
      }
    }
  },
  ""components"": {
    ""securitySchemes"": {
      ""Bearer"": {
        ""type"": ""http"",
        ""description"": ""Protects an API by adding an Authorization header using the Bearer scheme in JWT format."",
        ""scheme"": ""bearer"",
        ""bearerFormat"": ""JWT""
      }
    }
  },
  ""security"": [
    {
      ""Bearer"": [ ]
    }
  ]
}", result, ignoreLineEndingDifferences: true);

            }
        }
    }
}
