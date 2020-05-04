using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Cuemon.Integrity;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core.Tests
{
    public class ByteArrayTest : Test
    {
        public ByteArrayTest(ITestOutputHelper output) : base(output)
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
                Assert.True(result.All(b => b == '*'), "Expected all elements to have the value of '*'.");
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
                Assert.True(result.All(b => b == '*'), "Expected all elements to have the value of '*'.");
            }
        }
    }
}