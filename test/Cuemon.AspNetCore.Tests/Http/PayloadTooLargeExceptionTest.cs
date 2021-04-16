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
    public class PayloadTooLargeExceptionTest : Test
    {
        public PayloadTooLargeExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf413()
        {
            var sut = new PayloadTooLargeException();

            TestOutput.WriteLine(sut.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, sut);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as PayloadTooLargeException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                Assert.Equal(sut.StatusCode, desEx.StatusCode);
                Assert.Equal(sut.ReasonPhrase, desEx.ReasonPhrase);
                Assert.Equal(sut.Message, desEx.Message);
                Assert.Equal(sut.ToString(), desEx.ToString());
            }

            Assert.Equal(StatusCodes.Status413PayloadTooLarge, sut.StatusCode);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf413_Json()
        {
            var sut1 = new PayloadTooLargeException();
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<PayloadTooLargeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status413PayloadTooLarge, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"{
  ""type"": ""Cuemon.AspNetCore.Http.PayloadTooLargeException"",
  ""message"": ""The server is refusing to process a request because the request entity is larger than the server is willing or able to process."",
  ""statusCode"": 413,
  ""reasonPhrase"": ""Payload Too Large""
}", sut4);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf413_Xml()
        {
            var sut1 = new PayloadTooLargeException();
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<PayloadTooLargeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status413PayloadTooLarge, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<PayloadTooLargeException namespace=""Cuemon.AspNetCore.Http"">
	<Message>The server is refusing to process a request because the request entity is larger than the server is willing or able to process.</Message>
	<StatusCode>413</StatusCode>
	<ReasonPhrase>Payload Too Large</ReasonPhrase>
</PayloadTooLargeException>", sut4);
        }
    }
}