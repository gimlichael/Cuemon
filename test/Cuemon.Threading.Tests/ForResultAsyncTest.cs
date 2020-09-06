using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class ForResultAsyncTest : Test
    {
        public ForResultAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ForResultAsyncTest_ShouldRunOn1000Threads()
        {
            var cb = new ConcurrentBag<int>();
            var result = await ParallelFactory.ForResultAsync(0, 1000, i =>            {
                Thread.Sleep(50); // todo: refactor to true async method
                cb.Add(Thread.CurrentThread.ManagedThreadId);
                return i;
            }, o => o.PartitionSize = 64);

            Assert.Equal(1000, result.Count);
            Assert.Equal(1000, result.Distinct().Count());
            Assert.Equal(0, result.Min());
            Assert.Equal(999, result.Max());
            Assert.Equal(0, result.First());
            Assert.Equal(999, result.Last());

            Assert.Equal(1000, cb.Count);
        }
    }
}