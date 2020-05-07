using System.IO;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core.Tests
{
    public class StringDecoratorExtensionsTest : Test
    {
        public StringDecoratorExtensionsTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void ToStream_ShouldConvertStringToStream()
        {
            var size = 2048;
            var rs = Generate.RandomString(size);
            var s = Decorator.Enclose(rs).ToStream();
            using (var sr = new StreamReader(s))
            {
                var result = sr.ReadToEnd();
                Assert.Equal(size, s.Length);
                Assert.Equal(rs, result);
            }
        }

        [Fact]
        public async Task ToStreamAsync_ShouldConvertStringToStream()
        {
            var size = 2048;
            var rs = Generate.RandomString(size);
            var s = await Decorator.Enclose(rs).ToStreamAsync();
            using (var sr = new StreamReader(s))
            {
                var result = await sr.ReadToEndAsync();
                Assert.Equal(size, s.Length);
                Assert.Equal(rs, result);
            }
        }
    }
}