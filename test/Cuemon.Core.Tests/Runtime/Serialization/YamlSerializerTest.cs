using System.Globalization;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Runtime.Serialization
{
    public class YamlSerializerTest : Test
    {
        public YamlSerializerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Serialize_ShouldSerializeDateFormatInfo()
        {
            var sut1 = new YamlSerializer();
            var sut2 = new CultureInfo("da-DK");
            var sut3 = sut1.Serialize(sut2.DateTimeFormat);
            var sut4 = sut3.ToEncodedString();

            TestOutput.WriteLine(sut4);

            Assert.Equal(@"AMDesignator: 
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
  - søn
  - man
  - tir
  - ons
  - tor
  - fre
  - lør
ShortestDayNames: 
  - S
  - M
  - T
  - O
  - T
  - F
  - L
DayNames: 
  - søndag
  - mandag
  - tirsdag
  - onsdag
  - torsdag
  - fredag
  - lørdag
AbbreviatedMonthNames: 
  - jan.
  - feb.
  - mar.
  - apr.
  - maj
  - jun.
  - jul.
  - aug.
  - sep.
  - okt.
  - nov.
  - dec.
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
  - jan.
  - feb.
  - mar.
  - apr.
  - maj
  - jun.
  - jul.
  - aug.
  - sep.
  - okt.
  - nov.
  - dec.
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
  - ", sut4, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void Serialize_ShouldSerializeNumberFormatInfo()
        {
            var sut1 = new YamlSerializer();
            var sut2 = new CultureInfo("da-DK");
            var sut3 = sut1.Serialize(sut2.NumberFormat);
            var sut4 = sut3.ToEncodedString();

            TestOutput.WriteLine(sut4);

            Assert.Equal(@"CurrencyDecimalDigits: 2
CurrencyDecimalSeparator: ,
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
DigitSubstitution: None", sut4, ignoreLineEndingDifferences: true);
        }


        [Fact]
        public void Serialize_ShouldSerializeCultureInfo()
        {
            var sut1 = new YamlSerializer();
            var sut2 = new CultureInfo("da-DK");
            var sut3 = sut1.Serialize(sut2);
            var sut4 = sut3.ToEncodedString();

            TestOutput.WriteLine(sut4);

            Assert.Equal(@"LCID: 1030
KeyboardLayoutId: 1030
Name: da-DK
IetfLanguageTag: da-DK
DisplayName: Danish (Denmark)
NativeName: dansk (Danmark)
EnglishName: Danish (Denmark)
TwoLetterISOLanguageName: da
ThreeLetterISOLanguageName: dan
ThreeLetterWindowsLanguageName: DAN
CompareInfo: 
  Name: da-DK
  Version: 
    FullVersion: 136734873
    SortId: 08266899-0000-0000-0000-000000000406
  LCID: 1030
TextInfo: 
  ANSICodePage: 1252
  OEMCodePage: 850
  MacCodePage: 10000
  EBCDICCodePage: 20277
  LCID: 1030
  CultureName: da-DK
  ListSeparator: ;
  IsRightToLeft: False
IsNeutralCulture: False
CultureTypes: SpecificCultures, InstalledWin32Cultures
NumberFormat: 
  CurrencyDecimalDigits: 2
  CurrencyDecimalSeparator: ,
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
    - søn
    - man
    - tir
    - ons
    - tor
    - fre
    - lør
  ShortestDayNames: 
    - S
    - M
    - T
    - O
    - T
    - F
    - L
  DayNames: 
    - søndag
    - mandag
    - tirsdag
    - onsdag
    - torsdag
    - fredag
    - lørdag
  AbbreviatedMonthNames: 
    - jan.
    - feb.
    - mar.
    - apr.
    - maj
    - jun.
    - jul.
    - aug.
    - sep.
    - okt.
    - nov.
    - dec.
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
    - jan.
    - feb.
    - mar.
    - apr.
    - maj
    - jun.
    - jul.
    - aug.
    - sep.
    - okt.
    - nov.
    - dec.
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
UseUserOverride: True", sut4, ignoreLineEndingDifferences: true);
        }
    }
}
