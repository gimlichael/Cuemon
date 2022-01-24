using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class SuccessfulValueTest : Test
    {
        public SuccessfulValueTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_SucceededShouldBeTrue()
        {
            var sut = new SuccessfulValue();
            Assert.True(sut.Succeeded);
        }

        [Fact]
        public void Ctor_SucceededShouldBeTrueWithExpectedResult()
        {
            var value = Guid.NewGuid();
            var sut = new SuccessfulValue<Guid>(value);
            
            Assert.True(sut.Succeeded);
            Assert.Equal(value, sut.Result);
        }
    }
}
