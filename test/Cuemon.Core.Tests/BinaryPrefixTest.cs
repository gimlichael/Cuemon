using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class BinaryPrefixTest : Test
    {
        public BinaryPrefixTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BinaryPrefix_ShouldVerifyMultiplePrefixConstants()
        {
            Assert.Equal(BinaryPrefix.Kibi.Multiplier, 1024d);
            Assert.Equal(BinaryPrefix.Mebi.Multiplier, 1048576d);
            Assert.Equal(BinaryPrefix.Gibi.Multiplier, 1073741824d);
            Assert.Equal(BinaryPrefix.Tebi.Multiplier, 1099511627776d);
            Assert.Equal(BinaryPrefix.Pebi.Multiplier, 1125899906842624d);
            Assert.Equal(BinaryPrefix.Exbi.Multiplier, 1152921504606846976d);
            Assert.Equal(BinaryPrefix.Zebi.Multiplier, 1180591620717411303424d);
            Assert.Equal(BinaryPrefix.Yobi.Multiplier, 1208925819614629174706176d);
        }
    }
}