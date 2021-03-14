using System.Threading;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class AsyncOptionsTest : Test
    {
        public AsyncOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldInitializeCancellationTokenToDefault()
        {
            var sut = new AsyncOptions();

            Assert.Equal(sut.CancellationToken, CancellationToken.None);
        }
    }
}