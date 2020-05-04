using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Cuemon.Integrity;
using Cuemon.IO;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core.Tests.IO
{
    public class StreamDecoratorExtensionsTest : Test
    {
        public StreamDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToByteArray_ShouldConvertStreamToByteArrayWithDefaultOptions()
        {
            var size = 1024 * 1024;
            var fs = Generate.FixedString('*', size);
            var fsBytes = Convertible.GetBytes(fs);
            var s = new MemoryStream(fsBytes);
            var sb = Decorator.Enclose(s).ToByteArray();

            Assert.Throws<ObjectDisposedException>(() => s.Capacity);
            Assert.Equal(fsBytes, sb);
            Assert.Equal(size, sb.Length);
            Assert.True(sb.All(b => b == '*'), "Expected all elements to have the value of '*'.");
        }

        [Fact]
        public async Task ToByteArrayAsync_ShouldConvertStreamToByteArrayWithDefaultOptions()
        {
            var size = 1024 * 1024;
            var fs = Generate.FixedString('*', size);
            var fsBytes = Convertible.GetBytes(fs);
            var s = new MemoryStream(fsBytes);
            var sb = await Decorator.Enclose(s).ToByteArrayAsync();
            
            Assert.Throws<ObjectDisposedException>(() => s.Capacity);
            Assert.Equal(fsBytes, sb);
            Assert.Equal(size, sb.Length);
            Assert.True(sb.All(b => b == '*'), "Expected all elements to have the value of '*'.");
        }
    }
}