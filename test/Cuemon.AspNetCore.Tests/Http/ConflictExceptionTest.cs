using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Http
{
    public class ConflictExceptionTest : Test
    {
        public ConflictExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf409_Json()
        {
            var sut1 = new ConflictException();
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<ConflictException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status409Conflict, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal("""
                         {
                           "type": "Cuemon.AspNetCore.Http.ConflictException",
                           "message": "The request could not be completed due to a conflict with the current state of the resource.",
                           "headers": {},
                           "statusCode": 409,
                           "reasonPhrase": "Conflict"
                         }
                         """.ReplaceLineEndings(), sut4);
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf409_Xml()
        {
            var sut1 = new ConflictException();
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<ConflictException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(StatusCodes.Status409Conflict, sut1.StatusCode);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal("""
                         <?xml version="1.0" encoding="utf-8"?>
                         <ConflictException namespace="Cuemon.AspNetCore.Http">
                         	<Message>The request could not be completed due to a conflict with the current state of the resource.</Message>
                         	<Headers />
                         	<StatusCode>409</StatusCode>
                         	<ReasonPhrase>Conflict</ReasonPhrase>
                         </ConflictException>
                         """.ReplaceLineEndings(), sut4);
        }
    }
}