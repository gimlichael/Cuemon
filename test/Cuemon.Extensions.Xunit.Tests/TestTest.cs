using System;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Cuemon.Extensions.Xunit
{
    public class TestTest : Test
    {
        private const string ExpectedStringValue = "AllIsGood";
        private bool _onDisposeManagedResourcesCalled;

        public TestTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void Test_ShouldHaveTestOutput()
        {
            Assert.True(HasTestOutput);
            Assert.IsAssignableFrom<ITestOutputHelper>(TestOutput);
            Assert.IsType<TestOutputHelper>(TestOutput);
        }

        [Fact]
        public void Test_ShouldInvokeDispose()
        {
            Assert.Equal(ExpectedStringValue, DisposeSensitiveMethod());
            Assert.False(_onDisposeManagedResourcesCalled);
            Dispose();
            Assert.True(_onDisposeManagedResourcesCalled);
            Assert.True(Disposed);
            Assert.Throws<ObjectDisposedException>(DisposeSensitiveMethod);
        }

        public string DisposeSensitiveMethod()
        {
            if (Disposed) { throw new ObjectDisposedException(GetType().FullName); }
            return ExpectedStringValue;
        }

        protected override void OnDisposeManagedResources()
        {
            _onDisposeManagedResourcesCalled = true;
            base.OnDisposeManagedResources();
        }
    }
}