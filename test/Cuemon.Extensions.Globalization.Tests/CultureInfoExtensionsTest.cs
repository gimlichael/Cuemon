using System.Globalization;
using System.Runtime.InteropServices;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Globalization
{
    public class CultureInfoExtensionsTest : Test
    {
        public CultureInfoExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void UseNationalLanguageSupport_ShouldHaveDifferentFormattingAsWindowsVariant()
        {
            var sut1 = new CultureInfo("da-DK", false);
            var sut2 = (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        ? new CultureInfo("da-DK") // Linux uses ICU
                        : new CultureInfo("da-DK", false) // Ensure we do not read from user culture settings on Windows
                ).UseNationalLanguageSupport();

            Assert.NotEqual(sut1.DateTimeFormat, sut2.DateTimeFormat);
            Assert.NotEqual(sut1.NumberFormat, sut2.NumberFormat);
#if NET48_OR_GREATER
            Assert.Equal(sut1.DateTimeFormat.ShortDatePattern, sut2.DateTimeFormat.ShortDatePattern);
#else
            Assert.Equal("dd.MM.yyyy", sut1.DateTimeFormat.ShortDatePattern);
            Assert.Equal("dd-MM-yyyy", sut2.DateTimeFormat.ShortDatePattern);
#endif
        }

        [Fact]
        public void UseNationalLanguageSupport_ShouldHaveDifferentFormattingAsWindowsVariant_FromReadOnlyCultureInfos()
        {
            var sut1 = CultureInfo.GetCultureInfo("da-DK");
            var sut2 = CultureInfo.GetCultureInfo("da-DK").UseNationalLanguageSupport();

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
