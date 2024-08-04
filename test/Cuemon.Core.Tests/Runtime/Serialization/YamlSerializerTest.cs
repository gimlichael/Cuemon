using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.Globalization;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Text.Yaml;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Runtime.Serialization
{
    public class YamlSerializerTest : Test
    {
        private readonly CultureInfo _cultureInfo = new CultureInfo("da-DK").UseNationalLanguageSupport(); // from .NET6+ this is needed for both Windows and Linux; at least from pipeline (worked locally for Windows without Merge ...)

        public YamlSerializerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Serialize_ShouldSerializeDateFormatInfo()
        {
            var sut1 = new YamlSerializer(o => o.Converters.Add(YamlConverterFactory.Create<DateTime>((writer, dt, so) =>
            {
                writer.WriteLine(dt.ToString(_cultureInfo));
            })));
            var sut2 = _cultureInfo;
            var sut3 = sut1.Serialize(sut2.DateTimeFormat);
            var sut4 = sut3.ToEncodedString();

            TestOutput.WriteLine(sut4);

            var expected = @"AMDesignator: 
Calendar: 
  MinSupportedDateTime: 01-01-0001 00:00:00
  MaxSupportedDateTime: 31-12-9999 23:59:59
  AlgorithmType: SolarCalendar
  CalendarType: Localized
  Eras: 
  - 1
  TwoDigitYearMax: {0}
DateSeparator: ""-""
FirstDayOfWeek: Monday
CalendarWeekRule: FirstFourDayWeek
FullDateTimePattern: d. MMMM yyyy HH:mm:ss
LongDatePattern: d. MMMM yyyy
LongTimePattern: HH:mm:ss
MonthDayPattern: d. MMMM
PMDesignator: 
RFC1123Pattern: ddd, dd MMM yyyy HH':'mm':'ss 'GMT'
ShortDatePattern: dd-MM-yyyy
ShortTimePattern: HH:mm
SortableDateTimePattern: yyyy'-'MM'-'dd'T'HH':'mm':'ss
TimeSeparator: "":""
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
- 
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
- 
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
- 
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
- ".ReplaceLineEndings();

#if NET8_0_OR_GREATER
            expected = string.Format(expected, "2049");
#elif NET6_0_OR_GREATER
            expected = string.Format(expected, "2029");
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
            var sut1 = new YamlSerializer();
            var sut2 = _cultureInfo;
            var sut3 = sut1.Serialize(sut2.NumberFormat);
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
CurrencySymbol: kr.
NaNSymbol: NaN
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
PositiveInfinitySymbol: ∞
PositiveSign: ""+""
PercentDecimalDigits: 2
PercentDecimalSeparator: "",""
PercentGroupSeparator: "".""
PercentSymbol: ""%""
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
DigitSubstitution: None".ReplaceLineEndings(), sut4);
        }


        [Fact]
        public void Serialize_ShouldSerializeCultureInfo()
        {
            var sut1 = new YamlSerializer(o => o.Converters.Add(YamlConverterFactory.Create<DateTime>((writer, dt, so) =>
            {
                writer.WriteLine(dt.ToString(_cultureInfo));
            })));

            var sut2 = _cultureInfo;
            var sut3 = sut1.Serialize(sut2);
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
  ListSeparator: "";""
  IsRightToLeft: False
IsNeutralCulture: False
NumberFormat: 
  CurrencyDecimalDigits: 2
  CurrencyDecimalSeparator: "",""
  CurrencyGroupSizes: 
  - 3
  NumberGroupSizes: 
  - 3
  PercentGroupSizes: 
  - 3
  CurrencyGroupSeparator: "".""
  CurrencySymbol: kr.
  NaNSymbol: NaN
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
  PositiveInfinitySymbol: ∞
  PositiveSign: ""+""
  PercentDecimalDigits: 2
  PercentDecimalSeparator: "",""
  PercentGroupSeparator: "".""
  PercentSymbol: ""%""
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
  AMDesignator: 
  Calendar: 
    MinSupportedDateTime: 01-01-0001 00:00:00
    MaxSupportedDateTime: 31-12-9999 23:59:59
    AlgorithmType: SolarCalendar
    CalendarType: Localized
    Eras: 
    - 1
    TwoDigitYearMax: {0}
  DateSeparator: ""-""
  FirstDayOfWeek: Monday
  CalendarWeekRule: FirstFourDayWeek
  FullDateTimePattern: d. MMMM yyyy HH:mm:ss
  LongDatePattern: d. MMMM yyyy
  LongTimePattern: HH:mm:ss
  MonthDayPattern: d. MMMM
  PMDesignator: 
  RFC1123Pattern: ddd, dd MMM yyyy HH':'mm':'ss 'GMT'
  ShortDatePattern: dd-MM-yyyy
  ShortTimePattern: HH:mm
  SortableDateTimePattern: yyyy'-'MM'-'dd'T'HH':'mm':'ss
  TimeSeparator: "":""
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
  - 
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
  - 
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
  - 
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
  - 
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
UseUserOverride: True";

#if NET8_0_OR_GREATER || NET48_OR_GREATER
            expected = string.Format(expected, "2049");
#else
            expected = string.Format(expected, "2029");
#endif

#if NET48_OR_GREATER
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                expected = @"IetfLanguageTag: da-DK
KeyboardLayoutId: 1030
LCID: 1030
Name: da-DK
NativeName: dansk (Danmark)
Calendar: 
  MinSupportedDateTime: 01-01-0001 00:00:00
  MaxSupportedDateTime: 31-12-9999 23:59:59
  AlgorithmType: SolarCalendar
  CalendarType: Localized
  Eras: 
    - 1
  TwoDigitYearMax: 2029
OptionalCalendars: 
  - 
    MinSupportedDateTime: 01-01-0001 00:00:00
    MaxSupportedDateTime: 31-12-9999 23:59:59
    AlgorithmType: SolarCalendar
    CalendarType: Localized
    Eras: 
      - 1
    TwoDigitYearMax: 2029
TextInfo: 
  ANSICodePage: 1252
  OEMCodePage: 850
  MacCodePage: 10000
  EBCDICCodePage: 20277
  LCID: 1030
  CultureName: da-DK
  ListSeparator: ;
  IsRightToLeft: False
ThreeLetterISOLanguageName: dan
ThreeLetterWindowsLanguageName: DAN
TwoLetterISOLanguageName: da
UseUserOverride: True
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
    NegativeInfinitySymbol: ""-∞""
    NegativeSign: -
    NumberDecimalDigits: 2
    NumberDecimalSeparator: ,
    NumberGroupSeparator: .
    CurrencyPositivePattern: 3
    PositiveInfinitySymbol: ∞
    PositiveSign: +
    PercentDecimalDigits: 2
    PercentDecimalSeparator: ,
    PercentGroupSeparator: .
    PercentSymbol: %
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
    AMDesignator: 
    Calendar: 
      MinSupportedDateTime: 01-01-0001 00:00:00
      MaxSupportedDateTime: 31-12-9999 23:59:59
      AlgorithmType: SolarCalendar
      CalendarType: Localized
      Eras: 
        - 1
      TwoDigitYearMax: 2029
    DateSeparator: -
    FirstDayOfWeek: Monday
    CalendarWeekRule: FirstFourDayWeek
    FullDateTimePattern: d. MMMM yyyy HH:mm:ss
    LongDatePattern: d. MMMM yyyy
    LongTimePattern: HH:mm:ss
    MonthDayPattern: d. MMMM
    PMDesignator: 
    RFC1123Pattern: ddd, dd MMM yyyy HH':'mm':'ss 'GMT'
    ShortDatePattern: dd-MM-yyyy
    ShortTimePattern: HH:mm
    SortableDateTimePattern: yyyy'-'MM'-'dd'T'HH':'mm':'ss
    TimeSeparator: :
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
      - 
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
      - 
    NativeCalendarName: dansk (Danmark)
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
      - 
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
      - 
  DisplayName: Danish (Denmark)
  EnglishName: Danish (Denmark)";
            }
            else
            {
                expected = expected.ReplaceAll("gregoriansk", "Gregoriansk", StringComparison.Ordinal);
            }
#endif

            TestOutput.WriteLines(sut4);

            Assert.Equal(expected.ReplaceLineEndings().Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList(), sut4);
        }
    }
}
