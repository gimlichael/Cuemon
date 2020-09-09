using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class ForEachAsyncTest : Test
    {
        public ForEachAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ForEachAsyncTest_ShouldRunOn1000Threads()
        {
            var ic = Generate.RangeOf(1000, i => i);
            var cb = new ConcurrentBag<int>();
            await ParallelFactory.ForEachAsync(ic, i =>
            {
                Thread.Sleep(50); // todo: refactor to true async method
                cb.Add(Thread.CurrentThread.ManagedThreadId);
            }, o => o.PartitionSize = 64);

            Assert.Equal(1000, cb.Count);
        }
    }
}