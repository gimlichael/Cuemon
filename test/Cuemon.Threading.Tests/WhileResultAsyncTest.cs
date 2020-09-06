using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class WhileResultAsyncTest : Test
    {
        public WhileResultAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task WhileResultAsyncTest_ShouldRunOn1000Threads()
        {
            var cb = new ConcurrentBag<int>();
            var fakeReader = new ConcurrentQueue<int>(Generate.RangeOf(1000, i => i));
            var result = await ParallelFactory.WhileResultAsync(fakeReader, () => Task.FromResult(fakeReader.TryPeek(out _)), cq =>
            {
                if (cq.TryDequeue(out var x))
                {
                    return x;
                }
                return -1;
            }, i =>
            {
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