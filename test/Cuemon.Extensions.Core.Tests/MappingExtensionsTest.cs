using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions
{
    public class MappingExtensionsTest : Test
    {
        public MappingExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Add_ShouldAddMappingWithStringValues()
        {
            var sut1 = new List<Mapping>();
            var sut2 = new List<Mapping>(sut1);
            var sut3 = new Mapping("Source", "Destination");
            sut2.AddMapping("Source", "Destination");

            Assert.Empty(sut1);
            Assert.Equal(1, sut2.Count);
            Assert.Equal(sut3.Source, sut2.Single().Source);
            Assert.Equal(sut3.Destination, sut2.Single().Destination);
        }

        [Fact]
        public void Add_ShouldAddIndexMappingWithOrdinalAndString()
        {
            var sut1 = new List<Mapping>();
            var sut2 = new List<Mapping>(sut1);
            var sut3 = new IndexMapping(42, "Destination");
            sut2.AddMapping(42, "Destination");

            Assert.Empty(sut1);
            Assert.Equal(1, sut2.Count);
            Assert.Equal(sut3.SourceIndex, sut2.Single().As<IndexMapping>().SourceIndex);
            Assert.Equal(sut3.Destination, sut2.Single().Destination);
        }

        [Fact]
        public void Add_ShouldAddIndexMappingWithStringAndOrdinal()
        {
            var sut1 = new List<Mapping>();
            var sut2 = new List<Mapping>(sut1);
            var sut3 = new IndexMapping("Source", 42);
            sut2.AddMapping("Source", 42);

            Assert.Empty(sut1);
            Assert.Equal(1, sut2.Count);
            Assert.Equal(sut3.Source, sut2.Single().Source);
            Assert.Equal(sut3.DestinationIndex, sut2.Single().As<IndexMapping>().DestinationIndex);
        }

        [Fact]
        public void Add_ShouldAddIndexMappingWithOrdinals()
        {
            var sut1 = new List<Mapping>();
            var sut2 = new List<Mapping>(sut1);
            var sut3 = new IndexMapping(24, 42);
            sut2.AddMapping(24, 42);

            Assert.Empty(sut1);
            Assert.Equal(1, sut2.Count);
            Assert.Equal(sut3.SourceIndex, sut2.Single().As<IndexMapping>().SourceIndex);
            Assert.Equal(sut3.DestinationIndex, sut2.Single().As<IndexMapping>().DestinationIndex);
        }
    }
}