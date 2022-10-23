using System;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.DependencyInjection
{
    public class ServiceOptionsTest : Test
    {
        public ServiceOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ValidateOptions_ShouldThrowInvalidOperationExceptionForNestedTypePredicate()
        {
            var sut1 = new ServiceOptions
            {
                NestedTypePredicate = null
            };

            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NestedTypePredicate == null')", sut2.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowInvalidOperationExceptionForNestedTypeSelector()
        {
            var sut1 = new ServiceOptions
            {
                NestedTypeSelector = null
            };

            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NestedTypeSelector == null')", sut2.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldHaveDefaultValues()
        {
            var sut = new ServiceOptions();

            Assert.NotNull(sut.NestedTypePredicate);
            Assert.NotNull(sut.NestedTypeSelector);
            Assert.False(sut.UseNestedTypeForwarding);
            Assert.Equal(ServiceLifetime.Transient, sut.Lifetime);
        }
    }
}
