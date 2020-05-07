using System;
using System.IO;
using System.Linq;
using System.Text;
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

        [Fact]
        public void ToEncodedString_ShouldConvertStreamToString()
        {
            var size = 1024;
            var enc = Encoding.GetEncoding("iso-8859-1");
            var fs = Generate.RandomString(size, "æøåÆØÅ");
            var fsBytesUnicode = Convertible.GetBytes(fs);
            var fsBytesIso88591 = Convertible.GetBytes(fs, o => o.Encoding = enc);
            var sUnicode = Decorator.Enclose(fsBytesUnicode).ToStream();
            var sUnicodeLength = sUnicode.Length;
            var sIso88591 = Decorator.Enclose(fsBytesIso88591).ToStream();
            var sIso88591Length = sIso88591.Length;
            var resultUnicode = Decorator.Enclose(sUnicode).ToEncodedString(o => o.LeaveOpen = true);
            var resultIso88591 = Decorator.Enclose(sIso88591).ToEncodedString(o =>
            {
                o.Encoding = enc;
                o.LeaveOpen = true;
            });
            var wrongDecodedUnicodeResult = Decorator.Enclose(sUnicode).ToEncodedString(o => o.Encoding = Encoding.ASCII);
            var wrongDecodedIso88591Result = Decorator.Enclose(sIso88591).ToEncodedString(o => o.Encoding = Encoding.ASCII);
            
            Assert.Throws<ObjectDisposedException>(() => sIso88591.Length);
            Assert.Throws<ObjectDisposedException>(() => sUnicode.Length);
            Assert.Equal(fsBytesIso88591.Length, sIso88591Length);
            Assert.Equal(fsBytesUnicode.Length, sUnicodeLength);
            Assert.NotEqual(fsBytesIso88591.Length, fsBytesUnicode.Length);
            Assert.NotEqual(sIso88591Length, sUnicodeLength);
            Assert.Equal(fs, resultIso88591);
            Assert.Equal(fs, resultUnicode);
            Assert.NotEqual(fs, wrongDecodedIso88591Result);
            Assert.NotEqual(fs, wrongDecodedUnicodeResult);
        }

        [Fact]
        public async Task ToEncodedStringAsync_ShouldConvertStreamToString()
        {
            var size = 1024;
            var enc = Encoding.GetEncoding("iso-8859-1");
            var fs = Generate.RandomString(size, "æøåÆØÅ");
            var fsBytesUnicode = Convertible.GetBytes(fs);
            var fsBytesIso88591 = Convertible.GetBytes(fs, o => o.Encoding = enc);
            var sUnicode = await Decorator.Enclose(fsBytesUnicode).ToStreamAsync();
            var sUnicodeLength = sUnicode.Length;
            var sIso88591 = await Decorator.Enclose(fsBytesIso88591).ToStreamAsync();
            var sIso88591Length = sIso88591.Length;
            var resultUnicode = await Decorator.Enclose(sUnicode).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var resultIso88591 = await Decorator.Enclose(sIso88591).ToEncodedStringAsync(o =>
            {
                o.Encoding = enc;
                o.LeaveOpen = true;
            });
            var wrongDecodedUnicodeResult = await Decorator.Enclose(sUnicode).ToEncodedStringAsync(o => o.Encoding = Encoding.ASCII);
            var wrongDecodedIso88591Result = await Decorator.Enclose(sIso88591).ToEncodedStringAsync(o => o.Encoding = Encoding.ASCII);
            
            Assert.Throws<ObjectDisposedException>(() => sIso88591.Length);
            Assert.Throws<ObjectDisposedException>(() => sUnicode.Length);
            Assert.Equal(fsBytesIso88591.Length, sIso88591Length);
            Assert.Equal(fsBytesUnicode.Length, sUnicodeLength);
            Assert.NotEqual(fsBytesIso88591.Length, fsBytesUnicode.Length);
            Assert.NotEqual(sIso88591Length, sUnicodeLength);
            Assert.Equal(fs, resultIso88591);
            Assert.Equal(fs, resultUnicode);
            Assert.NotEqual(fs, wrongDecodedIso88591Result);
            Assert.NotEqual(fs, wrongDecodedUnicodeResult);
        }
    }
}