using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Http;
using Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    public class ServiceProviderExtensionsTest : Test
    {
        public ServiceProviderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetExceptionResponseFormatters_ShouldGetAllRegisteredServicesOf_IExceptionResponseFormatter()
        {
            var services = new ServiceCollection();
            
            services.AddOptions();
            services.AddXmlExceptionResponseFormatter();
            services.AddJsonExceptionResponseFormatter();
            services.AddNewtonsoftJsonExceptionResponseFormatter();
            services.AddYamlExceptionResponseFormatter();

            var serviceProvider = services.BuildServiceProvider();

            var formatters = serviceProvider.GetExceptionResponseFormatters().ToList();

            var formattersAndResponseHandlers = formatters.SelectMany(formatter => formatter.ExceptionDescriptorHandlers.Select(handler => $"{formatter.GetType().GenericTypeArguments[0].Name} -> {handler.ContentType}")).ToList();

            TestOutput.WriteLine(formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));

            Assert.Equal(11, formattersAndResponseHandlers.Count);
            Assert.Equal("""
                         XmlFormatterOptions -> application/xml
                         XmlFormatterOptions -> text/xml
                         JsonFormatterOptions -> application/json
                         JsonFormatterOptions -> text/json
                         NewtonsoftJsonFormatterOptions -> application/json
                         NewtonsoftJsonFormatterOptions -> text/json
                         YamlFormatterOptions -> text/plain; charset=utf-8
                         YamlFormatterOptions -> text/plain
                         YamlFormatterOptions -> application/yaml
                         YamlFormatterOptions -> text/yaml
                         YamlFormatterOptions -> */*
                         """.ReplaceLineEndings(), formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));
        }
    }
}
