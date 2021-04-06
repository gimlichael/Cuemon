using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions
{
    public class DateTimeExtensionsTest : Test
    {
        public DateTimeExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToUnixEpochTime_ShouldConvertToDoubleUnixEpochTime()
        {
            var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
            var sut2 = sut1.ToUnixEpochTime();
            var sut3 = sut2.FromUnixEpochTime().ToLocalTime();

            TestOutput.WriteLine(sut2.ToString());

            Assert.Equal(sut1, sut3);
        }

        [Fact]
        public void ToUtcKind_ShouldRepresentLocalTimeAsUtcWithoutConversion()
        {
            var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
            var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
            var sut3 = sut1.ToUtcKind();

            Assert.NotEqual(sut1.ToUniversalTime(), sut2);
            Assert.Equal(sut2, sut3);
        }

        [Fact]
        public void ToLocalKind_ShouldRepresentUtcTimeAsLocalWithoutConversion()
        {
            var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
            var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
            var sut3 = sut2.ToLocalKind();

            Assert.NotEqual(sut1.ToUniversalTime(), sut2);
            Assert.Equal(sut1, sut3);
        }

        [Fact]
        public void ToDefaultKind_ShouldRepresentUtcTimeAsDefaultWithoutConversion()
        {
            var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
            var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
            var sut3 = sut1.ToDefaultKind();
            var sut4 = sut2.ToDefaultKind();

            Assert.NotEqual(sut1.ToUniversalTime(), sut2);
            Assert.Equal(sut3, sut4);
        }

        [Fact]
        public void IsWithinRange_DateShouldBeWithinSpecifiedRangeDate()
        {
            var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
            var sut2 = sut1.AddHours(-6);
            var sut3 = sut1.AddHours(6);
            var sut4 = sut1.IsWithinRange(sut2, sut3);
            var sut5 = sut1.IsWithinRange(sut2.AddHours(7), sut3.AddHours(-7));

            Assert.InRange(sut1, sut2, sut3);
            Assert.NotInRange(sut1, sut2.AddHours(7), sut3.AddHours(-7));
            Assert.True(sut4);
            Assert.False(sut5);
        }

        [Fact]
        public void IsWithinRange_DateShouldBeWithinSpecifiedRangeOfTime()
        {
            var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
            var sut2 = new DateTimeRange(sut1.AddHours(-6), sut1.AddHours(6));
            var sut3 = sut1.IsWithinRange(sut2);
            var sut4 = sut1.IsWithinRange(sut2.Start.AddHours(7), sut2.End.AddHours(-7));

            Assert.InRange(sut1, sut2.Start, sut2.End);
            Assert.NotInRange(sut1, sut2.Start.AddHours(7), sut2.End.AddHours(-7));
            Assert.True(sut3);
            Assert.False(sut4);
        }

        [Fact]
        public void IsTimeOfDayNight_ShouldBeFrom2100To0300()
        {
            var sut1 = new DateTime(2021, 4, 6, 19, 44, 37, DateTimeKind.Utc);
            var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
            var sut3 = new DateTime(2021, 4, 6, 2, 44, 37, DateTimeKind.Utc);
            var sut4 = new DateTime(2021, 4, 6, 4, 44, 37, DateTimeKind.Utc);

            TestOutput.WriteLine(DayPart.Night.ToString());

            Assert.False(sut1.IsTimeOfDayNight());
            Assert.True(sut2.IsTimeOfDayNight());
            Assert.True(sut3.IsTimeOfDayNight());
            Assert.False(sut4.IsTimeOfDayNight());
        }

        [Fact]
        public void IsTimeOfDayMorning_ShouldBeFrom0300To0900()
        {
            var sut1 = new DateTime(2021, 4, 6, 2, 44, 37, DateTimeKind.Utc);
            var sut2 = new DateTime(2021, 4, 6, 4, 44, 37, DateTimeKind.Utc);
            var sut3 = new DateTime(2021, 4, 6, 8, 44, 37, DateTimeKind.Utc);
            var sut4 = new DateTime(2021, 4, 6, 10, 44, 37, DateTimeKind.Utc);

            TestOutput.WriteLine(DayPart.Morning.ToString());

            Assert.False(sut1.IsTimeOfDayMorning());
            Assert.True(sut2.IsTimeOfDayMorning());
            Assert.True(sut3.IsTimeOfDayMorning());
            Assert.False(sut4.IsTimeOfDayMorning());
        }

        [Fact]
        public void IsTimeOfDayForenoon_ShouldBeFrom0900To1200()
        {
            var sut1 = new DateTime(2021, 4, 6, 8, 44, 37, DateTimeKind.Utc);
            var sut2 = new DateTime(2021, 4, 6, 10, 44, 37, DateTimeKind.Utc);
            var sut3 = new DateTime(2021, 4, 6, 11, 44, 37, DateTimeKind.Utc);
            var sut4 = new DateTime(2021, 4, 6, 12, 44, 37, DateTimeKind.Utc);

            TestOutput.WriteLine(DayPart.Forenoon.ToString());

            Assert.False(sut1.IsTimeOfDayForenoon());
            Assert.True(sut2.IsTimeOfDayForenoon());
            Assert.True(sut3.IsTimeOfDayForenoon());
            Assert.False(sut4.IsTimeOfDayForenoon());
        }

        [Fact]
        public void IsTimeOfDayAfternoon_ShouldBeFrom1200To1800()
        {
            var sut1 = new DateTime(2021, 4, 6, 11, 44, 37, DateTimeKind.Utc);
            var sut2 = new DateTime(2021, 4, 6, 12, 44, 37, DateTimeKind.Utc);
            var sut3 = new DateTime(2021, 4, 6, 17, 44, 37, DateTimeKind.Utc);
            var sut4 = new DateTime(2021, 4, 6, 18, 44, 37, DateTimeKind.Utc);

            TestOutput.WriteLine(DayPart.Afternoon.ToString());

            Assert.False(sut1.IsTimeOfDayAfternoon());
            Assert.True(sut2.IsTimeOfDayAfternoon());
            Assert.True(sut3.IsTimeOfDayAfternoon());
            Assert.False(sut4.IsTimeOfDayAfternoon());
        }

        [Fact]
        public void IsTimeOfDayEvening_ShouldBeFrom1800To2100()
        {
            var sut1 = new DateTime(2021, 4, 6, 17, 44, 37, DateTimeKind.Utc);
            var sut2 = new DateTime(2021, 4, 6, 18, 44, 37, DateTimeKind.Utc);
            var sut3 = new DateTime(2021, 4, 6, 20, 44, 37, DateTimeKind.Utc);
            var sut4 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);

            TestOutput.WriteLine(DayPart.Evening.ToString());

            Assert.False(sut1.IsTimeOfDayEvening());
            Assert.True(sut2.IsTimeOfDayEvening());
            Assert.True(sut3.IsTimeOfDayEvening());
            Assert.False(sut4.IsTimeOfDayEvening());
        }
    }
}