using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class ForEachResultAsyncTest : Test
    {
        public ForEachResultAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ForEachResultAsyncTest_ShouldRunOn1000Threads()
        {
            var ic = Generate.RangeOf(1000, i => i);
            var cb = new ConcurrentBag<int>();
            var result = await ParallelFactory.ForEachResultAsync(ic, i =>
            {
                Thread.Sleep(500); // todo: refactor to true async method
                cb.Add(Thread.CurrentThread.ManagedThreadId);
                return i;
            }, o => o.PartitionSize = 1000);

            Assert.Equal(1000, result.Count);
            Assert.Equal(1000, result.Distinct().Count());
            Assert.Equal(0, result.Min());
            Assert.Equal(999, result.Max());
            Assert.Equal(0, result.First());
            Assert.Equal(999, result.Last());

            Assert.Equal(1000, cb.Count);
            Assert.Equal(1000, cb.Distinct().Count());
        }
    }
}