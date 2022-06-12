using System.Collections.Generic;
using System.Text.Json.Serialization;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Text.Json.Formatters
{
    public class JsonFormatterOptionsTest : Test
    {
        public JsonFormatterOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DefaultConverters_ShouldHaveSameAmountOfDefaultConverters()
        {
            var defaultConverters = new List<JsonConverter>();
            JsonFormatterOptions.DefaultConverters(defaultConverters);

            var x = new JsonFormatterOptions();
            var y = new JsonFormatterOptions();
            var bootstrapInvocationList = JsonFormatterOptions.DefaultConverters.GetInvocationList().Length;

            Assert.Equal(4, defaultConverters.Count);
            Assert.Equal(1, bootstrapInvocationList);
            Assert.Equal(2, x.Settings.Converters.Count - defaultConverters.Count);
            Assert.Equal(2, y.Settings.Converters.Count - defaultConverters.Count);

            Assert.Equal(x.Settings.Converters.Count, y.Settings.Converters.Count);
        }
    }
}
