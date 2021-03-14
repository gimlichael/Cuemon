using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class ByteStorageCapacityTest : Test
    {
        public ByteStorageCapacityTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ByteStorageCapacity_UseSymbol_ShouldConvertOneBillionBytesToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = ByteStorageCapacity.FromBytes(1000000000);

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
        public void ByteStorageCapacity_UseCompound_ShouldConvertOneBillionBytesToBinaryAndMetricPrefixToStringRepresentation()
        {
            var x = ByteStorageCapacity.FromBytes(1000000000, o => o.Style = NamingStyle.Compound);

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