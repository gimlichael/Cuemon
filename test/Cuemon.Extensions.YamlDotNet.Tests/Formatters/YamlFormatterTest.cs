using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.YamlDotNet.Assets;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.NamingConventions;

namespace Cuemon.Extensions.YamlDotNet.Formatters
{
    public class YamlFormatterTest : Test
    {
        public YamlFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void YamlFormatter_ShouldDeserializeUsingFactory()
        {
            var yaml = """
                       ---
                       title: Title A
                       summary: Summary A
                       ---
                       title: Title B
                       summary: Summary B
                       ---
                       title: Title C
                       summary: Summary C
                       """;

            List<Book> books = new List<Book>();

            YamlFormatter.DeserializeObject(yaml.ToStream(), (serializer, parser) =>
            {
                parser.Consume<StreamStart>();

                while (parser.Accept<DocumentStart>(out _))
                {
                    books.Add(serializer.Deserialize<Book>(parser));
                }
            });

            Assert.Equal(3, books.Count);
            Assert.Collection(books, book =>
            {
                Assert.Equal("Title A", book.Title);
                Assert.Equal("Summary A", book.Summary);
            }, book =>
            {
                Assert.Equal("Title B", book.Title);
                Assert.Equal("Summary B", book.Summary);
            }, book =>
            {
                Assert.Equal("Title C", book.Title);
                Assert.Equal("Summary C", book.Summary);
            });
        }

        [Fact]
        public void YamlFormatter_ShouldSerializeToYamlCollection()
        {
            var sut = Generate.RangeOf<Book>(10, i => new Book()
            {
                Title = Alphanumeric.Letters[i].ToString(),
                Summary = i.ToString()
            });

            var yaml = YamlFormatter.SerializeObject(sut, o =>
            {
                o.Settings.NamingConvention = PascalCaseNamingConvention.Instance;
            });
            var yamlString = yaml.ToEncodedString();

            TestOutput.WriteLine(yamlString);

            Assert.Equal("""
                         - Title: A
                           Summary: 0
                         - Title: B
                           Summary: 1
                         - Title: C
                           Summary: 2
                         - Title: D
                           Summary: 3
                         - Title: E
                           Summary: 4
                         - Title: F
                           Summary: 5
                         - Title: G
                           Summary: 6
                         - Title: H
                           Summary: 7
                         - Title: I
                           Summary: 8
                         - Title: J
                           Summary: 9

                         """.ReplaceLineEndings(), yamlString.ReplaceLineEndings());
        }

        [Fact]
        public void YamlFormatter_ShouldSerializeBookToYaml()
        {
            var sut = new Book()
            {
                Title = "A",
                Summary = "0"
            };

            var yaml = YamlFormatter.SerializeObject(sut, o =>
            {
                o.Settings.NamingConvention = PascalCaseNamingConvention.Instance;
            });
            var yamlString = yaml.ToEncodedString();

            TestOutput.WriteLine(yamlString);

            Assert.Equal("""
                         Title: A
                         Summary: 0
                         
                         """.ReplaceLineEndings(), yamlString.ReplaceLineEndings());
        }

        [Fact]
        public void YamlFormatter_ShouldDeserializeFromYamlToBook()
        {


            var book = YamlFormatter.DeserializeObject<Book>("""
                                                             title: A
                                                             summary: 0
                                                             """.ToStream());

            Assert.Equal("A", book.Title);
            Assert.Equal("0", book.Summary);
        }

        [Fact]
        public void YamlFormatter_ShouldSerializeArgumentOutOfRangeExceptionToYaml()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() => Validator.ThrowIfGreaterThan(5, 1, "argument"));

            var yaml = YamlFormatter.SerializeObject(sut, o =>
            {
                o.Settings.NamingConvention = CamelCaseNamingConvention.Instance;
                o.SensitivityDetails = FaultSensitivityDetails.All;
                o.Settings.WhiteSpaceIndentation = 4;
                o.Settings.IndentSequences = false;
            });

            var yamlString = yaml.ToEncodedString();

            TestOutput.WriteLine(yamlString);

#if NET48_OR_GREATER
#if DEBUG
            Assert.True(Match("""
                                     type: System.ArgumentOutOfRangeException
                                     source: Cuemon.Core
                                     message: |-
                                         Specified arguments x is greater than y.
                                         Parameter name: argument
                                         Actual value was 5 > 1.
                                     stack:
                                     - at Cuemon.Validator.ThrowIfGreaterThan[T](T x, T y, String paramName, String message) *
                                     - at Cuemon.Extensions.YamlDotNet.Formatters.YamlFormatterTest.<>c.<YamlFormatter_ShouldSerializeArgumentOutOfRangeExceptionToYaml>*
                                     - at Xunit.Assert.RecordException(Action testCode) *
                                     rangeMessage: Specified argument was out of the range of valid values.
                                     paramName: argument

                                     """.ReplaceLineEndings(), yamlString, o => o.ThrowOnNoMatch = true));
#else
            Assert.True(Match("""
                              type: System.ArgumentOutOfRangeException
                              source: Cuemon.Core
                              message: |-
                                  Specified arguments x is greater than y.
                                  Parameter name: argument
                                  Actual value was 5 > 1.
                              stack:
                              - at Cuemon.Validator.ThrowIfGreaterThan[T](T x, T y, String paramName, String message) *
                              - at Xunit.Assert.RecordException(Action testCode) *
                              rangeMessage: Specified argument was out of the range of valid values.
                              paramName: argument

                              """.ReplaceLineEndings(), yamlString, o => o.ThrowOnNoMatch = true));
#endif
#else
            Assert.True(Match("""
                         type: System.ArgumentOutOfRangeException
                         source: Cuemon.Core
                         message: |-
                             Specified arguments x is greater than y. (Parameter 'argument')
                             Actual value was 5 > 1.
                         stack:
                         - at Cuemon.Validator.ThrowIfGreaterThan[T](T x, T y, String paramName, String message) *
                         - at Cuemon.Extensions.YamlDotNet.Formatters.YamlFormatterTest.<>c.<YamlFormatter_ShouldSerializeArgumentOutOfRangeExceptionToYaml>*
                         - at Xunit.Assert.RecordException(Action testCode) *
                         paramName: argument
                         
                         """.ReplaceLineEndings(), yamlString, o => o.ThrowOnNoMatch = true));
#endif

        }
    }
}
