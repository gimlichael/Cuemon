using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class TemplateTest : Test
    {
        public TemplateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateZero_ShouldCreateEmpty()
        {
            var sut = Template.CreateZero();
            Assert.True(sut.IsEmpty);
            Assert.Empty(sut.ToArray());
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateSingle()
        {
            var sut = Template.CreateOne(1);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(), o => Assert.Equal(o, 1));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateDouble()
        {
            var sut = Template.CreateTwo(1, 2);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateTriple()
        {
            var sut = Template.CreateThree(1, 2, 3);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateQuadruple()
        {
            var sut = Template.CreateFour(1, 2, 3, 4);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateQuintuple()
        {
            var sut = Template.CreateFive(1, 2, 3, 4, 5);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateSextuple()
        {
            var sut = Template.CreateSix(1, 2, 3, 4, 5, 6);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateSeptuple()
        {
            var sut = Template.CreateSeven(1, 2, 3, 4, 5, 6, 7);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateOctuple()
        {
            var sut = Template.CreateEight(1, 2, 3, 4, 5, 6, 7, 8);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateNonuple()
        {
            var sut = Template.CreateNine(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateDecuple()
        {
            var sut = Template.CreateTen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateUndecuple()
        {
            var sut = Template.CreateEleven(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateDuodecuple()
        {
            var sut = Template.CreateTwelve(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateTredecuple()
        {
            var sut = Template.CreateThirteen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateQuattuordecuple()
        {
            var sut = Template.CreateFourteen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateQuindecuple()
        {
            var sut = Template.CreateFifteen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14),
                o => Assert.Equal(o, 15));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateSexdecuple()
        {
            var sut = Template.CreateSixteen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14),
                o => Assert.Equal(o, 15),
                o => Assert.Equal(o, 16));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateSeptendecuple()
        {
            var sut = Template.CreateSeventeen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14),
                o => Assert.Equal(o, 15),
                o => Assert.Equal(o, 16),
                o => Assert.Equal(o, 17));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateOctodecuple()
        {
            var sut = Template.CreateEighteen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14),
                o => Assert.Equal(o, 15),
                o => Assert.Equal(o, 16),
                o => Assert.Equal(o, 17),
                o => Assert.Equal(o, 18));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateNovemdecuple()
        {
            var sut = Template.CreateNineteen(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14),
                o => Assert.Equal(o, 15),
                o => Assert.Equal(o, 16),
                o => Assert.Equal(o, 17),
                o => Assert.Equal(o, 18),
                o => Assert.Equal(o, 19));
            TestOutput.WriteLine(sut.ToString());
        }

        [Fact]
        public void CreateZero_ShouldCreateViguple()
        {
            var sut = Template.CreateTwenty(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            Assert.False(sut.IsEmpty);
            Assert.Collection(sut.ToArray(),
                o => Assert.Equal(o, 1),
                o => Assert.Equal(o, 2),
                o => Assert.Equal(o, 3),
                o => Assert.Equal(o, 4),
                o => Assert.Equal(o, 5),
                o => Assert.Equal(o, 6),
                o => Assert.Equal(o, 7),
                o => Assert.Equal(o, 8),
                o => Assert.Equal(o, 9),
                o => Assert.Equal(o, 10),
                o => Assert.Equal(o, 11),
                o => Assert.Equal(o, 12),
                o => Assert.Equal(o, 13),
                o => Assert.Equal(o, 14),
                o => Assert.Equal(o, 15),
                o => Assert.Equal(o, 16),
                o => Assert.Equal(o, 17),
                o => Assert.Equal(o, 18),
                o => Assert.Equal(o, 19),
                o => Assert.Equal(o, 20));
            TestOutput.WriteLine(sut.ToString());
        }
    }
}