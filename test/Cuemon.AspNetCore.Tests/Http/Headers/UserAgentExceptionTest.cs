﻿using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Http.Headers
{
    public class UserAgentExceptionTest : Test
    {
        public UserAgentExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void UserAgentException_ShouldBeSerializable_Json()
        {
            var sut1 = new UserAgentException(400, "Bad Request.");
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<UserAgentException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(StatusCodes.Status400BadRequest, sut1.StatusCode);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal("""
                         {
                           "type": "Cuemon.AspNetCore.Http.Headers.UserAgentException",
                           "message": "Bad Request.",
                           "headers": {},
                           "statusCode": 400,
                           "reasonPhrase": "Bad Request"
                         }
                         """.ReplaceLineEndings(), sut4);
        }

        [Fact]
        public void UserAgentException_ShouldBeSerializable_Xml()
        {
            var sut1 = new UserAgentException(400, "Bad Request.");
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<UserAgentException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.StatusCode, original.StatusCode);
            Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
            Assert.Equal(StatusCodes.Status400BadRequest, sut1.StatusCode);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal("""
                         <?xml version="1.0" encoding="utf-8"?>
                         <UserAgentException namespace="Cuemon.AspNetCore.Http.Headers">
                         	<Message>Bad Request.</Message>
                         	<Headers />
                         	<StatusCode>400</StatusCode>
                         	<ReasonPhrase>Bad Request</ReasonPhrase>
                         </UserAgentException>
                         """.ReplaceLineEndings(), sut4);
        }
    }
}
