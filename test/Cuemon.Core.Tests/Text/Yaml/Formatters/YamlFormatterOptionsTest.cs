using System;
using System.Linq;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
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
            Assert.StartsWith("YamlFormatterOptions are not in a valid state.", sut3.Message);
            Assert.Contains("sut1", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void YamlFormatterOptions_SupportedMediaTypesIsNull_ShouldThrowInvalidOperationException()
        {
	        var sut1 = new YamlFormatterOptions()
	        {
		        SupportedMediaTypes = null
	        };
	        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
	        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

	        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SupportedMediaTypes == null')", sut2.Message);
	        Assert.StartsWith("YamlFormatterOptions are not in a valid state.", sut3.Message);
	        Assert.Contains("sut1", sut3.Message);
	        Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void YamlFormatterOptions_ShouldHaveDefaultValues()
        {
            var sut = new YamlFormatterOptions();
            var sutType = sut.GetType();

            Assert.NotNull(sut.Settings);
            Assert.NotNull(sut.SupportedMediaTypes);
            Assert.Equal(EncodingOptions.DefaultEncoding, sut.Encoding);
            Assert.Equal(EncodingOptions.DefaultPreambleSequence, sut.Preamble);
            Assert.Equal(FaultSensitivityDetails.None, sut.SensitivityDetails);
            Assert.True(sut.Settings.Converters.Count == 0, "sut.Settings.Converters.Count == 0");
            Assert.Equal(YamlFormatterOptions.DefaultMediaType, sut.SupportedMediaTypes.First());

            sutType.GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything)!.Invoke(sut, null);

            Assert.True(sut.Settings.Converters.Count == 2, "sut.Settings.Converters.Count == 2");
        }
    }
}
