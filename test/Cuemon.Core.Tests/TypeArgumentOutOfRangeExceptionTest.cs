using System;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Cuemon.Xml.Serialization.Formatters;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class TypeArgumentOutOfRangeExceptionTest : Test
    {
        public TypeArgumentOutOfRangeExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TypeArgumentOutOfRangeException_ShouldBeSerializable_Json()
        {
            var randomParamName = Generate.RandomString(10);
            var actualValue = 42;
            var randomMessage = Generate.RandomString(50);
            var sut1 = new TypeArgumentOutOfRangeException(randomParamName, 42, randomMessage);
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<TypeArgumentOutOfRangeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.ParamName, original.ParamName);
            Assert.Equal(sut1.ActualValue!.ToString(), original.ActualValue!.ToString());
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());

#if NET48_OR_GREATER
            Assert.Equal($$"""
                         {
                           "type": "Cuemon.TypeArgumentOutOfRangeException",
                           "message": "{{randomMessage}}\r\nParameter name: {{randomParamName}}\r\nActual value was {{actualValue}}.",
                           "actualValue": {{actualValue}},
                           "paramName": "{{randomParamName}}"
                         }
                         """, sut4);
#else
            Assert.Equal($$"""
                         {
                           "type": "Cuemon.TypeArgumentOutOfRangeException",
                           "message": "{{randomMessage}} (Parameter '{{randomParamName}}')\r\nActual value was {{actualValue}}.",
                           "actualValue": {{actualValue}},
                           "paramName": "{{randomParamName}}"
                         }
                         """, sut4, ignoreLineEndingDifferences: true);
#endif
        }

        [Fact]
        public void TypeArgumentOutOfRangeException_ShouldBeSerializable_Xml()
        {
            var sut1 = new TypeArgumentOutOfRangeException(Generate.RandomString(10), 42, Generate.RandomString(50));
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<TypeArgumentOutOfRangeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.ParamName, original.ParamName);
            Assert.Equal(sut1.ActualValue, original.ActualValue);
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<TypeArgumentOutOfRangeException namespace=""Cuemon"">
	<Message>XMo4qZhzTYqJsxwv2M7h9TZKf9c8alGmOyodlDYuzNsNmNsYIU (Parameter '0QsNxuz6LR')
Actual value was 42.</Message>
	<ParamName>0QsNxuz6LR</ParamName>
</TypeArgumentOutOfRangeException>", sut4);
        }
    }
}
