using System.IO;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class ByteArrayDecoratorExtensionsTest : Test
    {
        public ByteArrayDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToStream_ShouldConvertByteArrayToStream()
        {
            var size = 1024 * 1024;
            var fs = Generate.FixedString('*', size);
            var fsBytes = Convertible.GetBytes(fs);
            var s = Decorator.Enclose(fsBytes).ToStream();
            using (var sr = new StreamReader(s))
            {
                var result = sr.ReadToEnd();
                Assert.Equal(size, s.Length);
                Assert.All(result, c => Assert.Equal('*', c));
            }
        }

        [Fact]
        public async Task ToStreamAsync_ShouldConvertByteArrayToStream()
        {
            var size = 1024 * 1024;
            var fs = Generate.FixedString('*', size);
            var fsBytes = Convertible.GetBytes(fs);
            var s = await Decorator.Enclose(fsBytes).ToStreamAsync();
            using (var sr = new StreamReader(s))
            {
                var result = await sr.ReadToEndAsync();
                Assert.Equal(size, s.Length);
                Assert.All(result, c => Assert.Equal('*', c));
            }
        }
    }
}