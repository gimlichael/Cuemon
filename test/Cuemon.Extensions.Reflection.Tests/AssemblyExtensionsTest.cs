using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Reflection
{
    public class AssemblyExtensionsTest : Test
    {
        public AssemblyExtensionsTest(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public void GetAssemblyVersion_ShouldReturnAssemblyVersion()
        {
            var sut1 = typeof(Disposable).Assembly;
            var sut2 = sut1.GetAssemblyVersion();

            TestOutput.WriteLine(sut2.ToString());

            Assert.Equal("6.0.0.0", sut2.ToString());
            Assert.False(sut2.HasAlphanumericVersion);
            Assert.False(sut2.IsSemanticVersion());
        }

        [Fact]
        public void GetFileVersion_ShouldReturnFileVersion()
        {
            var sut1 = typeof(Disposable).Assembly;
            var sut2 = sut1.GetFileVersion();

            TestOutput.WriteLine(sut2.ToString());

            Assert.False(sut2.IsSemanticVersion());
            Assert.True(sut2.HasAlphanumericVersion);
            Assert.StartsWith("6.0.0", sut2.ToString());
        }

        [Fact]
        public void GetProductVersion_ShouldReturnProductVersion()
        {
            var sut1 = typeof(Disposable).Assembly;
            var sut2 = sut1.GetProductVersion();

            TestOutput.WriteLine(sut2.ToString());

            Assert.True(sut2.IsSemanticVersion());
            Assert.True(sut2.HasAlphanumericVersion);
            Assert.Equal("6.0", sut2.ToVersion().ToString());
        }

        [Fact]
        public void IsDebugBuild_ShouldBeTrueForDebugOrFalseForRelease()
        {
#if RELEASE
            Assert.False(this.GetType().Assembly.IsDebugBuild());
#else
            Assert.True(this.GetType().Assembly.IsDebugBuild());
#endif
        }
    }
}
