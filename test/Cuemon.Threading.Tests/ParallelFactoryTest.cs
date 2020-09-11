using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class ParallelFactoryTest : Test
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        private readonly int _extremePartitionSize = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 512 : 4096;
        private readonly int _longRunningTaskInMs = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 100 : 1000;

        public ParallelFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void For_ShouldRunConcurrent()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.For(0, count, i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void For_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            var ae = Assert.Throws<AggregateException>(() =>
            {
                ParallelFactory.For(0, count, i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 450) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
        }

        [Fact]
        public void For_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.For(0, count, i =>
            {
                Thread.Sleep(_longRunningTaskInMs);
                cb.Add(i);
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void For_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.For(0, count, i =>
            {
                Thread.Sleep(1);
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
        public void ForResult_ShouldRunConcurrent()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();

            var result = ParallelFactory.ForResult(0, count, i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void ForResult_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            var ae = Assert.Throws<AggregateException>(() =>
            {
                ParallelFactory.ForResult(0, count, i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 450) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                    return i;
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
        }

        [Fact]
        public void ForResult_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var cb = new ConcurrentBag<int>();

            var result = ParallelFactory.ForResult(0, count, i =>
            {
                Thread.Sleep(_longRunningTaskInMs);
                cb.Add(i);
                return i;
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void ForResult_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var cb = new ConcurrentBag<int>();

            var result = ParallelFactory.ForResult(0, count, i =>
            {
                Thread.Sleep(100);
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
        public void ForEach_ShouldRunConcurrent()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.ForEach(ic, i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void ForEach_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            var ae = Assert.Throws<AggregateException>(() =>
            {
                ParallelFactory.ForEach(ic, i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 450) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
        }

        [Fact]
        public void ForEach_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.ForEach(ic, i =>
            {
                Thread.Sleep(_longRunningTaskInMs);
                cb.Add(i);
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void ForEach_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.ForEach(ic, i =>
            {
                Thread.Sleep(100);
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
        public void ForEachResult_ShouldRunConcurrent()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            var result = ParallelFactory.ForEachResult(ic, i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void ForEachResult_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            var ae = Assert.Throws<AggregateException>(() =>
            {
                ParallelFactory.ForEachResult(ic, i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 450) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                    return i;
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
        }

        [Fact]
        public void ForEachResult_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            var result = ParallelFactory.ForEachResult(ic, i =>
            {
                Thread.Sleep(_longRunningTaskInMs);
                cb.Add(i);
                return i;
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void ForEachResult_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var ic = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            var result = ParallelFactory.ForEachResult(ic, i =>
            {
                Thread.Sleep(100);
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
        public void While_ShouldRunConcurrent()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            AdvancedParallelFactory.While(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void While_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            var ae = Assert.Throws<AggregateException>(() =>
            {
                AdvancedParallelFactory.While(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 450) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
        }

        [Fact]
        public void While_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            AdvancedParallelFactory.While(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
            {
                Thread.Sleep(_longRunningTaskInMs);
                cb.Add(i);
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void While_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            AdvancedParallelFactory.While(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
            {
                Thread.Sleep(100);
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
        public void WhileResult_ShouldRunConcurrent()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            var result = AdvancedParallelFactory.WhileResult(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = _cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void WhileResult_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            var ae = Assert.Throws<AggregateException>(() =>
            {
                AdvancedParallelFactory.WhileResult(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 450) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                    return i;
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
        }

        [Fact]
        public void WhileResult_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            var result = AdvancedParallelFactory.WhileResult(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
            {
                Thread.Sleep(_longRunningTaskInMs);
                cb.Add(i);
                return i;
            }, o => o.CancellationToken = _cts.Token);

            Assert.Equal(count, cb.Count);
            Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void WhileResult_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var ic = new Queue<int>(expected);
            var cb = new ConcurrentBag<int>();

            var result = AdvancedParallelFactory.WhileResult(ic, () => ic.TryPeek(out _), intProvider => intProvider.Dequeue(), i =>
            {
                Thread.Sleep(100);
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