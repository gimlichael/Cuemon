using Cuemon.Assets;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Runtime.Serialization
{
    public class HierarchySerializerTest : Test
    {
        public HierarchySerializerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldMakeHierarchy()
        {
            var sut1 = new HierarchyExample();
            var sut2 = new HierarchySerializer(sut1);
        }
    }
}