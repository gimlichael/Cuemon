using System;
using System.IO;
using Cuemon.Extensions.Xunit;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    public class ValidatorExtensionsTest : Test
    {
        public ValidatorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void InvalidJsonDocument_ShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() => Validator.ThrowIf.InvalidJsonDocument($$"""{ "id" "{{Guid.NewGuid():D}}"  }""", "paramName"));

            Assert.Equal("paramName", sut.ParamName);
            Assert.StartsWith("Value must be a JSON representation that complies with RFC 8259.", sut.Message);

            var json = $$"""{ "id" "{{Guid.NewGuid():D}}"  }""";
            sut = Assert.Throws<ArgumentException>(() => Validator.ThrowIf.InvalidJsonDocument(json));

            Assert.Equal("json", sut.ParamName);
            Assert.StartsWith("Value must be a JSON representation that complies with RFC 8259.", sut.Message);

            var reader = GetJsonReader($$"""{ "id" "{{Guid.NewGuid():D}}"  }""");
            sut = Assert.Throws<ArgumentException>(() => Validator.ThrowIf.InvalidJsonDocument(ref reader));
            Assert.Equal("reader", sut.ParamName);
            Assert.StartsWith("Value must be a JSON representation that complies with RFC 8259.", sut.Message);
        }

        [Fact]
        public void InvalidJsonDocument_ShouldNotThrowArgumentException()
        {
            Validator.ThrowIf.InvalidJsonDocument($$"""{ "id": "{{Guid.NewGuid():D}}"  }""");
            var reader = GetJsonReader($$"""{ "id": "{{Guid.NewGuid():D}}"  }""");
            Validator.ThrowIf.InvalidJsonDocument(ref reader);
        }

        private JsonReader GetJsonReader(string json)
        {
            var sr = new StringReader(json);
            return new JsonTextReader(sr);
        }
    }
}
