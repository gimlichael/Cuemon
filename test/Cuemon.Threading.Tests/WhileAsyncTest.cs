using System.Collections.Concurrent;
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
            await AdvancedParallelFactory.WhileAsync(fakeReader, () => Task.FromResult(fakeReader.TryPeek(out _)), cq => cq.TryDequeue(out var x), i =>
            {
                Thread.Sleep(50); // todo: refactor to true async method
                cb.Add(Thread.CurrentThread.ManagedThreadId);
            }, o => o.PartitionSize = 64);

            Assert.Equal(1000, cb.Count);
        }
    }
}