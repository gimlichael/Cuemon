using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class BitStorageCapacityTest : Test
    {
        public BitStorageCapacityTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitStorageCapacity_ShouldBeEqualWithAFactorOfEight()
        {
            var x = BitStorageCapacity.FromBytes(1000000000);
            var y = BitStorageCapacity.FromBits(8000000000);
            Assert.Equal(x, y);
        }

        [Fact]
        public void BitStorageCapacity_UseSymbol_ShouldConvertOneBillionBitsToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = BitStorageCapacity.FromBits(1000000000);

            Assert.Equal("0 Pib", x.Pebi.ToString());
            Assert.Equal("0 Tib", x.Tebi.ToString());
            Assert.Equal("0.93 Gib", x.Gibi.ToString());
            Assert.Equal("953.67 Mib", x.Mebi.ToString());
            Assert.Equal("976,562.5 Kib", x.Kibi.ToString());

            Assert.Equal("0 Pb", x.Peta.ToString());
            Assert.Equal("0 Tb", x.Tera.ToString());
            Assert.Equal("1 Gb", x.Giga.ToString());
            Assert.Equal("1,000 Mb", x.Mega.ToString());
            Assert.Equal("1,000,000 kb", x.Kilo.ToString());

            Assert.Equal("1,000,000,000 b", x.Unit.ToString());

            TestOutput.WriteLine(x.ToAggregateString());
        }

        [Fact]
        public void BitStorageCapacity_UseCompound_ShouldConvertOneBillionBitsToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = BitStorageCapacity.FromBits(1000000000, o => o.Style = NamingStyle.Compound);

            Assert.Equal("0 pebibit", x.Pebi.ToString());
            Assert.Equal("0 tebibit", x.Tebi.ToString());
            Assert.Equal("0.93 gibibit", x.Gibi.ToString());
            Assert.Equal("953.67 mebibit", x.Mebi.ToString());
            Assert.Equal("976,562.5 kibibit", x.Kibi.ToString());

            Assert.Equal("0 petabit", x.Peta.ToString());
            Assert.Equal("0 terabit", x.Tera.ToString());
            Assert.Equal("1 gigabit", x.Giga.ToString());
            Assert.Equal("1,000 megabit", x.Mega.ToString());
            Assert.Equal("1,000,000 kilobit", x.Kilo.ToString());

            Assert.Equal("1,000,000,000 bit", x.Unit.ToString());

            TestOutput.WriteLine(x.ToAggregateString());
        }

        public void BitStorageCapacity_UseCompound_ShouldConvertOneBillionBitsToBinaryAndMetricPrefixDoubleRepresentation()
        {
            var bs = BitStorageCapacity.FromBits(1000000000, o =>
            {
                o.Style = NamingStyle.Compound;
                o.Prefix = UnitPrefix.Decimal;
            });

            Assert.Equal(1000000000, (double)bs);
            Assert.Equal(1000000, bs.Kilo.PrefixValue);
            Assert.Equal(1000, bs.Mega.PrefixValue);
            Assert.Equal(1, bs.Giga.PrefixValue);
            Assert.Equal(0.001, bs.Tera.PrefixValue);
            Assert.Equal(1E-06, bs.Peta.PrefixValue);
            Assert.Equal(976562.5, bs.Kibi.PrefixValue);
            Assert.Equal(953.67431640625, bs.Mebi.PrefixValue);
            Assert.Equal(0.93132257461547852, bs.Gibi.PrefixValue);
            Assert.Equal(0.00090949470177292824, bs.Tebi.PrefixValue);
            Assert.Equal(8.8817841970012523E-07, bs.Pebi.PrefixValue);
            Assert.Equal("1 gigabit", bs.ToString());

            TestOutput.WriteLine(bs.ToString());
        }
    }
}