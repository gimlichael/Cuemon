using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class DayPartTest : Test
    {
        public DayPartTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Night_ShouldBeInRangeOfTwentyOneToThree()
        {
            var sut = DayPart.Night;

            Assert.Equal("Night", sut.Name);
            Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(6));
            Assert.Equal(sut.Range.Start, DateTime.UtcNow.Date.AddHours(-3));
            Assert.Equal(sut.Range.End, DateTime.UtcNow.Date.AddHours(3));
        }

        [Fact]
        public void Morning_ShouldBeInRangeOfThreeToNine()
        {
            var sut = DayPart.Morning;

            Assert.Equal("Morning", sut.Name);
            Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(6));
            Assert.Equal(sut.Range.Start, DateTime.UtcNow.Date.AddHours(3));
            Assert.Equal(sut.Range.End, DateTime.UtcNow.Date.AddHours(9));
        }

        [Fact]
        public void Forenoon_ShouldBeInRangeOfNineToTwelve()
        {
            var sut = DayPart.Forenoon;

            Assert.Equal("Forenoon", sut.Name);
            Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(3));
            Assert.Equal(sut.Range.Start, DateTime.UtcNow.Date.AddHours(9));
            Assert.Equal(sut.Range.End, DateTime.UtcNow.Date.AddHours(12));
        }

        [Fact]
        public void Afternoon_ShouldBeInRangeOfTwelveToEighteen()
        {
            var sut = DayPart.Afternoon;

            Assert.Equal("Afternoon", sut.Name);
            Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(6));
            Assert.Equal(sut.Range.Start, DateTime.UtcNow.Date.AddHours(12));
            Assert.Equal(sut.Range.End, DateTime.UtcNow.Date.AddHours(18));
        }

        [Fact]
        public void Evening_ShouldBeInRangeOfEighteenToTwentyOne()
        {
            var sut = DayPart.Evening;

            Assert.Equal("Evening", sut.Name);
            Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(3));
            Assert.Equal(sut.Range.Start, DateTime.UtcNow.Date.AddHours(18));
            Assert.Equal(sut.Range.End, DateTime.UtcNow.Date.AddHours(21));
        }

        [Fact]
        public void Ctor_ShouldBeInRangeOfFourToSixInTheMorning()
        {
            var sut = new DayPart("Dawn", new TimeRange(DateTime.UtcNow.Date.AddHours(4), DateTime.UtcNow.Date.AddHours(6)));

            Assert.Equal("Dawn", sut.Name);
            Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(2));
            Assert.Equal(sut.Range.Start, DateTime.UtcNow.Date.AddHours(4));
            Assert.Equal(sut.Range.End, DateTime.UtcNow.Date.AddHours(6));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenExceedingTwentyFourHoursDuration()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() => new DayPart("Custom", new TimeRange(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddHours(24).AddMilliseconds(1))));

            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void Ctor_ShouldListAndVerifyAllBuiltInDayParts()
        {
            var sut = DayPart.All;

            Assert.Collection(sut,
                part => Assert.Equal(part.Name, nameof(DayPart.Night)),
                part => Assert.Equal(part.Name, nameof(DayPart.Morning)),
                part => Assert.Equal(part.Name, nameof(DayPart.Forenoon)),
                part => Assert.Equal(part.Name, nameof(DayPart.Afternoon)),
                part => Assert.Equal(part.Name, nameof(DayPart.Evening))
                );

            var all = DelimitedString.Create(sut.Select(part => part.ToString()), o => o.Delimiter = Alphanumeric.NewLine);

            TestOutput.WriteLine(all);
        }
    }
}