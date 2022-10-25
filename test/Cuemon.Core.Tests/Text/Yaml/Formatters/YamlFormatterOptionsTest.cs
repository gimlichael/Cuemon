using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Text.Yaml.Formatters
{
    public class YamlFormatterOptionsTest : Test
    {
        public YamlFormatterOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void YamlFormatterOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
        {
            var sut1 = new YamlFormatterOptions()
            {
                Settings = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Settings == null')", sut2.Message);
            Assert.Equal("YamlFormatterOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void YamlFormatterOptions_ShouldHaveDefaultValues()
        {
            var sut = new YamlFormatterOptions();

            Assert.NotNull(sut.Settings);
            Assert.Equal(EncodingOptions.DefaultEncoding, sut.Encoding);
            Assert.Equal(EncodingOptions.DefaultPreambleSequence, sut.Preamble);
        }
    }
}
