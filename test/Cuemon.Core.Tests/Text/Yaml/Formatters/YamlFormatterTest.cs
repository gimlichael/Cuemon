using System;
using Cuemon.Assets;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Text.Yaml.Formatters
{
    public class YamlFormatterTest : Test
    {
        public YamlFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void YamlFormatter_ShouldSerializeToYamlCollection()
        {
            var sut = Generate.RangeOf<Book>(10, i => new Book()
            {
                Title = Alphanumeric.Letters[i].ToString(),
                Summary = i.ToString()
            });

            var yaml = YamlFormatter.SerializeObject(sut);
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
        public void YamlFormatter_ShouldSerializeToYaml()
        {
            var sut = new Book()
            {
                Title = "A",
                Summary = "0"
            };

            var yaml = YamlFormatter.SerializeObject(sut);
            var yamlString = yaml.ToEncodedString();

            TestOutput.WriteLine(yamlString);

            Assert.Equal("""
                         Title: A
                         Summary: 0
                         """.ReplaceLineEndings(), yamlString.ReplaceLineEndings());
        }
    }
}
