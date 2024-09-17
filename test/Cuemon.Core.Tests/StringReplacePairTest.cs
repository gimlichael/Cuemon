﻿using System;
using System.Linq;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class StringReplacePairTest : Test
    {
        private static readonly string LoremIpsum = Decorator.Enclose(typeof(StringReplacePairTest).Assembly).GetManifestResources("LoremIpsum.txt", ManifestResourceMatch.ContainsName).Single().Value.ToEncodedString();

        public StringReplacePairTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ReplaceAll_ShouldReplaceAllOccurrencesOfOldValueWithNewValue_UsingOrdinalComparison()
        {
            var comparison = StringComparison.Ordinal;
            var loremIpsumWords = LoremIpsum.Split(' ');
            var cuemonCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("cuemon", comparison));
            var utCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("ut", comparison));
            var sut = StringReplacePair.ReplaceAll(LoremIpsum, "ut", "cuemon", comparison);
            var sutWords = sut.Split(' ');
            var utCountAfterSut = sutWords.Count(s => s.ContainsAny("ut", comparison));
            var cuemonCountAfterSut = sutWords.Count(s => s.ContainsAny("cuemon", comparison));
            var loremIpsumDifference = sutWords.Except(loremIpsumWords).ToList();
            var sutDifference = loremIpsumWords.Except(sutWords).ToList();


            Assert.NotEqual(loremIpsumWords, sutWords);

            Assert.Equal(loremIpsumDifference.Select(s => s.ReplaceAll("cuemon", "", comparison)), sutDifference.Select(s => s.ReplaceAll("ut", "", comparison)));
            Assert.Equal(loremIpsumDifference.Count, sutDifference.Count);
            Assert.Equal(loremIpsumWords.Length, sutWords.Length);
            Assert.Equal(utCountBeforeSut, cuemonCountAfterSut);
            Assert.Equal(cuemonCountBeforeSut, utCountAfterSut);

            TestOutput.WriteLine(sut);
        }

        [Fact]
        public void ReplaceAll_ShouldReplaceAllOccurrencesOfOldValueWithNewValue()
        {
            var comparison = StringComparison.OrdinalIgnoreCase;
            var loremIpsumWords = LoremIpsum.Split(' ');
            var cuemonCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("cuemon", comparison));
            var utCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("ut", comparison));
            var sut = StringReplacePair.ReplaceAll(LoremIpsum, "ut", "cuemon"); // default is ordinal-ignore-case
            var sutWords = sut.Split(' ');
            var utCountAfterSut = sutWords.Count(s => s.ContainsAny("ut", comparison));
            var cuemonCountAfterSut = sutWords.Count(s => s.ContainsAny("cuemon", comparison));
            var loremIpsumDifference = sutWords.Except(loremIpsumWords).ToList();
            var sutDifference = loremIpsumWords.Except(sutWords).ToList();

            Assert.NotEqual(loremIpsumWords, sutWords);
            Assert.NotEqual(loremIpsumDifference.Select(s => s.ReplaceAll("cuemon", "", comparison)), sutDifference.Select(s => s.ReplaceAll("ut", "", comparison)));
            Assert.NotEqual(loremIpsumDifference.Count, sutDifference.Count);

            Assert.Equal(loremIpsumWords.Length, sutWords.Length);
            Assert.Equal(utCountBeforeSut, cuemonCountAfterSut);
            Assert.Equal(cuemonCountBeforeSut, utCountAfterSut);

            TestOutput.WriteLine(sut);
        }

        [Fact]
        public void RemoveAll_ShouldRemoveAllOccurrencesOfFragments_UsingOrdinalIgnoreCaseComparison()
        {
            var comparison = StringComparison.OrdinalIgnoreCase;
            var loremIpsumWords = LoremIpsum.Split(' ');
            var ametCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("amet", comparison));
            var scelerisqueCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("scelerisque", comparison));
            var sut = StringReplacePair.RemoveAll(LoremIpsum, comparison, "amet", "scelerisque");
            var sutWords = sut.Split(' ');
            var ametCountAfterSut = sutWords.Count(s => s.ContainsAny("amet", comparison));
            var scelerisqueCountAfterSut = sutWords.Count(s => s.ContainsAny("scelerisque", comparison));

            Assert.NotEqual(loremIpsumWords, sutWords);

            Assert.Equal(loremIpsumWords.Length, sutWords.Length);

            Assert.Equal(111, ametCountBeforeSut);
            Assert.Equal(0, ametCountAfterSut);
            Assert.Equal(53, scelerisqueCountBeforeSut);
            Assert.Equal(0, scelerisqueCountAfterSut);

            TestOutput.WriteLine(sut);
        }

        [Fact]
        public void RemoveAll_ShouldRemoveAllOccurrencesOfFragments()
        {
            var comparison = StringComparison.Ordinal;
            var loremIpsumWords = LoremIpsum.Split(' ');
            var ametCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("amet", comparison));
            var scelerisqueCountBeforeSut = loremIpsumWords.Count(s => s.ContainsAny("scelerisque", comparison));
            var sut = StringReplacePair.RemoveAll(LoremIpsum, "amet", "scelerisque"); // default is ordinal
            var sutWords = sut.Split(' ');
            var ametCountAfterSut = sutWords.Count(s => s.ContainsAny("amet", comparison));
            var scelerisqueCountAfterSut = sutWords.Count(s => s.ContainsAny("scelerisque", comparison));

            Assert.NotEqual(loremIpsumWords, sutWords);

            Assert.Equal(loremIpsumWords.Length, sutWords.Length);

            Assert.Equal(100, ametCountBeforeSut);
            Assert.Equal(0, ametCountAfterSut);
            Assert.Equal(49, scelerisqueCountBeforeSut);
            Assert.Equal(0, scelerisqueCountAfterSut);

            TestOutput.WriteLine(sut);
        }
    }
}