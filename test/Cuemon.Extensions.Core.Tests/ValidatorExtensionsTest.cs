using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Core
{
    public class ValidatorExtensionsTest : Test
    {
        public ValidatorExtensionsTest(ITestOutputHelper output = null) : base(output)
        {
            
        }

        [Fact]
        public void IfHasDistinctDifference_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.Throw.IfHasDistinctDifference("aaabbbccc", "dddeeefff", "paramName");
            });
        }

        [Fact]
        public void IfHasNotDistinctDifference_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.Throw.IfHasNotDistinctDifference("aaabbbccc", "cccbbbbaaaa", "paramName");
            });
        }

        [Fact]
        public void IfHasDifference_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.Throw.IfHasDifference("aaabbbccc", "dddeeefff", "paramName");
            });
        }

        [Fact]
        public void IfHasNotDifference_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.Throw.IfHasNotDifference("aaabbbccc", "cccbbbbaaaa", "paramName");
            });
        }
    }
}