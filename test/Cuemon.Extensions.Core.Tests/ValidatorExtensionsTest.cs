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

        [Fact]
        public void ContainsReservedKeyword_ShouldThrowReservedKeywordException()
        {
            var sut = Assert.Throws<ArgumentReservedKeywordException>(() =>
            {
                var resKw = "dj bobo";
                var resKwList = new string[]
                {
                    "rene",
                    "baumann",
                    "dj bobo"
                };
                Validator.ThrowIf.ContainsReservedKeyword(resKw, resKwList);
            });

            Assert.Equal("resKw", sut.ParamName);
            Assert.Equal("dj bobo", sut.ActualValue);
            Assert.StartsWith("Specified argument is a reserved keyword.", sut.Message);
        }

        [Fact]
        public void ContainsReservedKeyword_WithEqualityComparer_ShouldThrowReservedKeywordException()
        {
            var resKw = "DJ BOBO";
            var resKwList = new string[]
            {
                "rene",
                "baumann",
                "dj bobo"
            };

            Validator.ThrowIf.ContainsReservedKeyword(resKw, resKwList); // should not throw as we are using EqualityComparer<string>.Default

            var sut = Assert.Throws<ArgumentReservedKeywordException>(() =>
            {
                Validator.ThrowIf.ContainsReservedKeyword(resKw, resKwList, StringComparer.OrdinalIgnoreCase);
            });

            Assert.Equal("resKw", sut.ParamName);
            Assert.Equal("DJ BOBO", sut.ActualValue);
            Assert.StartsWith("Specified argument is a reserved keyword.", sut.Message);
        }

        [Fact]
        public void ContainsAny_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var argument = "dj bobo\tis the best 90ies artist!";
                var characters = new[] { ' ', Alphanumeric.TabChar };
                Validator.ThrowIf.ContainsAny(argument, characters);
            });

            Assert.Equal("argument", sut.ParamName);
            Assert.Equal("' ','\t'", sut.ActualValue);
            Assert.StartsWith("One or more character matches were found.", sut.Message);
        }

        [Fact]
        public void NotContainsAny_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var argument = "dj bobo\tis the best 90ies artist!";
                var characters = new[] { Alphanumeric.LinefeedChar, Alphanumeric.CaretChar };
                Validator.ThrowIf.NotContainsAny(argument, characters);
            });

            Assert.Equal("argument", sut.ParamName);
            Assert.Equal("'\n','^'", sut.ActualValue);
            Assert.StartsWith("No matching characters were found.", sut.Message);
        }

        [Fact]
        public void ContainsAny_ShouldNotThrowAnyException()
        {
            var argument = "dj bobo\tis the best 90ies artist!";
            var characters = new[] { Alphanumeric.LinefeedChar, Alphanumeric.CaretChar };
            Validator.ThrowIf.ContainsAny(argument, characters);
        }

        [Fact]
        public void NotContainsAny_ShouldNotThrowAnyException()
        {
            var argument = "dj bobo\tis the best 90ies artist!";
            var characters = new[] { Alphanumeric.TabChar, ' ' };
            Validator.ThrowIf.NotContainsAny(argument, characters);
        }
    }
}
