using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions
{
    public class AsyncDisposableTest : Test
    {
        public AsyncDisposableTest(ITestOutputHelper output) : base(output)
        {
        }

        private class TestAsyncDisposable : AsyncDisposable
        {
            public bool ManagedResourcesDisposed { get; private set; }

            protected override ValueTask OnDisposeManagedResourcesAsync()
            {
                ManagedResourcesDisposed = true;
                return default;
            }
        }

        [Fact]
        public async Task DisposeAsync_ShouldCallOnDisposeManagedResourcesAsync()
        {
            // Arrange
            var disposable = new TestAsyncDisposable();

            // Act
            await disposable.DisposeAsync();

            // Assert
            Assert.True(disposable.ManagedResourcesDisposed);
        }

        [Fact]
        public async Task DisposeAsync_ShouldSuppressFinalize()
        {
            // Arrange
            var disposable = new TestAsyncDisposable();

            // Act
            await disposable.DisposeAsync();

            // Assert
            Assert.True(disposable.Disposed);
        }
    }
}
