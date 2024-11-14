using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Extensions.IO;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Core;
using YamlDotNet.Serialization.NamingConventions;

namespace Cuemon.Extensions.Globalization
{
    public class YamlSerializerTest : Test
    {
        private readonly CultureInfo _cultureInfo;

        public YamlSerializerTest(ITestOutputHelper output) : base(output)
        {
            try
            {
                _cultureInfo = new CultureInfo("da-DK").UseNationalLanguageSupport(); // from .NET6+ this is needed for both Windows and Linux; at least from pipeline (worked locally for Windows without Merge ...)
            }
            catch (Exception e)
            {
                TestOutput.WriteLine(e.ToString());
            }
        }

        [Fact]
        public void Serialize_ShouldSerializeDateFormatInfo()
        {
            var sut2 = _cultureInfo;
            var sut3 = YamlFormatter.SerializeObject(sut2.DateTimeFormat, o =>
            {
                o.Settings.NamingConvention = PascalCaseNamingConvention.Instance;
                o.Settings.ScalarStyle = ScalarStyle.Plain;
                o.Settings.IndentSequences = false;
                o.Settings.FormatProvider = _cultureInfo;
                o.Settings.Converters.Add(YamlConverterFactory.Create<DateTime>((writer, dt, _) => writer.WriteValue(dt.ToString(_cultureInfo))));
            });
            var sut4 = sut3.ToEncodedString();

            TestOutput.WriteLine(sut4);

            var expected = @"AMDesignator: ''
Calendar:
  MinSupportedDateTime: 01-01-0001 00:00:00
  MaxSupportedDateTime: 31-12-9999 23:59:59
  AlgorithmType: SolarCalendar
  CalendarType: Localized
  Eras:
  - 1
  TwoDigitYearMax: {0}
DateSeparator: '-'
FirstDayOfWeek: Monday
CalendarWeekRule: FirstFourDayWeek
FullDateTimePattern: d. MMMM yyyy HH:mm:ss
LongDatePattern: d. MMMM yyyy
LongTimePattern: HH:mm:ss
MonthDayPattern: d. MMMM
PMDesignator: ''
RFC1123Pattern: ddd, dd MMM yyyy HH':'mm':'ss 'GMT'
ShortDatePattern: dd-MM-yyyy
ShortTimePattern: HH:mm
SortableDateTimePattern: yyyy'-'MM'-'dd'T'HH':'mm':'ss
TimeSeparator: ':'
UniversalSortableDateTimePattern: yyyy'-'MM'-'dd HH':'mm':'ss'Z'
YearMonthPattern: MMMM yyyy
AbbreviatedDayNames:
- sø
- ma
- ti
- on
- to
- fr
- lø
ShortestDayNames:
- sø
- ma
- ti
- on
- to
- fr
- lø
DayNames:
- søndag
- mandag
- tirsdag
- onsdag
- torsdag
- fredag
- lørdag
AbbreviatedMonthNames:
- jan
- feb
- mar
- apr
- maj
- jun
- jul
- aug
- sep
- okt
- nov
- dec
- ''
MonthNames:
- januar
- februar
- marts
- april
- maj
- juni
- juli
- august
- september
- oktober
- november
- december
- ''
NativeCalendarName: gregoriansk kalender
AbbreviatedMonthGenitiveNames:
- jan
- feb
- mar
- apr
- maj
- jun
- jul
- aug
- sep
- okt
- nov
- dec
- ''
MonthGenitiveNames:
- januar
- februar
- marts
- april
- maj
- juni
- juli
- august
- september
- oktober
- november
- december
- ''
".ReplaceLineEndings();

#if NET8_0_OR_GREATER
            expected = string.Format(expected, "2049");
#elif NET48_OR_GREATER
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                expected = expected.ReplaceAll("gregoriansk kalender", "dansk (Danmark)", StringComparison.Ordinal);
                expected = string.Format(expected, "2029");
            }
            else
            {
                expected = expected.ReplaceAll("gregoriansk", "Gregoriansk", StringComparison.Ordinal);
                expected = string.Format(expected, "2049");
            }
#endif

            Assert.Equal(expected, sut4);
        }

        [Fact]
        public void Serialize_ShouldSerializeNumberFormatInfo()
        {
            var sut2 = _cultureInfo;
            var sut3 = YamlFormatter.SerializeObject(sut2.NumberFormat, o =>
            {
                o.Settings.ScalarStyle = ScalarStyle.DoubleQuoted;
                o.Settings.IndentSequences = false;
                o.Settings.FormatProvider = _cultureInfo;
                o.Settings.NamingConvention = PascalCaseNamingConvention.Instance;
            });
            var sut4 = sut3.ToEncodedString();

            TestOutput.WriteLine(sut4);

            Assert.Equal(@"CurrencyDecimalDigits: 2
CurrencyDecimalSeparator: "",""
CurrencyGroupSizes:
- 3
NumberGroupSizes:
- 3
PercentGroupSizes:
- 3
CurrencyGroupSeparator: "".""
CurrencySymbol: ""kr.""
NaNSymbol: ""NaN""
CurrencyNegativePattern: 8
NumberNegativePattern: 1
PercentPositivePattern: 0
PercentNegativePattern: 0
NegativeInfinitySymbol: ""-∞""
NegativeSign: ""-""
NumberDecimalDigits: 2
NumberDecimalSeparator: "",""
NumberGroupSeparator: "".""
CurrencyPositivePattern: 3
PositiveInfinitySymbol: ""∞""
PositiveSign: ""+""
PercentDecimalDigits: 2
PercentDecimalSeparator: "",""
PercentGroupSeparator: "".""
PercentSymbol: ""%""
PerMilleSymbol: ""‰""
NativeDigits:
- ""0""
- ""1""
- ""2""
- ""3""
- ""4""
- ""5""
- ""6""
- ""7""
- ""8""
- ""9""
DigitSubstitution: ""None""
".ReplaceLineEndings(), sut4);
        }


        [Fact]
        public void Serialize_ShouldSerializeCultureInfo()
        {
            var sut2 = _cultureInfo;
            var sut3 = YamlFormatter.SerializeObject(sut2, o =>
            {
                o.Settings.IndentSequences = false;
                o.Settings.NamingConvention = PascalCaseNamingConvention.Instance;
                o.Settings.Converters.Add(YamlConverterFactory.Create<DateTime>((writer, dt, _) => writer.WriteValue(dt.ToString(_cultureInfo))));
            });
            var sut4 = sut3.ToEncodedString().ReplaceLineEndings().Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            sut4.RemoveRange(sut4.FindIndex(s => s.StartsWith("CompareInfo")), 6);
            sut4.RemoveRange(sut4.FindIndex(s => s.StartsWith("CultureTypes")), 1);

            var expected = @"LCID: 1030
KeyboardLayoutId: 1030
Name: da-DK
IetfLanguageTag: da-DK
DisplayName: Danish (Denmark)
NativeName: dansk (Danmark)
EnglishName: Danish (Denmark)
TwoLetterISOLanguageName: da
ThreeLetterISOLanguageName: dan
ThreeLetterWindowsLanguageName: DAN
TextInfo:
  ANSICodePage: 1252
  OEMCodePage: 850
  MacCodePage: 10000
  EBCDICCodePage: 20277
  LCID: 1030
  CultureName: da-DK
  ListSeparator: ;
  IsRightToLeft: false
IsNeutralCulture: false
NumberFormat:
  CurrencyDecimalDigits: 2
  CurrencyDecimalSeparator: ','
  CurrencyGroupSizes:
  - 3
  NumberGroupSizes:
  - 3
  PercentGroupSizes:
  - 3
  CurrencyGroupSeparator: .
  CurrencySymbol: kr.
  NaNSymbol: NaN
  CurrencyNegativePattern: 8
  NumberNegativePattern: 1
  PercentPositivePattern: 0
  PercentNegativePattern: 0
  NegativeInfinitySymbol: -∞
  NegativeSign: '-'
  NumberDecimalDigits: 2
  NumberDecimalSeparator: ','
  NumberGroupSeparator: .
  CurrencyPositivePattern: 3
  PositiveInfinitySymbol: ∞
  PositiveSign: +
  PercentDecimalDigits: 2
  PercentDecimalSeparator: ','
  PercentGroupSeparator: .
  PercentSymbol: '%'
  PerMilleSymbol: ‰
  NativeDigits:
  - 0
  - 1
  - 2
  - 3
  - 4
  - 5
  - 6
  - 7
  - 8
  - 9
  DigitSubstitution: None
DateTimeFormat:
  AMDesignator: ''
  Calendar:
    MinSupportedDateTime: 01-01-0001 00:00:00
    MaxSupportedDateTime: 31-12-9999 23:59:59
    AlgorithmType: SolarCalendar
    CalendarType: Localized
    Eras:
    - 1
    TwoDigitYearMax: {0}
  DateSeparator: '-'
  FirstDayOfWeek: Monday
  CalendarWeekRule: FirstFourDayWeek
  FullDateTimePattern: d. MMMM yyyy HH:mm:ss
  LongDatePattern: d. MMMM yyyy
  LongTimePattern: HH:mm:ss
  MonthDayPattern: d. MMMM
  PMDesignator: ''
  RFC1123Pattern: ddd, dd MMM yyyy HH':'mm':'ss 'GMT'
  ShortDatePattern: dd-MM-yyyy
  ShortTimePattern: HH:mm
  SortableDateTimePattern: yyyy'-'MM'-'dd'T'HH':'mm':'ss
  TimeSeparator: ':'
  UniversalSortableDateTimePattern: yyyy'-'MM'-'dd HH':'mm':'ss'Z'
  YearMonthPattern: MMMM yyyy
  AbbreviatedDayNames:
  - sø
  - ma
  - ti
  - on
  - to
  - fr
  - lø
  ShortestDayNames:
  - sø
  - ma
  - ti
  - on
  - to
  - fr
  - lø
  DayNames:
  - søndag
  - mandag
  - tirsdag
  - onsdag
  - torsdag
  - fredag
  - lørdag
  AbbreviatedMonthNames:
  - jan
  - feb
  - mar
  - apr
  - maj
  - jun
  - jul
  - aug
  - sep
  - okt
  - nov
  - dec
  - ''
  MonthNames:
  - januar
  - februar
  - marts
  - april
  - maj
  - juni
  - juli
  - august
  - september
  - oktober
  - november
  - december
  - ''
  NativeCalendarName: gregoriansk kalender
  AbbreviatedMonthGenitiveNames:
  - jan
  - feb
  - mar
  - apr
  - maj
  - jun
  - jul
  - aug
  - sep
  - okt
  - nov
  - dec
  - ''
  MonthGenitiveNames:
  - januar
  - februar
  - marts
  - april
  - maj
  - juni
  - juli
  - august
  - september
  - oktober
  - november
  - december
  - ''
Calendar:
  MinSupportedDateTime: 01-01-0001 00:00:00
  MaxSupportedDateTime: 31-12-9999 23:59:59
  AlgorithmType: SolarCalendar
  CalendarType: Localized
  Eras:
  - 1
  TwoDigitYearMax: {0}
OptionalCalendars:
- MinSupportedDateTime: 01-01-0001 00:00:00
  MaxSupportedDateTime: 31-12-9999 23:59:59
  AlgorithmType: SolarCalendar
  CalendarType: Localized
  Eras:
  - 1
  TwoDigitYearMax: {0}
UseUserOverride: true
";

#if NET8_0_OR_GREATER || NET48_OR_GREATER
            expected = string.Format(expected, "2049");
#else
            expected = string.Format(expected, "2029");
#endif

#if NET48_OR_GREATER
            expected = expected.ReplaceAll("gregoriansk", "Gregoriansk", StringComparison.Ordinal);
#endif

            TestOutput.WriteLines(sut4);

            Assert.Equal(expected.ReplaceLineEndings().Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList(), sut4);
        }
    }
}
