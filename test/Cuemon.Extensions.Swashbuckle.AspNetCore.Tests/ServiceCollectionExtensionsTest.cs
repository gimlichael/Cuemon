﻿using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Cuemon.Extensions.Asp.Versioning;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Swashbuckle.AspNetCore.Assets.V1;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AddRestfulSwagger_ShouldInclude_RestfulApiVersioning_and_AddRestfulSwagger_Defaults()
        {
            using (var filter = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters();
                       services.AddEndpointsApiExplorer();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.Conventions.Controller<FakeController>().HasApiVersion(new ApiVersion(1, 0));
                           o.Conventions.Controller<Assets.V2.FakeController>().HasApiVersion(new ApiVersion(2, 0));
                       });
                       services.AddRestfulSwagger();
                       services.AddSwaggerGen();
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                       app.UseSwagger();
                       app.UseSwaggerUI();
                   }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetStringAsync("/swagger/v1/swagger.json");

                TestOutput.WriteLine(result);

                Assert.Equal(@"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""API 1.0"",
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
            ""description"": ""OK""
          }
        }
      }
    }
  },
  ""components"": { }
}", result, ignoreLineEndingDifferences: true);

                result = await client.GetStringAsync("/swagger/v2/swagger.json");

                TestOutput.WriteLine(result);

                Assert.Equal(@"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""API 2.0"",
    ""version"": ""2.0""
  },
  ""paths"": {
    ""/Fake"": {
      ""get"": {
        ""tags"": [
          ""Fake""
        ],
        ""parameters"": [
          {
            ""name"": ""details"",
            ""in"": ""query"",
            ""schema"": {
              ""$ref"": ""#/components/schemas/FaultSensitivityDetails""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""OK""
          }
        }
      }
    }
  },
  ""components"": {
    ""schemas"": {
      ""FaultSensitivityDetails"": {
        ""enum"": [
          0,
          1,
          2,
          3,
          4,
          5,
          7,
          8,
          15
        ],
        ""type"": ""integer"",
        ""format"": ""int32""
      }
    }
  }
}", result, ignoreLineEndingDifferences: true);

            }
        }

        [Fact]
        public async Task AddRestfulSwagger_ShouldInclude_RestfulApiVersioning_and_AddRestfulSwagger_Configured()
        {
            using (var filter = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly)
                           .AddJsonFormatters();
                       services.AddEndpointsApiExplorer();
                       services.AddRestfulApiVersioning(o =>
                       {
                           o.Conventions.Controller<FakeController>().HasApiVersion(new ApiVersion(1, 0));
                           o.Conventions.Controller<Assets.V2.FakeController>().HasApiVersion(new ApiVersion(2, 0));
                       });
                       services.AddRestfulSwagger(o =>
                       {
                           o.OpenApiInfo.License = new OpenApiLicense()
                           {
                               Url = new Uri("https://github.com/codebeltnet/webapp-webapi-refapp/blob/main/LICENSE.md"),
                               Name = "MIT"
                           };
                           o.OpenApiInfo.Contact = new OpenApiContact()
                           {
                               Url = new Uri("https://github.com/gimlichael"),
                               Name = "gimlichael",
                               Email = "root@gimlichael.dev"
                           };
                           o.OpenApiInfo.Title = "ASP.NET Core Reference Web API";
                           o.OpenApiInfo.Description = "Sample ASP.NET Core Web App for Web API reference application, powered by Codebelt, based on the standard project generated by Microsoft.";
                           o.OpenApiInfo.TermsOfService = new Uri("https://docs.github.com/en/site-policy/github-terms/github-terms-of-service");
                           o.IncludeControllerXmlComments = true;
                           o.XmlDocumentations.AddByType(typeof(ServiceCollectionExtensionsTest));
                           o.JsonSerializerOptionsFactory = provider => provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value.Settings;
                       });
                       services.AddSwaggerGen();
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                       app.UseSwagger();
                       app.UseSwaggerUI();
                   }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetStringAsync("/swagger/v1/swagger.json");

                TestOutput.WriteLine(result);

                Assert.Equal(@"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""ASP.NET Core Reference Web API"",
    ""description"": ""Sample ASP.NET Core Web App for Web API reference application, powered by Codebelt, based on the standard project generated by Microsoft."",
    ""termsOfService"": ""https://docs.github.com/en/site-policy/github-terms/github-terms-of-service"",
    ""contact"": {
      ""name"": ""gimlichael"",
      ""url"": ""https://github.com/gimlichael"",
      ""email"": ""root@gimlichael.dev""
    },
    ""license"": {
      ""name"": ""MIT"",
      ""url"": ""https://github.com/codebeltnet/webapp-webapi-refapp/blob/main/LICENSE.md""
    },
    ""version"": ""1.0""
  },
  ""paths"": {
    ""/Fake"": {
      ""get"": {
        ""tags"": [
          ""Fake""
        ],
        ""summary"": ""Gets an OK response with a body of Unit Test V1."",
        ""responses"": {
          ""200"": {
            ""description"": ""OK""
          }
        }
      }
    }
  },
  ""components"": { }
}", result, ignoreLineEndingDifferences: true);

                result = await client.GetStringAsync("/swagger/v2/swagger.json");

                TestOutput.WriteLine(result);

                Assert.Equal(@"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""ASP.NET Core Reference Web API"",
    ""description"": ""Sample ASP.NET Core Web App for Web API reference application, powered by Codebelt, based on the standard project generated by Microsoft."",
    ""termsOfService"": ""https://docs.github.com/en/site-policy/github-terms/github-terms-of-service"",
    ""contact"": {
      ""name"": ""gimlichael"",
      ""url"": ""https://github.com/gimlichael"",
      ""email"": ""root@gimlichael.dev""
    },
    ""license"": {
      ""name"": ""MIT"",
      ""url"": ""https://github.com/codebeltnet/webapp-webapi-refapp/blob/main/LICENSE.md""
    },
    ""version"": ""2.0""
  },
  ""paths"": {
    ""/Fake"": {
      ""get"": {
        ""tags"": [
          ""Fake""
        ],
        ""summary"": ""Gets an OK response with a body of Unit Test V2."",
        ""parameters"": [
          {
            ""name"": ""details"",
            ""in"": ""query"",
            ""schema"": {
              ""$ref"": ""#/components/schemas/FaultSensitivityDetails""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""OK""
          }
        }
      }
    }
  },
  ""components"": {
    ""schemas"": {
      ""FaultSensitivityDetails"": {
        ""enum"": [
          [
            ""none""
          ],
          [
            ""failure""
          ],
          [
            ""stackTrace""
          ],
          [
            ""failureWithStackTrace""
          ],
          [
            ""data""
          ],
          [
            ""failureWithData""
          ],
          [
            ""failureWithStackTraceAndData""
          ],
          [
            ""evidence""
          ],
          [
            ""all""
          ]
        ],
        ""type"": ""integer"",
        ""format"": ""int32""
      }
    }
  }
}", result, ignoreLineEndingDifferences: true);

            }
        }
    }
}
