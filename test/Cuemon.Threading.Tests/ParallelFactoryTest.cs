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
    public class ParallelFactoryTest : Test
    {
        public ParallelFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void For_ShouldRunConcurrent()
        {
            var atMostExpectedCount = 500;
            var count = 1000;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.For(0, count, i =>
            {
                Thread.Sleep(50);
                cb.Add(i);
            }, o => o.CreationOptions = TaskCreationOptions.None);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)));
        }

        [Fact]
        public void For_ShouldRunConcurrent_IgniteCancellation()
        {
            var count = 1000;
            var cb = new ConcurrentBag<int>();
            var cts = new CancellationTokenSource();
            var x = 0;
            Assert.Throws<AggregateException>(() =>
            {
                ParallelFactory.For(0, count, i =>
                {
                    Interlocked.Increment(ref x);
                    if (i > 500) { cts.Cancel(); }
                    Thread.Sleep(Generate.RandomNumber(25, 75));
                    cb.Add(i);
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.CreationOptions = TaskCreationOptions.None;
                });
            });

            Thread.Sleep(500); // wait for possible background threads being canceled

            TestOutput.WriteLine(x.ToString());
            TestOutput.WriteLine($"Threads processed: {cb.Count}.");

            Assert.InRange(cb.Count, 300, 600); // most threads should have executed before cancellation
            Assert.True(Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i)), "Generate.RangeOf(cb.Count, i => i).SequenceEqual(cb.OrderBy(i => i))");
        }

        [Fact]
        public void For_ShouldRunConcurrent_LongRunning_SystemPartition()
        {
            var count = sbyte.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.For(0, count, i =>
            {
                Thread.Sleep(1000);
                cb.Add(i);
            });

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)));
        }

        [Fact]
        public void For_ShouldRunConcurrent_LongRunning_ExtremePartition()
        {
            var count = short.MaxValue;
            var expected = Generate.RangeOf(count, i => i);
            var cb = new ConcurrentBag<int>();

            ParallelFactory.For(0, count, i =>
            {
                Thread.Sleep(100);
                cb.Add(i);
            }, o => o.PartitionSize = 4096);

            Assert.Equal(count, cb.Count);
            Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
        }


    }
}