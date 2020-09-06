using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class WhileAsyncTest : Test
    {
        public WhileAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task WhileAsyncTest_ShouldRunOn1000Threads()
        {
            var cb = new ConcurrentBag<int>();
            var fakeReader = new ConcurrentQueue<int>(Generate.RangeOf(1000, i => i));
            await ParallelFactory.WhileAsync(fakeReader, () => Task.FromResult(fakeReader.TryPeek(out _)), cq => cq.TryDequeue(out var x), i =>
            {
                Thread.Sleep(500); // todo: refactor to true async method
                cb.Add(Thread.CurrentThread.ManagedThreadId);
            }, o => o.PartitionSize = 1000);

            Assert.Equal(1000, cb.Count);
            Assert.Equal(1000, cb.Distinct().Count());
        }
    }
}