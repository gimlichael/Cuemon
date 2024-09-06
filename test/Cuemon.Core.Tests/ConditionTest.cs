using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class ConditionTest : Test
    {
        public ConditionTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void HasDifference_ShouldProvideDifferenceBetweenFirstAndSecond()
        {
            var sut1 = "Cuemon for .NET";
            var sut2 = "There once was a library named Cuemon for .NET; it is getting better by the day!";
            var sut3 = "XYZ Cuemon for .NET ÆØÅ";
            var sut5 = Condition.HasDifference(sut1, sut2, out var sut4);
            var sut6 = Condition.HasDifference(sut1, sut1, out _);
            var sut8 = Condition.HasDifference(sut1, sut3, out var sut7);

            TestOutput.WriteLine(sut4);
            TestOutput.WriteLine(sut7);

            Assert.Equal("hcwaslibyd;tg!", sut4);
            Assert.True(sut5);
            Assert.False(sut6);
            Assert.Equal("XYZÆØÅ", sut7);
            Assert.True(sut8);
        }
    }
}
