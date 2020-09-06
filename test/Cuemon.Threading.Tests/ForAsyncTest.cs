using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class ForAsyncTest : Test
    {
        public ForAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ForAsync_ShouldRunOn1000Threads()
        {
            var cb = new ConcurrentBag<int>();
            await ParallelFactory.ForAsync(0, 1000, i =>
            {
                Thread.Sleep(50); // todo: refactor to true async method
                cb.Add(Thread.CurrentThread.ManagedThreadId);
            }, o => o.PartitionSize = 64);

            Assert.Equal(1000, cb.Count);
        }
    }
}