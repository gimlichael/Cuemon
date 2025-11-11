using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Collections.Generic
{
    public class ListExtensionsTest : Test
    {
        public ListExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Remove_ShouldRemoveTheSpecifiedItemFromList()
        {
            var sut1 = Enumerable.Range(0, 1024).ToList();
            var sut2 = new List<bool>();

            for (var i = 1018; i < 1028; i++)
            {
                sut2.Add(sut1.Remove(item => item == i));
            }

            Assert.Equal(1018, sut1.Count);
            Assert.Collection(sut2,
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(false, i),
                i => Assert.Equal(false, i),
                i => Assert.Equal(false, i),
                i => Assert.Equal(false, i));
        }

        [Fact]
        public void HasIndex_ShouldVerifyIfAnIndexIsValidWithinTheList()
        {
            var sut1 = Enumerable.Range(0, 1024).ToList();
            var sut2 = new List<bool>();

            for (var i = 1018; i < 1028; i++)
            {
                sut2.Add(sut1.HasIndex(i));
            }

            Assert.Equal(1024, sut1.Count);
            Assert.Collection(sut2,
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(true, i),
                i => Assert.Equal(false, i),
                i => Assert.Equal(false, i),
                i => Assert.Equal(false, i),
                i => Assert.Equal(false, i));
        }

        [Fact]
        public void Next_ShouldPeekForwardUntilDefaultWithinTheList()
        {
            var sut1 = Generate.RangeOf<int?>(1024, i => i - 1).ToList();
            var sut2 = new List<int?>();

            for (var i = 1018; i < 1028; i++)
            {
                sut2.Add(sut1.Next(i));
            }

            Assert.Equal(1024, sut1.Count);
            Assert.Collection(sut2,
                i => Assert.Equal(1018, i),
                i => Assert.Equal(1019, i),
                i => Assert.Equal(1020, i),
                i => Assert.Equal(1021, i),
                i => Assert.Equal(1022, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i));
        }

        [Fact]
        public void Previous_ShouldPeekBackwardUntilDefaultWithinTheList()
        {
            var sut1 = Generate.RangeOf<int?>(1024, i => i - 1).ToList();
            var sut2 = new List<int?>();

            for (var i = 1020; i < 1030; i++)
            {
                sut2.Add(sut1.Previous(i));
            }

            Assert.Equal(1024, sut1.Count);
            Assert.Collection(sut2,
                i => Assert.Equal(1018, i),
                i => Assert.Equal(1019, i),
                i => Assert.Equal(1020, i),
                i => Assert.Equal(1021, i),
                i => Assert.Equal(1022, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i),
                i => Assert.Equal(null, i));
        }
    }
}