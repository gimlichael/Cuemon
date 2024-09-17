using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class DecimalPrefixTest : Test
    {
        public DecimalPrefixTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DecimalPrefix_ShouldVerifyMultiplePrefixConstants()
        {
            Assert.Equal(DecimalPrefix.Deca.Multiplier, 10d);
            Assert.Equal(DecimalPrefix.Hecto.Multiplier, 100d);
            Assert.Equal(DecimalPrefix.Kilo.Multiplier, 1000d);
            Assert.Equal(DecimalPrefix.Mega.Multiplier, 1000000d);
            Assert.Equal(DecimalPrefix.Giga.Multiplier, 1000000000d);
            Assert.Equal(DecimalPrefix.Tera.Multiplier, 1000000000000d);
            Assert.Equal(DecimalPrefix.Peta.Multiplier, 1000000000000000d);
            Assert.Equal(DecimalPrefix.Exa.Multiplier, 1000000000000000000d);
            Assert.Equal(DecimalPrefix.Zetta.Multiplier, 1000000000000000000000d);
            Assert.Equal(DecimalPrefix.Yotta.Multiplier, 1000000000000000000000000d);
        }
    }
}