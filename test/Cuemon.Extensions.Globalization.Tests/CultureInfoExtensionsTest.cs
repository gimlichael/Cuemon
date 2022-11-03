using System.Globalization;
using System.Linq;
using Cuemon.Extensions.Xunit;
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
            var sut2 = new CultureInfo("da-dk").MergeWithOriginalFormatting();

            Assert.NotEqual(sut1.DateTimeFormat, sut2.DateTimeFormat);
            Assert.NotEqual(sut1.NumberFormat, sut2.NumberFormat);
            Assert.Equal("dd.MM.yyyy", sut1.DateTimeFormat.ShortDatePattern);
            Assert.Equal("dd-MM-yyyy", sut2.DateTimeFormat.ShortDatePattern);
        }
    }
}
