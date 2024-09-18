using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Cuemon.Globalization;
using Xunit;

namespace Cuemon.Extensions.Globalization
{
    public class CultureInfoExtensionsTest : Test
    {
        [Fact]
        public void MergeWithOriginal_ShouldHaveDifferentFormattingAsWindowsVariant()
        {
            var sut1 = World.GetCultures(new RegionInfo("DK")).Single(ci => ci.Name == "da-DK");
            var sut2 = new CultureInfo("da-dk").UseNationalLanguageSupport();

            Assert.NotEqual(sut1.DateTimeFormat, sut2.DateTimeFormat);
            Assert.NotEqual(sut1.NumberFormat, sut2.NumberFormat);
#if NET48_OR_GREATER
            Assert.Equal(sut1.DateTimeFormat.ShortDatePattern, sut2.DateTimeFormat.ShortDatePattern);
#else
            Assert.Equal("dd.MM.yyyy", sut1.DateTimeFormat.ShortDatePattern);
            Assert.Equal("dd-MM-yyyy", sut2.DateTimeFormat.ShortDatePattern);
#endif
        }
    }
}
