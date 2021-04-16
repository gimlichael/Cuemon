using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Http
{
    public class UnsupportedMediaTypeTest : Test
    {
        public UnsupportedMediaTypeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf404()
        {
            var sut = new UnsupportedMediaTypeException();

            TestOutput.WriteLine(sut.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, sut);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as UnsupportedMediaTypeException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                Assert.Equal(sut.StatusCode, desEx.StatusCode);
                Assert.Equal(sut.ReasonPhrase, desEx.ReasonPhrase);
                Assert.Equal(sut.Message, desEx.Message);
                Assert.Equal(sut.ToString(), desEx.ToString());
            }

            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, sut.StatusCode);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf415_Json()
        {
            var sut1 = new UnsupportedMediaTypeException();
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<UnsupportedMediaTypeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"{
  ""type"": ""Cuemon.AspNetCore.Http.UnsupportedMediaTypeException"",
  ""message"": ""The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method."",
  ""statusCode"": 415,
  ""reasonPhrase"": ""Unsupported Media Type""
}", sut4);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf415_Xml()
        {
            var sut1 = new UnsupportedMediaTypeException();
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<UnsupportedMediaTypeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<UnsupportedMediaTypeException namespace=""Cuemon.AspNetCore.Http"">
	<Message>The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.</Message>
	<StatusCode>415</StatusCode>
	<ReasonPhrase>Unsupported Media Type</ReasonPhrase>
</UnsupportedMediaTypeException>", sut4);
        }
    }
}