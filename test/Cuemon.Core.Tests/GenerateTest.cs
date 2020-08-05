using System.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core
{
    public class GenerateTest : Test
    {
        public GenerateTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void RandomString_ShouldGenerateUniqueStringsOfSpecifiedLength()
        {
            var length = 256;
            var strings = new List<string>();
            for (var i = 0; i < 1024; i++)
            {
                strings.Add(Generate.RandomString(length));
            }
            Assert.All(strings, s => Assert.True(s.Length == length));
            Assert.All(strings, s => Assert.Single(strings, s));
        }
    }
}