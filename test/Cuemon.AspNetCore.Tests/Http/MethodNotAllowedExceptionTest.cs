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
    public class MethodNotAllowedExceptionTest : Test
    {
        public MethodNotAllowedExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf405()
        {
            var sut = new MethodNotAllowedException();

            TestOutput.WriteLine(sut.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, sut);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as MethodNotAllowedException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                Assert.Equal(sut.StatusCode, desEx.StatusCode);
                Assert.Equal(sut.ReasonPhrase, desEx.ReasonPhrase);
                Assert.Equal(sut.Message, desEx.Message);
                Assert.Equal(sut.ToString(), desEx.ToString());
            }

            Assert.Equal(StatusCodes.Status405MethodNotAllowed, sut.StatusCode);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf405_Json()
        {
            var sut1 = new MethodNotAllowedException();
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<MethodNotAllowedException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status405MethodNotAllowed, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"{
  ""type"": ""Cuemon.AspNetCore.Http.MethodNotAllowedException"",
  ""message"": ""The method specified in the request is not allowed for the resource identified by the request URI."",
  ""statusCode"": 405,
  ""reasonPhrase"": ""Method Not Allowed""
}", sut4);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf405_Xml()
        {
            var sut1 = new MethodNotAllowedException();
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<MethodNotAllowedException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status405MethodNotAllowed, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<MethodNotAllowedException namespace=""Cuemon.AspNetCore.Http"">
	<Message>The method specified in the request is not allowed for the resource identified by the request URI.</Message>
	<StatusCode>405</StatusCode>
	<ReasonPhrase>Method Not Allowed</ReasonPhrase>
</MethodNotAllowedException>", sut4);
        }
    }
}