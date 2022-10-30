using System.Globalization;
using Cuemon.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Globalization
{
    public class CultureInfoExtensionsTest : Test
    {
        [Fact]
        public void MergeWithOriginal_ShouldHaveDifferentFormattingAsWindowsVariant()
        {
            var sut1 = new CultureInfo("da-dk");
            var sut2 = new CultureInfo("da-dk").MergeWithOriginalFormatting();

            Assert.NotEqual(sut1.DateTimeFormat, sut2.DateTimeFormat);
            Assert.NotEqual(sut1.NumberFormat, sut2.NumberFormat);
        }

        [Fact]
        public void MergeWithOriginal_ShouldHaveSameFormattingAsWindowsVariant()
        {
            var sut1 = new CultureInfo("da-dk");
            var sut2 = new CultureInfo("da-dk").MergeWithOriginalFormatting();

            Assert.NotEqual(sut1.DateTimeFormat, sut2.DateTimeFormat);
            Assert.NotEqual(sut1.NumberFormat, sut2.NumberFormat);
        }
    }
}
