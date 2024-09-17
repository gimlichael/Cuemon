﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Threading
{
    public class AsyncOptionsTest : Test
    {
        public AsyncOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldInitializeCancellationTokenToDefault()
        {
            var sut = new AsyncOptions();

            Assert.Equal(sut.CancellationToken, CancellationToken.None);
        }

        [Fact]
        public async Task AsyncOptions_ShouldThrow_OperationCanceledException()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(250);
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await SomeMethod(o => o.CancellationToken = cts.Token));
        }

        private async Task SomeMethod(Action<AsyncOptions> setup)
        {
            var options = setup.Configure();
            while (!options.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(50);
            }
            options.CancellationToken.ThrowIfCancellationRequested();
            TestOutput.WriteLine(options.CancellationToken.IsCancellationRequested.ToString());
        }
    }
}