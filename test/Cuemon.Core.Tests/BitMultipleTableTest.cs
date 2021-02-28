using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class BitMultipleTableTest : Test
    {
        public BitMultipleTableTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitMultipleTable_ShouldBeEqualWithAFactorOfEight()
        {
            var x = BitMultipleTable.FromBytes(1000000000);
            var y = BitMultipleTable.FromBits(8000000000);
            Assert.Equal(x, y);
        }

        [Fact]
        public void BitMultipleTable_UseSymbol_ShouldConvertOneBillionBitsToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = BitMultipleTable.FromBits(1000000000);

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
        public void BitMultipleTable_UseCompound_ShouldConvertOneBillionBitsToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = BitMultipleTable.FromBits(1000000000, o => o.Style = NamingStyle.Compound);

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

        public void BitMultipleTable_UseCompound_ShouldConvertOneBillionBitsToBinaryAndMetricPrefixDoubleRepresentation()
        {
            var bs = BitMultipleTable.FromBits(1000000000, o =>
            {
                o.Style  = NamingStyle.Compound;
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

        [Fact]
        public void BitMultipleTable_UseSymbol_ShouldConvertOneBillionBytesToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = ByteMultipleTable.FromBytes(1000000000);

            Assert.Equal("0 PiB", x.Pebi.ToString());
            Assert.Equal("0 TiB", x.Tebi.ToString());
            Assert.Equal("0.93 GiB", x.Gibi.ToString());
            Assert.Equal("953.67 MiB", x.Mebi.ToString());
            Assert.Equal("976,562.5 KiB", x.Kibi.ToString());
            
            Assert.Equal("0 PB", x.Peta.ToString());
            Assert.Equal("0 TB", x.Tera.ToString());
            Assert.Equal("1 GB", x.Giga.ToString());
            Assert.Equal("1,000 MB", x.Mega.ToString());
            Assert.Equal("1,000,000 kB", x.Kilo.ToString());

            Assert.Equal("1,000,000,000 B", x.Unit.ToString());

            TestOutput.WriteLine(x.ToAggregateString());
        }

        [Fact]
        public void BitMultipleTable_UseCompound_ShouldConvertOneBillionBytesToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = ByteMultipleTable.FromBytes(1000000000, o => o.Style = NamingStyle.Compound);

            Assert.Equal("0 pebibyte", x.Pebi.ToString());
            Assert.Equal("0 tebibyte", x.Tebi.ToString());
            Assert.Equal("0.93 gibibyte", x.Gibi.ToString());
            Assert.Equal("953.67 mebibyte", x.Mebi.ToString());
            Assert.Equal("976,562.5 kibibyte", x.Kibi.ToString());
            
            Assert.Equal("0 petabyte", x.Peta.ToString());
            Assert.Equal("0 terabyte", x.Tera.ToString());
            Assert.Equal("1 gigabyte", x.Giga.ToString());
            Assert.Equal("1,000 megabyte", x.Mega.ToString());
            Assert.Equal("1,000,000 kilobyte", x.Kilo.ToString());

            Assert.Equal("1,000,000,000 byte", x.Unit.ToString());

            TestOutput.WriteLine(x.ToAggregateString());
        }
    }
}
