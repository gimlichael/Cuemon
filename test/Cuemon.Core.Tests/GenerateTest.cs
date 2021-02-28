using System;
using System.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class GenerateTest : Test
    {
        public GenerateTest(ITestOutputHelper output) : base(output)
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

        [Fact]
        public void HashCode32_ShouldGenerateSameHashCode()
        {
            var hc1 = Generate.HashCode32(1, 2, 3, 4, 5);
            var hc2 = Generate.HashCode32(10, TimeSpan.FromSeconds(5).Ticks, TimeSpan.FromSeconds(15).Ticks, TimeSpan.FromSeconds(2).Ticks, "Class1.SomeMethod()");
            Assert.Equal(-3271143, hc1);
            Assert.Equal(1191125869, hc2);
        }
    }
}