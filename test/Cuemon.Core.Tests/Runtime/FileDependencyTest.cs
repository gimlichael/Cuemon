using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Xunit;

namespace Cuemon.Runtime
{
    public class FileDependencyTest : Test
    {
        public FileDependencyTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldNotInitializeFileWatcher()
        {
            var sut1 = $"{Directory.GetCurrentDirectory()}\\UnitTest1.txt";
            var sut2 = new Lazy<FileWatcher>(() => new FileWatcher(sut1));
            var sut3 = new FileDependency(sut2);

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality.");

            Assert.False(sut2.IsValueCreated);
            Assert.False(sut3.HasChanged);
            Assert.Null(sut3.UtcLastModified);
        }

        [Fact]
        public async Task StartAsync_ShouldReceiveTwoSignalsFromFileWatcher()
        {
            var ce = new CountdownEvent(2);
            var sut1 = $"{Directory.GetCurrentDirectory()}\\UnitTest2.txt";
            var sut2 = new Lazy<FileWatcher>(() => new FileWatcher(sut1, false, o => o.Period = TimeSpan.FromMilliseconds(800)));
            var sut3 = new FileDependency(sut2);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                ce.Signal();
            });

            sut3.DependencyChanged += sut6;

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality.");

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality."); // should trigger last modified

            await Task.Delay(TimeSpan.FromSeconds(1));

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality."); // should trigger last modified

            var signaled = ce.Wait(TimeSpan.FromSeconds(15));

            TestOutput.WriteLine(sut5.ToDelimitedString());

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(15));
            Assert.Equal(2, sut5.Count);
        }

        [Fact]
        public async Task StartAsync_ShouldReceiveOnlyOneSignalFromFileWatcher()
        {
            var are = new AutoResetEvent(false);
            var sut1 = $"{Directory.GetCurrentDirectory()}\\UnitTest3.txt";
            var sut2 = new Lazy<FileWatcher>(() => new FileWatcher(sut1, false, o => o.Period = TimeSpan.FromMilliseconds(800)));
            var sut3 = new FileDependency(sut2, true);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                are.Set();
            });

            sut3.DependencyChanged += sut6;

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality.");

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality."); // should trigger last modified

            await Task.Delay(TimeSpan.FromSeconds(1));

            File.WriteAllText(sut1, "Unit Test is key to ensure high code quality."); // should trigger last modified

            var signaled = are.WaitOne(TimeSpan.FromSeconds(15));

            TestOutput.WriteLine(sut5.ToDelimitedString());

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(5));
            Assert.Equal(1, sut5.Count);
        }

    }
}