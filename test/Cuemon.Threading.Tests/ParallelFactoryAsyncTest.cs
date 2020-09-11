using System;
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
    public class ParallelFactoryAsyncTest : Test
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromMinutes(15));
        private readonly int _extremePartitionSize = 2048;

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
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = 64;
            });

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
                    if (i > 450) { cts.Cancel(); }
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
            }, o => o.CancellationToken = _cts.Token);

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
            }, o => o.PartitionSize = _extremePartitionSize);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForResultAsync_ShouldRunConcurrent()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();

            var result = await ParallelFactory.ForResultAsync(0, count, async (i, ct) =>
            {
                await Task.Delay(50, ct);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = 64;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForResultAsync_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await ParallelFactory.ForResultAsync(0, count, async (i, ct) =>
                {
                    if (i > 450) { cts.Cancel(); }
                    await Task.Delay(Generate.RandomNumber(25, 75), ct);
                    cb.Add(i);
                    return i;
                }, o => o.CancellationToken = cts.Token);
            });

            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForResultAsync_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var cb = new ConcurrentBag<int>();

            var result = await ParallelFactory.ForResultAsync(0, count, async (i, ct) =>
            {
                await Task.Delay(1000, ct);
                cb.Add(i);
                return i;
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForResultAsync_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var cb = new ConcurrentBag<int>();

            var result = await ParallelFactory.ForResultAsync(0, count, async (i, ct) =>
            {
                await Task.Delay(100, ct);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = _extremePartitionSize;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachAsync_ShouldRunConcurrent()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            await ParallelFactory.ForEachAsync(ic, async (i, ct) =>
            {
                await Task.Delay(50, ct);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = 64;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachAsync_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await ParallelFactory.ForEachAsync(ic, async (i, ct) =>
                {
                    if (i > 450) { cts.Cancel(); }
                    await Task.Delay(Generate.RandomNumber(25, 75), ct);
                    cb.Add(i);
                }, o => o.CancellationToken = cts.Token);
            });

            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachAsync_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            await ParallelFactory.ForEachAsync(ic, async (i, ct) =>
            {
                await Task.Delay(1000, ct);
                cb.Add(i);
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachAsync_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            await ParallelFactory.ForEachAsync(ic, async (i, ct) =>
            {
                await Task.Delay(100, ct);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = _extremePartitionSize;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachResultAsync_ShouldRunConcurrent()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            var result = await ParallelFactory.ForEachResultAsync(ic, async (i, ct) =>
            {
                await Task.Delay(50, ct);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = 64;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachResultAsync_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await ParallelFactory.ForEachResultAsync(ic, async (i, ct) =>
                {
                    if (i > 450) { cts.Cancel(); }
                    await Task.Delay(Generate.RandomNumber(25, 75), ct);
                    cb.Add(i);
                    return i;
                }, o => o.CancellationToken = cts.Token);
            });

            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachResultAsync_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            var result = await ParallelFactory.ForEachResultAsync(ic, async (i, ct) =>
            {
                await Task.Delay(1000, ct);
                cb.Add(i);
                return i;
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task ForEachResultAsync_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            var result = await ParallelFactory.ForEachResultAsync(ic, async (i, ct) =>
            {
                await Task.Delay(100, ct);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = _extremePartitionSize;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileAsync_ShouldRunConcurrent()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            await AdvancedParallelFactory.WhileAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
            {
                await Task.Delay(50, ct);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = 64;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileAsync_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await AdvancedParallelFactory.WhileAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
                {
                    if (i > 450) { cts.Cancel(); }
                    await Task.Delay(Generate.RandomNumber(25, 75), ct);
                    cb.Add(i);
                }, o => o.CancellationToken = cts.Token);
            });

            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileAsync_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            await AdvancedParallelFactory.WhileAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
            {
                await Task.Delay(1000, ct);
                cb.Add(i);
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileAsync_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            await AdvancedParallelFactory.WhileAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
            {
                await Task.Delay(100, ct);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = _extremePartitionSize;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileResultAsync_ShouldRunConcurrent()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            var result = await AdvancedParallelFactory.WhileResultAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
            {
                await Task.Delay(50, ct);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = 64;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileResultAsync_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await AdvancedParallelFactory.WhileResultAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
                {
                    if (i > 450) { cts.Cancel(); }
                    await Task.Delay(Generate.RandomNumber(25, 75), ct);
                    cb.Add(i);
                    return i;
                }, o => o.CancellationToken = cts.Token);
            });

            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileResultAsync_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            var result = await AdvancedParallelFactory.WhileResultAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
            {
                await Task.Delay(1000, ct);
                cb.Add(i);
                return i;
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public async Task WhileResultAsync_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            var result = await AdvancedParallelFactory.WhileResultAsync(ic, () => Task.FromResult(ic.TryPeek(out _)), intProvider => intProvider.Dequeue(), async (i, ct) =>
            {
                await Task.Delay(100, ct);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.PartitionSize = _extremePartitionSize;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }
    }
}