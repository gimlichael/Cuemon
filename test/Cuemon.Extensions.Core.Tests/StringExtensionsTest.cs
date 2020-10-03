using System;
using System.Globalization;
using System.Linq;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions
{
    public class StringExtensionsTest : Test
    {
        public StringExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Difference_ShouldGetDifference()
        {
            var s1 = Alphanumeric.UppercaseLetters;
            var s2 = Alphanumeric.Letters;
            var s3 = s1.Difference(s2);

            Assert.Equal(Alphanumeric.LowercaseLetters, s3);
        }

        [Fact]
        public void ToCharArray_ShouldConvertStringToCharArray()
        {
            var s1 = Alphanumeric.LettersAndNumbers;
            var c1 = s1.ToCharArray();

            Assert.Equal(s1, string.Concat(c1));
        }

        [Fact]
        public void ToByteArray_ShouldConvertStringToByteArray()
        {
            var s1 = Alphanumeric.LettersAndNumbers;
            var b1 = s1.ToByteArray();

            Assert.Equal(s1, Convertible.ToString(b1));
        }

        [Fact]
        public void FromUrlEncodedBase64String_ShouldConvertUrlEncodedBase64StringToByteArray()
        {
            var s1 = "VGhpcyBpcyBhIHRlc3Qgd2l0aCBzcGVjaWFsIGNoYXJhY3RlcnMgISIjwqQlJi8oKUAhPQ";
            var b1 = s1.FromUrlEncodedBase64();
            var s2 = Convertible.ToString(b1);

            Assert.Equal("This is a test with special characters !\"#¤%&/()@!=", s2);
        }

        [Fact]
        public void FromBinaryDigits_ShouldConvertBinaryDigitsStringToByteArray()
        {
            var s1 = "01010100011010000110100101110011001000000110100101110011001000000110000100100000011101000110010101110011011101000010000001110111011010010111010001101000001000000111001101110000011001010110001101101001011000010110110000100000011000110110100001100001011100100110000101100011011101000110010101110010011100110010000000100001001000100010001111000010101001000010010100100110001011110010100000101001010000000010000100111101";
            var b1 = s1.FromBinaryDigits();
            var s2 = Convertible.ToString(b1);

            Assert.Equal("This is a test with special characters !\"#¤%&/()@!=", s2);
        }

        [Fact]
        public void FromBase64_ShouldConvertBase64StringToByteArray()
        {
            var s1 = "VGhpcyBpcyBhIHRlc3Qgd2l0aCBzcGVjaWFsIGNoYXJhY3RlcnMgISIjwqQlJi8oKUAhPQ==";
            var b1 = s1.FromBase64();
            var s2 = Convertible.ToString(b1);

            Assert.Equal("This is a test with special characters !\"#¤%&/()@!=", s2);
        }

        [Fact]
        public void ToCasing_ShouldConvertToDifferentCasingMethods()
        {
            var s1 = "Cuemon for .net";
            var s2 = s1.ToCasing();
            var s3 = s1.ToCasing(CasingMethod.LowerCase);
            var s4 = s1.ToCasing(CasingMethod.TitleCase);
            var s5 = s1.ToCasing(CasingMethod.UpperCase);
            
            Assert.Equal(s1, s2);
            Assert.Equal(s1.ToLowerInvariant(), s3);
            Assert.Equal(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s1), s4);
            Assert.Equal(s1.ToUpperInvariant(), s5);
        }
        
        [Fact]
        public void ToUri_ShouldConvertToUri()
        {
            var s1 = "https://www.cuemon.net/";
            var u1 = s1.ToUri();

            Assert.Equal(s1, u1.OriginalString);
        }

        [Fact]
        public void IsNullOrEmpty_ShouldGiveFalseOnAtLeastOneNullOrEmpty()
        {
            var l1 = Generate.RangeOf(5, i =>
            {
                if (i < 4) { return $"{i}"; }
                return null;
            });

            var l2 = Generate.RangeOf(5, i =>
            {
                if (i < 4) { return $"{i}"; }
                return "";
            });

            var l3 = Generate.RangeOf(5, i => $"{i}");

            Assert.True(l1.IsNullOrEmpty());
            Assert.True(l2.IsNullOrEmpty());
            Assert.False(l3.IsNullOrEmpty());
        }

        [Fact]
        public void Count_ShouldCountSpecifiedChar()
        {
            Assert.Equal(10, string.Concat(Alphanumeric.LettersAndNumbers, new string('a', 9)).Count('a'));
            Assert.Equal(10, string.Concat(Alphanumeric.LettersAndNumbers, new string('Z', 9)).Count('Z'));
        }

        [Fact]
        public void JsEscape_ShouldDoJavascriptEscape()
        {
            Assert.Equal("Need%20complemental%20framework%20for%20.NET%3F%20Use%20Cuemon%20for%20.NET%21", "Need complemental framework for .NET? Use Cuemon for .NET!".JsEscape());
        }

        [Fact]
        public void JsUnescape_ShouldDoJavascriptUnescape()
        {
            Assert.Equal("Need complemental framework for .NET? Use Cuemon for .NET!", "Need%20complemental%20framework%20for%20.NET%3F%20Use%20Cuemon%20for%20.NET%21".JsUnescape());
        }

        private static readonly string SentenceWithCuemonInIt = "Cuemon is following most of the Framework Design Guidelines.";
        private static readonly string SentenceWithForInIt = "Let's live for freedom!";
        private static readonly string SentenceWithDotNetInIt = "Microsoft .NET - one platform to rule them all.";

        [Fact]
        public void ContainsAny()
        {
            Assert.True("Cuemon for .NET".ContainsAny(SentenceWithCuemonInIt.Split(' ')));
            Assert.True("Cuemon for .NET".ContainsAny(SentenceWithForInIt.Split(' ')));
            Assert.True("Cuemon for .NET".ContainsAny(SentenceWithDotNetInIt.Split(' ')));
            Assert.False("Cuemon for .NET".ContainsAny("some", "random", "words"));
        }

        [Fact]
        public void ContainsAll()
        {
            Assert.True("Cuemon for .NET".ContainsAll("Cuemon", "for", ".NET"));
            Assert.False("Cuemon for .NET".ContainsAll("Cuemon", "for", "all"));
        }

        [Fact]
        public void EqualsAny()
        {
            Assert.True("Cuemon for .NET".EqualsAny("some sentence", "Cuemon for .NET"));
            Assert.False("Cuemon for .NET".EqualsAny(SentenceWithCuemonInIt.Split(' ')));
            Assert.False("Cuemon for .NET".EqualsAny(SentenceWithForInIt.Split(' ')));
            Assert.False("Cuemon for .NET".EqualsAny(SentenceWithDotNetInIt.Split(' ')));
            Assert.False("Cuemon for .NET".EqualsAny("some", "random", "words"));
        }

        [Fact]
        public void StartsWith()
        {
            Assert.True("Cuemon for .NET".StartsWith("some sentence", "Cuemon for .NET"));
            Assert.True("Cuemon for .NET".StartsWith(SentenceWithCuemonInIt.Split(' ')));
            Assert.False("Cuemon for .NET".StartsWith(SentenceWithForInIt.Split(' ')));
            Assert.False("Cuemon for .NET".StartsWith(SentenceWithDotNetInIt.Split(' ')));
            Assert.False("Cuemon for .NET".StartsWith("some", "random", "words"));
        }

        [Fact]
        public void ToGuid_ShouldConvertStringToGuid()
        {
            var g1 = Guid.NewGuid();
            var s1 = g1.ToString("N");
            var g2 = s1.ToGuid(o => o.Formats = GuidFormats.N);

            Assert.Equal(g1, g2);
        }

        [Fact]
        public void SplitDelimited_ShouldRevertDelimitedStringToSequence()
        {
            var i = Generate.RangeOf(10, i1 => i1.ToString());
            var s = "0,1,2,3,4,5,6,7,8,9".SplitDelimited().ToList();

            Assert.Equal(10, s.Count);
            Assert.True(s.SequenceEqual(i));
        }
    }
}