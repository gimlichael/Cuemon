using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class ParallelFactoryAsyncTest : Test
    {
        public ParallelFactoryAsyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ForAsync_ShouldRunConcurrent()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            await ParallelFactory.ForAsync(0, count, async (i, ct) =>
            {
                await Task.Delay(50, ct);
                cb.Add(i);
            }, o => o.PartitionSize = 64);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForAsync_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await ParallelFactory.ForAsync(0, count, async (i, ct) =>
                {
                    if (i > 500) { cts.Cancel(); }
                    await Task.Delay(Generate.RandomNumber(25, 75), ct);
                    cb.Add(i);
                }, o => o.CancellationToken = cts.Token);
            });

            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForAsync_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            await ParallelFactory.ForAsync(0, count, async (i, ct) =>
            {
                await Task.Delay(1000, ct);
                cb.Add(i);
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForAsync_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            await ParallelFactory.ForAsync(0, count, async (i, ct) =>
            {
                await Task.Delay(100, ct);
                cb.Add(i);
            }, o => o.PartitionSize = 4096);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }
    }
}