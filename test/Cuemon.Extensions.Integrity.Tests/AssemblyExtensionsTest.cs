using Cuemon.Extensions.Xunit;
using Cuemon.Integrity;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Integrity
{
    public class AssemblyExtensionsTest : Test
    {
        public AssemblyExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetCacheValidator_ShouldHaveStrongIntegrityChecksum()
        {

            var a = typeof(AssemblyExtensionsTest).Assembly;
            var cv = a.GetCacheValidator();
            Assert.Equal(ChecksumStrength.Strong, cv.Strength);
            Assert.NotEqual(cv.ToString(), CacheValidator.Default.ToString());
            TestOutput.WriteLine(cv.ToString());
        }
    }
}