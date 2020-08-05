using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core
{
    public class StringDecoratorExtensionsTest : Test
    {
        public StringDecoratorExtensionsTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void ToEncodedString_ShouldReplaceEmojisWithQuestionMarks()
        {
            var rs = $"{Generate.RandomString(128)}😁😂😃";
            var iso88591 = Decorator.Enclose(rs).ToEncodedString(o =>
            {
                o.TargetEncoding = Encoding.GetEncoding("iso-8859-1");
                o.EncoderFallback = new EncoderReplacementFallback("?");
            });
            TestOutput.WriteLine(rs);
            TestOutput.WriteLine(iso88591);
            Assert.Equal(rs.Length, iso88591.Length);
            Assert.EndsWith("??????", iso88591);
            Assert.DoesNotContain(iso88591, new List<string>()
            {
                "😁",
                "😂",
                "😃"
            });
        }

        [Fact]
        public void ToAsciiEncodedString_ShouldStripStringFromNoneAsciiCharacters()
        {
            var rs = $"{Generate.RandomString(128)}ÆØÅæøå";
            var asciiRs = Decorator.Enclose(rs).ToAsciiEncodedString();
            TestOutput.WriteLine(rs);
            TestOutput.WriteLine(asciiRs);
            Assert.NotEqual(rs.Length, asciiRs.Length);
            Assert.True(rs.Length > asciiRs.Length);
            Assert.DoesNotContain(asciiRs, new List<string>()
            {
                "æ",
                "ø",
                "å",
                "Æ",
                "Ø",
                "Å"
            });
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