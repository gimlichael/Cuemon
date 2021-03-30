﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Collections.Generic
{
    public class CollectionExtensionsTest : Test
    {
        public CollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToPartitioner_ShouldIterateOneHundredTwentyEightItems_WhileHavingPartitions()
        {
            var sut1 = Enumerable.Range(0, 1024).ToList().ToPartitioner();
            var sut2 = new List<int> { sut1.IteratedCount };

            while (sut1.HasPartitions)
            {
                foreach (var item in sut1)
                {

                }
                sut2.Add(sut1.IteratedCount);
            }

            Assert.False(sut1.HasPartitions);
            Assert.Equal(128, sut1.PartitionSize);
            Assert.Collection(sut2, 
                i => Assert.Equal(0, i),
                i => Assert.Equal(128, i),
                i => Assert.Equal(256, i),
                i => Assert.Equal(384, i),
                i => Assert.Equal(512, i),
                i => Assert.Equal(640, i),
                i => Assert.Equal(768, i),
                i => Assert.Equal(896, i),
                i => Assert.Equal(1024, i));
        }

        [Fact]
        public void ToPartitioner_ShouldIterateTwoHundredFiftySixItems_WhileHavingPartitions()
        {
            var sut1 = Enumerable.Range(0, 1024).ToList().ToPartitioner(256);
            var sut2 = new List<int> { sut1.IteratedCount };

            while (sut1.HasPartitions)
            {
                foreach (var item in sut1)
                {

                }
                sut2.Add(sut1.IteratedCount);
            }

            Assert.False(sut1.HasPartitions);
            Assert.Equal(256, sut1.PartitionSize);
            Assert.Collection(sut2, 
                i => Assert.Equal(0, i),
                i => Assert.Equal(256, i),
                i => Assert.Equal(512, i),
                i => Assert.Equal(768, i),
                i => Assert.Equal(1024, i));
        }

        [Fact]
        public void AddRange_ShouldAddNineItems_ByParamsArray_UsingGenericList()
        {
            var sut1 = new List<int>();
            sut1.AddRange(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.Collection(sut1, 
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i));
        }

        [Fact]
        public void AddRange_ShouldAddNineItems_ByParamsArray_UsingGenericCollection()
        {
            var sut1 = new Collection<int>();
            sut1.AddRange(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.Collection(sut1, 
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i));
        }
    }
}