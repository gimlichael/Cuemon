using System;
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
    public class BadRequestExceptionTest : Test
    {
        public BadRequestExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf400()
        {
            var sut = new BadRequestException();

            TestOutput.WriteLine(sut.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, sut);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as BadRequestException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete 
                Assert.Equal(sut.StatusCode, desEx.StatusCode);
                Assert.Equal(sut.ReasonPhrase, desEx.ReasonPhrase);
                Assert.Equal(sut.Message, desEx.Message);
                Assert.Equal(sut.ToString(), desEx.ToString());
            }

            Assert.Equal(StatusCodes.Status400BadRequest, sut.StatusCode);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf400_Json()
        {
            var sut1 = new BadRequestException();
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<BadRequestException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status400BadRequest, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());
            
            Assert.Equal(@"{
  ""type"": ""Cuemon.AspNetCore.Http.BadRequestException"",
  ""message"": ""The request could not be understood by the server due to malformed syntax."",
  ""statusCode"": 400,
  ""reasonPhrase"": ""Bad Request""
}", sut4);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf400_Xml()
        {
            var sut1 = new BadRequestException();
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<BadRequestException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status400BadRequest, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());
            
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<BadRequestException namespace=""Cuemon.AspNetCore.Http"">
	<Message>The request could not be understood by the server due to malformed syntax.</Message>
	<StatusCode>400</StatusCode>
	<ReasonPhrase>Bad Request</ReasonPhrase>
</BadRequestException>", sut4);
        }
    }
}