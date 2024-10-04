using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Codebelt.Extensions.Xunit;
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

            var serviceProvider = services.BuildServiceProvider();

            var formatters = serviceProvider.GetExceptionResponseFormatters().ToList();

            var formattersAndResponseHandlers = formatters.SelectMany(formatter => formatter.ExceptionDescriptorHandlers.Select(handler => $"{formatter.GetType().GenericTypeArguments[0].Name} -> {handler.ContentType}")).ToList();

            TestOutput.WriteLine(formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));

            Assert.Equal(6, formattersAndResponseHandlers.Count);
            Assert.Equal("""
                         XmlFormatterOptions -> application/xml
                         XmlFormatterOptions -> text/xml
                         XmlFormatterOptions -> application/problem+xml
                         JsonFormatterOptions -> application/json
                         JsonFormatterOptions -> text/json
                         JsonFormatterOptions -> application/problem+json
                         """.ReplaceLineEndings(), formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));
        }
    }
}
