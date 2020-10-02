using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions
{
    public class ValidatorExtensionsTest : Test
    {
        public ValidatorExtensionsTest(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public void IfHasDifference_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIf.HasDifference("aaabbbccc", "dddeeefff", "paramName");
            });
        }

        [Fact]
        public void IfHasNotDifference_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIf.NoDifference("aaabbbccc", "cccbbbbaaaa", "paramName");
            });
        }
    }
}