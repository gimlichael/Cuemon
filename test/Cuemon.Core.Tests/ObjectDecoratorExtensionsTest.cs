using System;
using System.Globalization;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core
{
    public class ObjectDecoratorExtensionsTest : Test
    {
        private readonly string _number = $"{Generate.RandomString(5, Alphanumeric.Numbers)},{Generate.RandomNumber(0, 99):D2}";

        public ObjectDecoratorExtensionsTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void ChangeType_ShouldConvertObjectToDesiredTargetAndFormat()
        {
            var culture = CultureInfo.GetCultureInfo("da-DK");
            var result = Decorator.Enclose(_number).ChangeType<double>(o => o.FormatProvider = culture);
            Assert.IsType<double>(result);
            Assert.Equal(Convert.ToDouble(_number, culture), result);
            TestOutput.WriteLine(result.ToString(culture));
        }

        [Fact]
        public void ChangeType_ShouldConvertNoneGenericObjectToDesiredTarget()
        {
            var result = Decorator.Enclose(_number).ChangeType(typeof(double));
            Assert.IsType<double>(result);
            Assert.Equal(Convert.ToDouble(_number, new ObjectFormattingOptions().FormatProvider), result);
            TestOutput.WriteLine(result.ToString());
        }

        [Fact]
        public void ChangeTypeOrDefault_ShouldFallbackToDefault()
        {
            var result = Decorator.Enclose(_number).ChangeTypeOrDefault<byte>(byte.MaxValue);
            Assert.IsType<byte>(result);
            Assert.Equal(byte.MaxValue, result);
            TestOutput.WriteLine(result.ToString());
        }
    }
}