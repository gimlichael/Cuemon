using System.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Cuemon.Xml.Serialization.Converters;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Xml.Serialization.Formatters
{
    public class XmlFormatterOptionsTest : Test
    {
        public XmlFormatterOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DefaultConverters_ShouldHaveSameAmountOfDefaultConverters()
        {
            var defaultConverters = new List<XmlConverter>();
            XmlFormatterOptions.DefaultConverters(defaultConverters);

            var x = new XmlFormatterOptions();
            var y = new XmlFormatterOptions();
            var bootstrapInvocationList = XmlFormatterOptions.DefaultConverters.GetInvocationList().Length;
            
            Assert.Equal(5, defaultConverters.Count);
            Assert.Equal(1, bootstrapInvocationList);
            Assert.Equal(2, x.Settings.Converters.Count - defaultConverters.Count);
            Assert.Equal(2, y.Settings.Converters.Count - defaultConverters.Count);

            Assert.Equal(x.Settings.Converters.Count, y.Settings.Converters.Count);
        }
    }
}